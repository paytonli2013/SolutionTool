using System;
using System.Collections.Generic;
using System.Linq;

namespace Orc.SolutionTool.Model
{
    public class TemplateManager : ITemplateManager
    {
        private static readonly string _dir = System.IO.Path.Combine(Environment.CurrentDirectory, @".\Templates\");

        public void GetAllTemplateFileNames(Action<IEnumerable<string>, Exception> onComplate)
        {
            var exception = null as Exception;
            var files = new List<string>();

            if (System.IO.Directory.Exists(_dir))
            {
                try
                {
                    files.AddRange(System.IO.Directory.GetFiles(_dir, "*.txt").Select(x => new System.IO.FileInfo(x).Name));
                }
                catch (Exception xe)
                {
                    exception = xe;
                }
            }

            if (onComplate != null)
            {
                onComplate(files, exception);
            }
        }

        public void LoadTemplate(string templateFileName, Action<FsDirectives, string, Exception> onComplete)
        {
            var exception = null as Exception;
            var fsDirectives = null as FsDirectives;
            var txt = null as string;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    var fsd = new FsDirectives();
                    
                    txt = System.IO.File.ReadAllText(path);
                    fsd.ParseText(txt);
                    fsDirectives = fsd;
                }
                catch (Exception xe)
                {
                    exception = xe;
                }
            }

            if (onComplete != null)
            {
                onComplete(fsDirectives, txt, exception);
            }
        }

        public void SaveTemplate(string templateFileName, string templateContent, Action<bool, Exception> onComplete)
        {
            var exception = null as Exception;
            var isOk = true;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            try
            {
                System.IO.File.WriteAllText(path, templateContent);
            }
            catch (Exception xe)
            {
                isOk = false;
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete(isOk, exception);
            }
        }

        public void DeleteTemplate(string templateFileName, Action<bool, Exception> onComplete)
        {
            var exception = null as Exception;
            var isOk = true;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception xe)
                {
                    exception = xe;
                    isOk = false;
                }
            }

            if (onComplete != null)
            {
                onComplete(isOk, exception);
            }
        }
    }
}
