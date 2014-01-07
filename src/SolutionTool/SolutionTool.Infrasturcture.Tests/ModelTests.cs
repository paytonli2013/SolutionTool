using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.SolutionTool.Model;

namespace SolutionTool.Infrasturcture.Tests
{
    [TestClass]
    public class ModelTests
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void TestSerializeDirectory()
        {
            var dir = new Directory
            {
                Name = "root",
                SubDirectories = new List<Directory>
                {
                    new Directory 
                    { 
                        Name = "src",
                    },
                    new Directory 
                    { 
                        Name = "output",
                        SubDirectories = new List<Directory>
                        {
                            new Directory 
                            { 
                                Name = "debug",
                                SubDirectories = new List<Directory>
                                {
                                    new Directory 
                                    { 
                                        Name = ".NET35",
                                    },
                                    new Directory 
                                    { 
                                        Name = ".NET40",
                                    },
                                },
                            },
                            new Directory 
                            { 
                                Name = "release",
                                SubDirectories = new List<Directory>
                                {
                                    new Directory 
                                    { 
                                        Name = ".NET35",
                                    },
                                    new Directory 
                                    { 
                                        Name = ".NET40",
                                    },
                                },
                            },
                        },
                    },
                    new Directory 
                    { 
                        Name = "doc", 
                    },
                    new Directory 
                    { 
                        Name = "deployment", 
                    },
                },
                Files = new List<File>
                {
                    new File { Name = "Readme.md", },
                    new File { Name = ".gitignore", },
                },
            };

            var xs = new XmlSerializer(typeof(Directory));

            using (var ms = new System.IO.MemoryStream())
            {
                xs.Serialize(ms, dir);

                ms.Seek(0, System.IO.SeekOrigin.Begin);

                using (var sr = new System.IO.StreamReader(ms))
                {
                    var s = sr.ReadToEnd();

                    TestContext.WriteLine(s);
                }
            }
        }

        [TestMethod]
        public void TestDeserializeDirectory()
        {
            var xs = new XmlSerializer(typeof(Directory));
            var xml1 = @"<dir name='root'/>";
            var xml2 = @"<dir name='root'><dir name='src'/><file name='readme.md'/></dir>";
            var xml3 = @"<?xml version='1.0'?>
<dir name='root'>
  <dir name='src' />
  <dir name='output'>
    <dir name='debug'>
      <dir name='.NET35' />
      <dir name='.NET40' />
    </dir>
    <dir name='release'>
      <dir name='.NET35' />
      <dir name='.NET40' />
    </dir>
  </dir>
  <dir name='doc' />
  <dir name='deployment' />
  <file name='Readme.md' />
  <file name='.gitignore' />
</dir>";

            using (var sr = new System.IO.StringReader(xml1))
            {
                var dir = xs.Deserialize(sr) as Directory;

                Assert.IsNotNull(dir);
                Assert.AreEqual(dir.Name, "root");
            }

            using (var sr = new System.IO.StringReader(xml2))
            {
                var dir = xs.Deserialize(sr) as Directory;

                Assert.IsNotNull(dir);
                Assert.AreEqual(dir.Name, "root");
                Assert.IsNotNull(dir.SubDirectories);
                Assert.AreEqual(dir.SubDirectories.Count, 1);
                Assert.IsNotNull(dir.Files);
                Assert.AreEqual(dir.Files.Count, 1);
            }

            using (var sr = new System.IO.StringReader(xml3))
            {
                var dir = xs.Deserialize(sr) as Directory;

                Assert.IsNotNull(dir);
                Assert.AreEqual(dir.Name, "root");
                Assert.IsNotNull(dir.SubDirectories);
                Assert.AreEqual(dir.SubDirectories.Count, 4);
                Assert.IsNotNull(dir.Files);
                Assert.AreEqual(dir.Files.Count, 2);
                Assert.IsNotNull(dir.SubDirectories
                    .FirstOrDefault(x => x.Name == "output"));
                Assert.IsNotNull(dir.SubDirectories
                    .FirstOrDefault(x => x.Name == "output").SubDirectories
                    .FirstOrDefault(x => x.Name == "debug"));
                Assert.IsNotNull(dir.SubDirectories
                    .FirstOrDefault(x => x.Name == "output").SubDirectories
                    .FirstOrDefault(x => x.Name == "debug").SubDirectories
                    .FirstOrDefault(x => x.Name == ".NET40"));
            }
        }

        [TestMethod]
        public void TestSerializeRuleSet()
        {
            var ruleSet = new Orc.SolutionTool.Model.Rules.RuleSet();
            var ruleSetXml = null as string;

            ruleSet.Add(new Orc.SolutionTool.Model.Rules.FileStructureRule() { Template = "default.xml", });
            ruleSet.Add(new Orc.SolutionTool.Model.Rules.OutputPathRule() { path = "./output/{{active_solution}}/", });
            ruleSet.Add(new Orc.SolutionTool.Model.Rules.CodeAnalysisRule());

            var xs = new XmlSerializer(typeof(Orc.SolutionTool.Model.Rules.RuleSet));

            using (var ms = new System.IO.MemoryStream())
            {
                xs.Serialize(ms, ruleSet);

                ms.Seek(0, System.IO.SeekOrigin.Begin);

                using (var sr = new System.IO.StreamReader(ms))
                {
                    ruleSetXml = sr.ReadToEnd();

                    TestContext.WriteLine(ruleSetXml);
                }
            }
        }

        [TestMethod]
        public void TestDeserializeRuleSet()
        {
            var ruleSetXml = @"<?xml version='1.0'?>
<ruleSet>
  <fileStructure enabled='false' template='default.xml' />
  <outputPath enabled='true' path='./output/{active_solution}/' />
  <codeAnalysis enabled='true' />
</ruleSet>
";
            var xs1 = new XmlSerializer(typeof(Orc.SolutionTool.Model.Rules.RuleSet));
            
            using (var sr = new System.IO.StringReader(ruleSetXml))
            {
                var rules = xs1.Deserialize(sr) as Orc.SolutionTool.Model.Rules.RuleSet;

                Assert.IsNotNull(rules);
                Assert.AreEqual(rules.Count, 3);
                Assert.IsInstanceOfType(rules[0], typeof(Orc.SolutionTool.Model.Rules.FileStructureRule));
                Assert.IsInstanceOfType(rules[1], typeof(Orc.SolutionTool.Model.Rules.OutputPathRule));
                Assert.IsInstanceOfType(rules[2], typeof(Orc.SolutionTool.Model.Rules.CodeAnalysisRule));
                Assert.AreEqual(((Orc.SolutionTool.Model.Rules.FileStructureRule)rules[0]).IsEnabled, false);
                Assert.AreEqual(((Orc.SolutionTool.Model.Rules.FileStructureRule)rules[0]).Template, "default.xml");
            }

        }
    }
}
