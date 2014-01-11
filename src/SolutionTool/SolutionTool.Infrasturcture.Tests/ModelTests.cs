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
        public void TestSerializeRuleSet()
        {
            var ruleSet = new Orc.SolutionTool.Model.RuleSet();
            var ruleSetXml = null as string;

            ruleSet.Add(new Orc.SolutionTool.Model.FileStructureRule() { Template = "default.xml", });
            ruleSet.Add(new Orc.SolutionTool.Model.OutputPathRule() { Path = "./output/{{active_solution}}/", });
            ruleSet.Add(new Orc.SolutionTool.Model.CodeAnalysisRule());

            var xs = new XmlSerializer(typeof(Orc.SolutionTool.Model.RuleSet));

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
            var xs1 = new XmlSerializer(typeof(Orc.SolutionTool.Model.RuleSet));
            
            using (var sr = new System.IO.StringReader(ruleSetXml))
            {
                var rules = xs1.Deserialize(sr) as Orc.SolutionTool.Model.RuleSet;

                Assert.IsNotNull(rules);
                Assert.AreEqual(rules.Count, 3);
                Assert.IsInstanceOfType(rules[0], typeof(Orc.SolutionTool.Model.FileStructureRule));
                Assert.IsInstanceOfType(rules[1], typeof(Orc.SolutionTool.Model.OutputPathRule));
                Assert.IsInstanceOfType(rules[2], typeof(Orc.SolutionTool.Model.CodeAnalysisRule));
                Assert.AreEqual(((Orc.SolutionTool.Model.FileStructureRule)rules[0]).IsEnabled, false);
                Assert.AreEqual(((Orc.SolutionTool.Model.FileStructureRule)rules[0]).Template, "default.xml");
            }

        }

        [TestMethod]
        public void TestFsDirectives()
        {
            var fsDirectives = new FsDirectives();
            var text1 = @"
#Default File Structure

#Directory must exist
src/
doc/
output/
output/debug/
output/release/
deployment/

#File must exist
.gitignore
readme.md
src/**/*.cs

#Directory must not exist
!**/bin/
!**/obj/

#File must not exist
";
            fsDirectives.ParseText(text1);

            Assert.IsNotNull(fsDirectives.Directives);
            Assert.AreNotEqual(fsDirectives.Directives.Count, 0);

            var outputs = new Dictionary<Directive, List<string>>();
            var path1 = System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\");
            var repository = System.IO.Path.GetFullPath(path1);

            fsDirectives.Execute(repository, ref outputs);

            Assert.AreNotEqual(outputs.Count, 0);

            foreach (var i in outputs)
            {
                TestContext.WriteLine(i.Key.Pattern);
                TestContext.WriteLine(new string('-', 80));

                foreach (var j in i.Value)
                {
                    TestContext.WriteLine(j);
                }

                if (i.Value.Count == 0)
                {
                    TestContext.WriteLine("Pass");
                }

                TestContext.WriteLine(Environment.NewLine);
            }
        }
    }
}
