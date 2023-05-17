using System;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public interface IPlist
    {
        PlistElementDict Root { get; }
        IPlistBooleanElements Booleans { get; }
        IPlistStringElements Strings { get; }
    }

    public interface IPlistBooleanElements
    {
        public bool this[string key] { get; set; }
    }

    public interface IPlistStringElements
    {
        public string this[string key] { get; set; }
    }

    internal class InfoPlist : IPlist, IDisposable,
        IPlistBooleanElements,
        IPlistStringElements
    {
        private readonly PlistDocument _plist;
        private readonly string _path;

        public PlistElementDict Root => _plist.root;

        public IPlistBooleanElements Booleans => this;
        public IPlistStringElements Strings => this;

        public InfoPlist(PBXProjectWrapper wrapper)
        {
            _plist = new PlistDocument();

            _path = wrapper.BuildPath + "/Info.plist";
            _plist.ReadFromFile(_path);
        }

        public void Dispose()
        {
            _plist.WriteToFile(_path);
        }

        bool IPlistBooleanElements.this[string key]
        {
            get => _plist.root.values[key].AsBoolean();
            set => _plist.root.SetBoolean(key, value);
        }

        string IPlistStringElements.this[string key]
        {
            get => _plist.root.values[key].AsString();
            set => _plist.root.SetString(key, value);
        }
    }
}