using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.SolutionTool.Core;
using Orc.SolutionTool.Core.Rules;

namespace slntool.Tests
{
    [TestClass]
    public class RuleTests
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
        public void TestCheckCsprojOutputPathRule()
        {
            var repository = new Repository(@"..\..\..\");
            var context = new Context(repository, null);
            var rule = new CheckCsprojOutputPathRule();

            rule.Exam(context, action);
        }

        private void action(Orc.SolutionTool.Core.ExamResult obj)
        {
            testContextInstance.WriteLine(obj.ToString());
        }

        [TestMethod]
        public void TestFolderMustExistsRule()
        {
            var repository = new Repository(@"..\");
            var context = new Context(repository, repository.Target);
            var rule = new FolderMustExistsRule();

            rule.Exam(context, action);
        }

        [TestMethod]
        public void TestFileMustExistsRule()
        {
            var repository = new Repository(@".\slntool.Tests.dll");
            var context = new Context(repository, repository.Target);
            var rule = new FileMustExistsRule();

            rule.Exam(context, action);
        }
    }
}
