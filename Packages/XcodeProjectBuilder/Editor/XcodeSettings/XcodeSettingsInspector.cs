using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XcodeProjectBuilder.XcodeSettings
{
    [CustomEditor(typeof(ScriptableXcodeSettings))]
    public class XcodeSettingsInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            if (!(target is ScriptableXcodeSettings settings)) return root;

            var uxml = Resources.Load<VisualTreeAsset>("XcodeSettingsInspector");
            uxml.CloneTree(root);

            var buildSettings = root.Q<Foldout>("build-settings");
            foreach (var property in settings.BuildSettings) 
            {
                var field = property.CreateField(settings);
                buildSettings.Add(field);
            }
            // var buildPhases = root.Q<Foldout>("build-phases");
            // foreach (var property in settings.BuildPhases)
            // {
            //     var field = property.CreateField(settings);
            //     buildPhases.Add(field);
            // }
            return root;
        }
    }
}