using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Orc.SolutionTool.Model
{
    //[DebuggerDisplay("{Directives.Count}")]
    public class FsDirectives
    {
        private List<Directive> _directives;
        public List<Directive> Directives
        {
            get
            {
                return _directives;
            }
            private set
            {
                if (_directives != value)
                {
                    _directives = value;
                }
            }
        }

        public FsDirectives()
        {

        }

        public void Execute(string repository, ref Dictionary<Directive, List<string>> outputs)
        {
            foreach (var i in Directives)
            {
                var results = null as List<string>;

                if (outputs.ContainsKey(i))
                {
                    results = outputs[i];
                }
                else
                {
                    results = new List<string>();

                    outputs.Add(i, results);
                }

                i.Execute(repository, ref results);
            }
        }

        public void ParseFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                Directives = new List<Directive>();

                using (var fr = System.IO.File.OpenText(path))
                {
                    var line = fr.ReadLine();

                    while (line != null)
                    {
                        var dx = Directive.Parse(line);

                        if (dx != null)
                        {
                            Directives.Add(dx);
                        }

                        line = fr.ReadLine();
                    }
                }
            }
        }

        public void ParseText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var lines = text.Split((";" + Environment.NewLine).ToArray());
                Directives = new List<Directive>();

                foreach (var line in lines)
                {
                    var dx = Directive.Parse(line);

                    if (dx != null)
                    {
                        Directives.Add(dx);
                    }
                }
            }
        }
    }

    [DebuggerDisplay("{GetType().Name}({Pattern},{Exclude})")]
    public abstract class Directive : IEquatable<Directive>, IComparable<Directive>, IComparable
    {
        private string _pattern;
        public string Pattern
        {
            get
            {
                return _pattern;
            }
            private set
            {
                if (_pattern != value)
                {
                    _pattern = value;
                }
            }
        }

        private bool _exclude;
        public bool Exclude
        {
            get
            {
                return _exclude;
            }
            private set
            {
                if (_exclude != value)
                {
                    _exclude = value;
                }
            }
        }

        protected Directive()
        {

        }

        public virtual void Execute(string repository, ref List<string> outputs)
        {
            //var output = "Executing " + GetType().Name + ": " + repository;

            //outputs.Add(output);
        }

        public static Directive Parse(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return null;
            }

            var rv = null as Directive;
            var px = pattern.Trim();

            #region Pattern is invalid.

            if (px[0] == '#')
            {
                rv = new CommentDirective
                {
                    Pattern = px,
                };

                return rv;
            }

            if (px[0] == '/' || px[0] == '\\' || px[0] == '!' && (px.Length == 1 || px[1] == '/' || px[1] == '\\'))
            {
                rv = new InvalidDirective
                {
                    Pattern = px,
                };

                return rv;
            }

            var cc = System.IO.Path.GetInvalidPathChars();

            foreach (var i in cc)
            {
                if (i == '*')
                {
                    continue;
                }

                if (px.Contains(i))
                {
                    rv = new InvalidDirective
                    {
                        Pattern = px,
                    };

                    return rv;
                }
            }

            var segs = px.TrimStart('!').Split('/', '\\');

            for (int i = 0; i < segs.Length; i++)
            {
                var seg = segs[i];

                if (string.IsNullOrWhiteSpace(seg) && (i < segs.Length - 1) || seg.IndexOf("**") > -1 && seg.Any(x => x != '*'))
                {
                    rv = new InvalidDirective
                    {
                        Pattern = px,
                    };

                    return rv;
                }
            }

            #endregion

            var lx = px.Length - 1;
            var ex = px[0] == '!';
            var dr = px[lx] == '/' || px[lx] == '\\';

            if (dr)
            {
                rv = new FolderDirective
                {
                    Pattern = px,
                    Exclude = ex,
                };
            }
            else
            {
                rv = new FileDirective
                {
                    Pattern = px,
                    Exclude = ex,
                };
            }

            return rv;
        }

        public bool Equals(Directive other)
        {
            if (other == null)
            {
                return false;
            }

            return Pattern == other.Pattern;
        }

        public int CompareTo(Directive other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Pattern.CompareTo(other.Pattern);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Directive);
        }
    }

    public sealed class CommentDirective : Directive
    {
        internal CommentDirective() { }

        public override void Execute(string repository, ref List<string> outputs)
        {
            // Do nothing with a invalid directive.
            outputs.Add("Escape checking a comment pattern. ");
        }
    }

    public sealed class InvalidDirective : Directive
    {
        internal InvalidDirective() { }

        public override void Execute(string repository, ref List<string> outputs)
        {
            // Do nothing with a invalid directive.
            outputs.Add("Escape checking an invalid pattern. ");
        }
    }

    public sealed class FolderDirective : Directive
    {
        internal FolderDirective() { }

        public override void Execute(string repository, ref List<string> outputs)
        {
            var relPath = Exclude ? Pattern.Substring(1) : Pattern;
            var recursive = relPath.IndexOf('*') > -1;

            if (recursive)
            {
                var segs = relPath.Split('/', '\\');
                var allDirectories = false;

                for (int i = 0; i < segs.Length; i++)
                {
                    var sub = string.Join("\\", segs.Take(i + 1));
                    var star = sub.IndexOf("*") > -1;

                    if (star)
                    {
                        allDirectories = segs[i].IndexOf("**") > -1 && i < segs.Length - 1;
                        sub = string.Join("\\", i == 0 ? "." : segs[i - 1]);

                        var path = System.IO.Path.Combine(repository, sub);
                        var di = new System.IO.DirectoryInfo(path);

                        if (allDirectories)
                        {
                            ExecuteAllDirectories(repository, ref outputs, i + 1, di);

                            break;
                        }
                        else
                        {
                            var dc = di.GetDirectories(segs[i]);

                            if (Exclude && dc.Length > 0)
                            {
                                foreach (var j in dc)
                                {
                                    Execute(repository, ref outputs, i + 1, j);
                                }
                            }
                            else if (!Exclude && dc.Length == 0)
                            {
                                var output = "[" + di.FullName + "] does not contain any sub directories that matches [" + segs[i] + "]. ";

                                outputs.Add(output);

                                break;
                            }
                        }
                    }
                    else
                    {
                        var path = System.IO.Path.Combine(repository, sub);
                        var exist = System.IO.Directory.Exists(path);

                        if (exist && Exclude || !exist && !Exclude)
                        {
                            var s = System.IO.Path.GetFullPath(path);

                            outputs.Add(s);

                            break;
                        }
                    }
                }
            }
            else
            {
                var path = System.IO.Path.Combine(repository, relPath);
                var exist = System.IO.Directory.Exists(path);

                if (exist && Exclude || !exist && !Exclude)
                {
                    var s = System.IO.Path.GetFullPath(path);

                    outputs.Add(s);
                }
            }
        }

        private void Execute(string repository, ref List<string> outputs, int level, System.IO.DirectoryInfo directoryInfo)
        {
            var relPath = Exclude ? Pattern.Substring(1) : Pattern;
            var segs = relPath.Split('/', '\\');

            if (level >= segs.Length)
            {
                return;
            }
            else
            {
                var star = segs[level].IndexOf("*") > -1;
                var allDirectories = segs[level].IndexOf("**") > -1;

                if (star)
                {
                    if (allDirectories)
                    {
                        ExecuteAllDirectories(repository, ref outputs, level + 1, directoryInfo);
                    }
                    else
                    {
                        AddFolderMustNotExists(ref outputs, directoryInfo, segs[level]);
                    }
                }
                else
                {
                    if (level == segs.Length - 1)
                    {
                        AddFolderMustNotExists(ref outputs, directoryInfo, segs[level]);
                    }
                    else
                    {
                        var dc = directoryInfo.GetDirectories(segs[level]);

                        foreach (var i in dc)
                        {
                            Execute(repository, ref outputs, level + 1, i);
                        }
                    }
                }
            }
        }

        private void ExecuteAllDirectories(string repository, ref List<string> outputs, int level, System.IO.DirectoryInfo directoryInfo)
        {
            var relPath = Exclude ? Pattern.Substring(1) : Pattern;
            var segs = relPath.Split('/', '\\');

            if (level >= segs.Length)
            {
                return;
            }
            else
            {
                var star = segs[level].IndexOf("*") > -1;
                var allDirectories = segs[level].IndexOf("**") > -1;

                if (star)
                {
                    var dc = directoryInfo.GetDirectories(allDirectories ? "*" : segs[level]);

                    foreach (var i in dc)
                    {
                        ExecuteAllDirectories(repository, ref outputs, level + 1, i);
                    }
                }
                else
                {
                    var dc = directoryInfo.GetDirectories(segs[level]);

                    if (Exclude && dc.Length > 0)
                    {
                        foreach (var i in dc)
                        {
                            Execute(repository, ref outputs, level + 1, i);
                        }
                    }
                    else if (!Exclude && dc.Length == 0)
                    {
                        var output = "[" + directoryInfo.FullName + "] does not contain any sub directories with the name [" + segs[level] + "]. ";

                        outputs.Add(output);

                        return;
                    }
                }
            }

        }

        private void AddFolderMustNotExists(ref List<string> outputs, System.IO.DirectoryInfo directoryInfo, string pattern)
        {
            var dc = directoryInfo.GetDirectories(pattern);

            if (Exclude && dc.Length > 0)
            {
                foreach (var i in dc)
                {
                    var output = i.FullName;

                    outputs.Add(output);
                }

                return;
            }
            else if (!Exclude && dc.Length == 0)
            {
                var output = "[" + directoryInfo.FullName + "] does not contain any sub directories that matches [" + pattern + "]. ";

                outputs.Add(output);

                return;
            }

            return;
        }
    }

    public sealed class FileDirective : Directive
    {
        internal FileDirective() { }

        public override void Execute(string repository, ref List<string> outputs)
        {
            var relPath = Exclude ? Pattern.Substring(1) : Pattern;
            var recursive = relPath.IndexOf('*') > -1;

            if (recursive)
            {
                var segs = relPath.Split('/', '\\');
                var allDirectories = false;

                for (int i = 0; i < segs.Length; i++)
                {
                    var sub = string.Join("\\", segs.Take(i + 1));
                    var star = sub.IndexOf("*") > -1;

                    if (star)
                    {
                        allDirectories = segs[i].IndexOf("**") > -1 && i < segs.Length - 1;
                        sub = string.Join("\\", segs[i - 1]);

                        var path = System.IO.Path.Combine(repository, sub);
                        var di = new System.IO.DirectoryInfo(path);

                        if (allDirectories)
                        {
                            Execute(repository, ref outputs, i + 1, di);

                            break;
                        }
                        else
                        {
                            var dc = di.GetDirectories(segs[i]);

                            if (Exclude && dc.Length > 0)
                            {
                                foreach (var j in dc)
                                {
                                    Execute(repository, ref outputs, i + 1, j);
                                }

                                break;
                            }
                            else if (!Exclude && dc.Length == 0)
                            {
                                var output = "[" + di.FullName + "] does not contain any sub directories that matches [" + segs[i] + "]. ";

                                outputs.Add(output);

                                break;
                            }
                        }
                    }
                    else
                    {
                        var path = System.IO.Path.Combine(repository, sub);
                        var exist = System.IO.Directory.Exists(path);

                        if (exist && Exclude || !exist && !Exclude)
                        {
                            var s = System.IO.Path.GetFullPath(path);

                            outputs.Add(s);

                            break;
                        }
                    }
                }
            }
            else
            {
                var path = System.IO.Path.Combine(repository, relPath);
                var exist = System.IO.File.Exists(path);

                if (exist && Exclude || !exist && !Exclude)
                {
                    var s = System.IO.Path.GetFullPath(path);

                    outputs.Add(s);
                }
            }
        }

        private void Execute(string repository, ref List<string> outputs, int level, System.IO.DirectoryInfo directoryInfo)
        {
            var relPath = Exclude ? Pattern.Substring(1) : Pattern;
            var segs = relPath.Split('/', '\\');

            if (level >= segs.Length)
            {
                return;
            }
            else
            {
                var star = segs[level].IndexOf("*") > -1;
                var allFiles = segs[level].IndexOf("**") > -1 && level == segs.Length - 1;
                var allDirectories = segs[level].IndexOf("**") > -1 && level < segs.Length - 1;

                if (star)
                {
                    if (allFiles)
                    {
                        if (Exclude)
                        {
                            AddFileMustNotExists(ref outputs, directoryInfo, true);
                        }
                    }
                    else if (allDirectories)
                    {
                        var dc = directoryInfo.GetDirectories();

                        foreach (var i in dc)
                        {
                            Execute(repository, ref outputs, level + 1, i);
                        }
                    }
                    else
                    {
                        AddFileMustNotExists(ref outputs, directoryInfo, segs[level]);
                    }
                }
                else
                {
                    if (level == segs.Length - 1)
                    {
                        AddFileMustNotExists(ref outputs, directoryInfo, segs[level]);
                    }
                    else
                    {
                        var dc = directoryInfo.GetDirectories(segs[level]);

                        if (Exclude && dc.Length > 0)
                        {
                            foreach (var i in dc)
                            {
                                Execute(repository, ref outputs, level + 1, i);
                            }
                        }
                        else if (!Exclude && dc.Length == 0)
                        {
                            var output = "[" + directoryInfo.FullName + "] does not contain any sub directories with the name [" + segs[level] + "]. ";

                            outputs.Add(output);

                            return;
                        }
                    }
                }
            }
        }

        private void AddFileMustNotExists(ref List<string> outputs, System.IO.DirectoryInfo directoryInfo, string pattern)
        {
            var fc = directoryInfo.GetFiles(pattern);

            if (Exclude && fc.Length > 0)
            {
                foreach (var i in fc)
                {
                    var output = i.FullName;

                    outputs.Add(output);
                }

                return;
            }
            else if (!Exclude && fc.Length == 0)
            {
                var output = "[" + directoryInfo.FullName + "] does not contain any files that matches [" + pattern + "]. ";

                outputs.Add(output);

                return;
            }

            return;
        }

        private void AddFileMustNotExists(ref List<string> outputs, System.IO.DirectoryInfo directoryInfo, bool recursive)
        {
            var dc = directoryInfo.GetDirectories();

            if (recursive)
            {
                foreach (var i in dc)
                {
                    AddFileMustNotExists(ref outputs, i, recursive);
                }
            }

            var fc = directoryInfo.GetFiles();

            foreach (var i in fc)
            {
                var output = i.FullName;

                outputs.Add(output);
            }
        }
    }
}
