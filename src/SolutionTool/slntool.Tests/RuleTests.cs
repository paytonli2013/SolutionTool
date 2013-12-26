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

        }
    }
}
