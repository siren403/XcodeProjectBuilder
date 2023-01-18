using System;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public interface IPlist
    {
        IPlistBooleanElements Booleans { get; }
    }

    public interface IPlistBooleanElements
    {
        public bool this[string key] { get; set; }
    }

    internal class InfoPlist : IPlist, IDisposable, IPlistBooleanElements
    {
        private readonly PlistDocument _plist;
        private readonly string _path;

        public IPlistBooleanElements Booleans => this;

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
    }
}