
namespace Orc.SolutionTool.Model
{
    public class FileStructureRuleExamContext : ExamContext
    {
        string _currentDirectory;

        public string CurrentDirctory
        {
            get { return _currentDirectory; }
        }

        ExamContext _context;

        public FileStructureRuleExamContext(ExamContext context, string currentDirectory)
        {
            _context = context;
            _currentDirectory = currentDirectory;
        }
    }
}
