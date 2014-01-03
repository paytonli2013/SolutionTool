using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Orc.SolutionTool.Model
{
    [XmlRoot("dir"), DebuggerDisplay("{Name}")]
    public class Directory : IXmlSerializable
    {
        public string Name { get; set; }

        public IList<Directory> SubDirectories { get; set; }

        public IList<File> Files { get; set; }

#if DEBUG
        public static Directory Sample
        {
            get
            {
                var dir = new Directory
                {
                    Name = "Root",
                    SubDirectories = new List<Directory>
                    {                 
                        new Directory
                        {
                            Name = "src",
                            Files = new List<File>
                            {
                                new File { Name = "class1.cs", },
                                new File { Name = "class2.cs", },
                                new File { Name = "class3.cs", },
                            },
                        },
                        new Directory
                        {
                            Name = "output",
                            SubDirectories = new List<Directory>
                            {
                                new Directory
                                {
                                    Name = "debug",
                                    Files = new List<File>
                                    {
                                        new File { Name = "app.exe", },
                                        new File { Name = "app.pdb", },
                                        new File { Name = "app.xml", },
                                    },
                                },
                            },
                            Files = new List<File>
                            {
                
                            },
                        },
                    },
                    Files = new List<File>
                    {
                        new File { Name = "readme.md", },
                    },
                };

                return dir;
            }
        } 
#endif

        #region IXmlSerializable members.

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            ReadDirectory(reader, this, true);
        }

        private static void ReadDirectory(XmlReader reader, Directory dir, bool isRoot = false)
        {
            var name = reader["name"];

            if (isRoot)
            {
                dir.Name = name;
            }

            if (!reader.IsEmptyElement)
            {
                while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Name == "dir")
                    {
                        name = reader["name"];

                        var cd = new Directory { Name = name, };

                        if (dir.SubDirectories == null)
                        {
                            dir.SubDirectories = new List<Directory>();
                        }

                        dir.SubDirectories.Add(cd);

                        ReadDirectory(reader, cd);
                    }
                    
                    if (reader.Name == "file")
                    {
                        name = reader["name"];

                        var file = new File { Name = name, };

                        if (dir.Files == null)
                        {
                            dir.Files = new List<File>();
                        }

                        dir.Files.Add(file);
                    }
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteDirectory(writer, this, true);
        }

        private static void WriteDirectory(XmlWriter writer, Directory dir, bool isRoot = false)
        {
            if (isRoot)
            {
                writer.WriteAttributeString("name", dir.Name);
            }

            if (dir != null && dir.SubDirectories != null)
            {
                foreach (var i in dir.SubDirectories)
                {
                    writer.WriteStartElement("dir");
                    writer.WriteAttributeString("name", i.Name);

                    if (i.SubDirectories != null)
                    {
                        WriteDirectory(writer, i);
                    }

                    writer.WriteEndElement();
                }
            }

            if (dir != null && dir.Files != null)
            {
                foreach (var i in dir.Files)
                {
                    writer.WriteStartElement("file");
                    writer.WriteAttributeString("name", i.Name);
                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }

    [XmlRoot("file"), DebuggerDisplay("{Name}")]
    public class File
    {
        public string Name { get; set; }
    }
}
