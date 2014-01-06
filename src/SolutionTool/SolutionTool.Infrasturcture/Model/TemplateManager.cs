using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

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
                    files.AddRange(System.IO.Directory.GetFiles(_dir, "*.xml").Select(x => new System.IO.FileInfo(x).Name));
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

        public void LoadTemplate(string templateFileName, Action<string, Exception> onComplete)
        {
            var exception = null as Exception;
            var directory = null as string;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    directory = System.IO.File.ReadAllText(path);
                }
                catch (Exception xe)
                {
                    exception = xe;
                }
            }

            if (onComplete != null)
            {
                onComplete(directory, exception);
            }
        }

        public void SaveTemplate(string templateFileName, string templateXmlContent, Action<bool, Exception> onComplete)
        {
            var exception = null as Exception;
            var isOk = true;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            ValidateTemplate(templateXmlContent, (x, y) => 
            {
                if (!x)
                {
                    isOk = false;
                    exception = new Exception("Not a valid template file! ");
                }

                try
                {
                    System.IO.File.WriteAllText(path, templateXmlContent);
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

            });
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


        public void ValidateTemplate(string templateXmlContent, Action<bool, Exception> onComplete)
        {
            var exception = null as Exception;
            var isValid = false;

            try
            {
                using (var sr = new System.IO.StringReader(templateXmlContent))
                {
                    var xs = new XmlSerializer(typeof(Directory));

                    var directory = xs.Deserialize(sr) as Directory;

                    if (directory != null)
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception xe)
            {
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete(isValid, exception);
            }
        }
    }
}
