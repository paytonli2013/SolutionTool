using System;

namespace Orc.SolutionTool
{
    public interface IChildViewService
    {
        void OpenChildView(IWindowHost host, string viewName, string title, Action<CloseResult> onClosed, ViewOptions option = null);
    }

    public class ViewOptions
    {
        public double Height { get; set; }

        public double Width { get; set; }

        public static ViewOptions Default = new ViewOptions() { Height = 800, Width = 600 };

        public object Payload { get; set; }
    }
}
