using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace XcodeProjectBuilder.XcodeSettings
{
    [Serializable]
    public class BitcodeProperty : BooleanBuildSettingsProperty
    {
        public BitcodeProperty() : base((settings, value) => settings.EnableBitcode = value)
        {
        }
    }

    [Serializable]
    public class OtherLinkerFlagsProperty : StringsBuildSettingsProperty
    {
        public OtherLinkerFlagsProperty() : base("Other Linker Flags",
            (settings, list) =>
            {
                foreach (var value in list)
                {
                    settings.AddOtherLinkerFlags(value);
                }
            })
        {
        }
    }


    [Serializable]
    public abstract class BuildSettingsProperty
    {
        public abstract void Apply(BuildSettings settings);
        public abstract VisualElement CreateField(ScriptableObject target);
    }

    [Serializable]
    public class BooleanBuildSettingsProperty : BuildSettingsProperty
    {
        private readonly Action<BuildSettings, bool> _setter;
        [SerializeField] private bool value;

        public BooleanBuildSettingsProperty(Action<BuildSettings, bool> setter)
        {
            _setter = setter;
        }

        public override void Apply(BuildSettings settings)
        {
            _setter(settings, value);
        }

        public override VisualElement CreateField(ScriptableObject target)
        {
            var field = new Toggle("Bitcode")
            {
                value = value
            };
            field.RegisterValueChangedCallback(e =>
            {
                value = e.newValue;
                EditorUtility.SetDirty(target);
            });
            return field;
        }
    }

    [Serializable]
    public class StringsBuildSettingsProperty : BuildSettingsProperty
    {
        private readonly string _displayName;
        private readonly Action<BuildSettings, List<string>> _setter;
        [SerializeField] private List<string> values = new List<string>();

        public StringsBuildSettingsProperty(string displayName, Action<BuildSettings, List<string>> setter)
        {
            _displayName = displayName;
            _setter = setter;
        }

        public override void Apply(BuildSettings settings)
        {
            _setter(settings, values);
        }

        public override VisualElement CreateField(ScriptableObject target)
        {
            var fold = new Foldout()
            {
                text = _displayName
            };
            var list = new ListView(values)
            {
                makeItem = () =>
                {
                    var field = new TextField() { isDelayed = true };
                    return field;
                },
                bindItem = (element, i) =>
                {
                    if (element is TextField field)
                    {
                        field.value = values[i];
                        field.RegisterValueChangedCallback(e =>
                        {
                            values[i] = e.newValue;
                            EditorUtility.SetDirty(target);
                        });
                    }
                },
                fixedItemHeight = 22
            };
            fold.Add(new Button(() =>
            {
                values.Add(string.Empty);
                EditorUtility.SetDirty(target);
            }) { text = "ADD" });
            fold.Add(list);
            return fold;
        }
    }
}