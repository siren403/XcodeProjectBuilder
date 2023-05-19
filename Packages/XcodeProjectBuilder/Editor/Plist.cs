using System;
using System.IO;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public class Plist :
        IDisposable,
        IPlistBooleanElements,
        IPlistStringElements
    {
        public PlistElementDict Root => Document.root;
        public IPlistBooleanElements Booleans => this;
        public IPlistStringElements Strings => this;

        protected readonly PlistDocument Document;
        protected readonly string Path;

        public Plist(string path)
        {
            Document = new PlistDocument();
            Path = path;
        }

        public void ReadOrCreate()
        {
            if (!ReadFromFile())
            {
                Create();
            }
        }

        public bool ReadFromFile()
        {
            try
            {
                Document.ReadFromFile(Path);
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        public void Create()
        {
            Document.Create();
            Document.WriteToFile(Path);
        }

        bool IPlistBooleanElements.this[string key]
        {
            get => Document.root.values[key].AsBoolean();
            set => Document.root.SetBoolean(key, value);
        }

        string IPlistStringElements.this[string key]
        {
            get => Document.root.values[key].AsString();
            set => Document.root.SetString(key, value);
        }

        public void WriteToFile()
        {
            Document.WriteToFile(Path);
        }

        public void Dispose()
        {
            WriteToFile();
        }
    }

    public interface IPlistBooleanElements
    {
        public bool this[string key] { get; set; }
    }

    public interface IPlistStringElements
    {
        public string this[string key] { get; set; }
    }
}