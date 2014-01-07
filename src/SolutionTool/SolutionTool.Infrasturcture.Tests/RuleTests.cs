using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orc.SolutionTool.Model;

namespace SolutionTool.Infrasturcture.Tests
{
    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void TestFileStructureRule()
        {
            var project = new Project
            {
                Name = "TestProject1",
                Path = @"D:\",
            };
            var context = new ExamContext(project);
            var rule = new FileStructureRule { Template = "default.xml", IsEnabled = true, Name = "FileStructureRule", Description = "FileStructureRule Description" };

            rule.Exam(context);

            Assert.IsTrue(context.Outputs.Count > 0);
            Assert.IsTrue(context.Results.Count > 0);
        }
    }
}
