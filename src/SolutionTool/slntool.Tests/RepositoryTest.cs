using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.SolutionTool.Model;
using Orc.SolutionTool.Model.Rules;

namespace slntool.Tests
{
    [TestClass]
    public class RepositoryTest
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
        public void TestCtor()
        {
            var repository = new Repository(@"..\");

            Assert.IsNotNull(repository.Target);
            Assert.AreNotEqual(repository.Target.Children.Count, 0);
        }

        [TestMethod]
        public void TestAddRule()
        {
            var repository = new Repository(@"..\");

            repository.AddRule("./Debug", new FolderMustExistsRule());
            repository.AddRule("./Debug/slntool.Tests.dll", new FileMustExistsRule());
            repository.AddRule("./Temp", new FolderMustExistsRule());
            repository.AddRule("./Temp/Temp.txt", new FileMustExistsRule());

            var target = repository.FindOrCreateTarget("./Debug");

            Assert.AreEqual("Debug", target.Name);

            var target1 = repository.FindOrCreateTarget("./Debug/slntool.Tests.dll");

            Assert.AreEqual("slntool.Tests.dll", target1.Name);
        }

        [TestMethod]
        public void TestExam()
        {
            var repository = new Repository(@"..\");
            var results = new List<Result>();

            repository.AddRule("./Debug", new FolderMustExistsRule());
            repository.AddRule("./Debug/slntool.Tests.dll", new FileMustExistsRule());
            repository.AddRule("./Temp", new FolderMustExistsRule());
            repository.AddRule("./Temp/Temp.txt", new FileMustExistsRule());
            repository.Exam(x => { results.Add(x); }, true);

            Assert.AreEqual(results.Count(x => x.Status == ActionStatus.Pass), 2);
            Assert.AreEqual(results.Count(x => x.Status == ActionStatus.Failed), 2);

            foreach (var i in results)
            {
                TestContext.WriteLine(i.ToString());
            }

            repository.RemoveRule("./Temp");
            repository.RemoveRule("./Temp/Temp.txt");
            results.Clear();
            repository.Exam(x => { results.Add(x); }, true);

            Assert.AreEqual(results.Count(x => x.Status == ActionStatus.Pass), 2);

            foreach (var i in results)
            {
                TestContext.WriteLine(i.ToString());
            }
        }

        [TestMethod]
        public void TestSerializeAndDeserialize()
        {
            var repository = new Repository(@"..\");

            repository.AddRule("./Debug", new FolderMustExistsRule());
            repository.AddRule("./Debug/slntool.Tests.dll", new FileMustExistsRule());
            repository.AddRule("./Temp", new FolderMustExistsRule());
            repository.AddRule("./Temp/Temp.txt", new FileMustExistsRule());

            //var s = ServiceStack.Text.XmlSerializer.SerializeToString(repository);

            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(typeof(Repository));

                xs.Serialize(ms, repository);

                ms.Seek(0, SeekOrigin.Begin);

                var sr = new StreamReader(ms);
                var s = sr.ReadToEnd();

                TestContext.WriteLine(s);
            }
        }
    }
}
