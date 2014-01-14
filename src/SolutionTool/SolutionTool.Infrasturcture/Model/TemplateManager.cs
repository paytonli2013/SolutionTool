using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orc.SolutionTool.Model
{
    public class TemplateManager : ITemplateManager
    {
        private static readonly string _dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Templates\");

        public void GetAllTemplateFileNames(Action<IEnumerable<string>, Exception> onComplate)
        {
            var exception = null as Exception;
            var files = new List<string>();

            if (Directory.Exists(_dir))
            {
                try
                {
                    files.AddRange(Directory.GetFiles(_dir, "*.txt").Select(x => new FileInfo(x).Name));
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
            var path = Path.Combine(_dir, templateFileName);

            if (File.Exists(path))
            {
                try
                {
                    var fsd = new FsDirectives();
                    
                    txt = File.ReadAllText(path);
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
            var path = Path.Combine(_dir, templateFileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            try
            {
                File.WriteAllText(path, templateContent);
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
            var path = Path.Combine(_dir, templateFileName);

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
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
