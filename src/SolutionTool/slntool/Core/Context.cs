namespace Orc.SolutionTool.Core
{
    public class Context
    {
        public Repository Repository { get; private set; }
        public Target Target { get; private set; }

        public Context(Repository repository, Target target)
        {
            Repository = repository;
            Target = target;
        }
    }
}
