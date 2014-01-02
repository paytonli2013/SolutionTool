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
