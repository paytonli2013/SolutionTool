using System;
using System.Linq;
using System.Collections.Generic;
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

        public void LoadTemplate(string templateFileName, Action<Repository, Exception> onComplete)
        {
            var exception = null as Exception;
            var repository = null as Repository;
            var path = System.IO.Path.Combine(_dir, templateFileName);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    using (var fs = System.IO.File.OpenRead(path))
                    {
                        var xs = new XmlSerializer(typeof(Repository));

                        repository = xs.Deserialize(fs) as Repository;
                    }
                }
                catch (Exception xe)
                {
                    exception = xe;
                }
            }

            if (onComplete != null)
            {
                onComplete(repository, exception);
            }
        }

        public void SaveTemplate(string templateFileName, Repository repository, Action<bool, Exception> onComplete)
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
                using (var sw = System.IO.File.CreateText(path))
                {
                    var xs = new XmlSerializer(typeof(Repository));

                    xs.Serialize(sw, repository);
                }
            }
            catch (Exception xe)
            {
                exception = xe;
            }

            if (onComplete != null)
            {
                onComplete(isOk, exception);
            }
        }
    }
}
