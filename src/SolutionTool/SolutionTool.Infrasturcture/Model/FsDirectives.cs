using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            if (File.Exists(path))
            {
                Directives = new List<Directive>();

                using (var fr = File.OpenText(path))
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

            var cc = Path.GetInvalidPathChars();

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
                var diMatched = new List<DirectoryInfo>();
                var diRoot = new DirectoryInfo(repository);
                var ix = 0;

                foreach (var i in segs)
                {
                    if (i.IndexOf("**") > -1)
                    {
                        break;
                    }

                    var p = Path.Combine(diRoot.FullName, i);

                    diRoot = new DirectoryInfo(p);
                    ix++;
                }

                var patterns = segs.Where((x, i) => x.IndexOf("**") == -1 & !string.IsNullOrWhiteSpace(x) && i > ix).ToArray();

                FsHelper.InterateAllDirectories(diRoot, ref diMatched, patterns);

                if (Exclude && diMatched.Count > 0)
                {
                    foreach (var i in diMatched)
                    {
                        var output = i.FullName;

                        outputs.Add(output);
                    }
                }
                else if (!Exclude && diMatched.Count == 0)
                {
                    var output = "[" + diRoot.FullName + "] does not contain any directories that matches this pattern. ";

                    outputs.Add(output);
                }
            }
            else
            {
                var path = Path.Combine(repository, relPath);
                var exist = Directory.Exists(path);

                if (exist && Exclude || !exist && !Exclude)
                {
                    var output = Path.GetFullPath(path);

                    outputs.Add(output);
                }
            }
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
                var diMatched = new List<DirectoryInfo>();
                var diRoot = new DirectoryInfo(repository);
                var ix = 0;

                foreach (var i in segs)
                {
                    if (i.IndexOf("**") > -1)
                    {
                        break;
                    }

                    var p = Path.Combine(diRoot.FullName, i);

                    diRoot = new DirectoryInfo(p);
                    ix++;
                }

                var patterns = segs.Where((x, i) => x.IndexOf("**") == -1 && i > ix && i != segs.Length - 1).ToArray();

                FsHelper.InterateAllDirectories(diRoot, ref diMatched, patterns);

                var lastSeg = segs[segs.Length - 1];
                var allFiles = lastSeg.IndexOf("**") > -1;

                foreach (var i in diMatched)
                {
                    if (allFiles)
                    {
                        var fiMatched = new List<FileInfo>();

                        FsHelper.InterateAllFiles(i, ref fiMatched, "*", true);

                        if (Exclude && fiMatched.Count > 0)
                        {
                            foreach (var j in fiMatched)
                            {
                                var output = j.FullName;

                                outputs.Add(output);
                            }
                        }
                        else if (!Exclude && fiMatched.Count == 0)
                        {
                            var output = "[" + i.FullName + "] does not contain any files that matches this pattern. ";

                            outputs.Add(output);
                        }
                    }
                    else
                    {
                        var fis = i.GetFiles(lastSeg);

                        if (Exclude && fis.Length > 0)
                        {
                            foreach (var j in fis)
                            {
                                var output = j.FullName;

                                outputs.Add(output);
                            }
                        }
                        else if (!Exclude && fis.Length == 0)
                        {
                            var output = "[" + i.FullName + "] does not contain any files that matches [" + lastSeg + "]. ";

                            outputs.Add(output);
                        }
                    }
                }
            }
            else
            {
                var path = Path.Combine(repository, relPath);
                var exist = File.Exists(path);

                if (exist && Exclude || !exist && !Exclude)
                {
                    var s = Path.GetFullPath(path);

                    outputs.Add(s);
                }
            }
        }
    }

    public static class FsHelper
    {
        class FileSystemInfoEqualityComparer : IEqualityComparer<FileSystemInfo>
        {
            public bool Equals(FileSystemInfo x, FileSystemInfo y)
            {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(FileSystemInfo obj)
            {
                return obj.FullName.GetHashCode();
            }
        }

        public static void InterateAllDirectories(DirectoryInfo diRoot, ref List<DirectoryInfo> diMatched, string[] patterns)
        {
            if (patterns.Length == 0)
            {
                InterateAllDirectories(diRoot, ref diMatched, (string)null);

                return;
            }

            var stack = new Stack<List<DirectoryInfo>>();
            var dis = new List<DirectoryInfo> { diRoot, };

            stack.Push(dis);

            foreach (var i in patterns)
            {
                dis = stack.Peek();

                var disx = new List<DirectoryInfo>();

                foreach (var j in dis)
                {
                    InterateAllDirectories(j, ref disx, i);
                }

                stack.Push(disx);
            }

            foreach (var i in stack.Peek())
            {
                if (!diMatched.Contains(i, new FileSystemInfoEqualityComparer()))
                {
                    diMatched.Add(i);
                }
            }
        }

        public static void InterateAllDirectories(DirectoryInfo diRoot, ref List<DirectoryInfo> diMatched, string pattern)
        {
            if (!diRoot.Exists)
            {
                return;
            }

            foreach (var i in diRoot.GetDirectories())
            {
                InterateAllDirectories(i, ref diMatched, pattern);
            }

            var dis = diRoot.GetDirectories(pattern == null ? "*" : pattern);

            foreach (var i in dis)
            {
                if (!diMatched.Contains(i, new FileSystemInfoEqualityComparer()))
                {
                    diMatched.Add(i);
                }
            }
        }

        public static void InterateAllFiles(DirectoryInfo diRoot, ref List<FileInfo> fiMatched, string pattern, bool recursive = false)
        {
            if (!diRoot.Exists)
            {
                return;
            }

            if (recursive)
            {
                foreach (var i in diRoot.GetDirectories())
                {
                    InterateAllFiles(i, ref fiMatched, pattern);
                }
            }

            var fis = diRoot.GetFiles(pattern);

            foreach (var i in fis)
            {
                if (!fiMatched.Contains(i, new FileSystemInfoEqualityComparer()))
                {
                    fiMatched.Add(i);
                }
            }
        }
    }
}
