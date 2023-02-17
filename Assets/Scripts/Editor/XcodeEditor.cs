using System;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.UIElements;
using XcodeProjectBuilder;

namespace Editor
{
    public class XcodeEditor : EditorWindow
    {
        [MenuItem("Xcode/Xcode Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<XcodeEditor>();
            window.titleContent = new GUIContent("Xcode Editor");
            window.Show();
        }

        private void CreateGUI()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/XcodeEditor.uxml");
            rootVisualElement.Add(uxml.Instantiate());

            rootVisualElement.Q<Button>("plist-add-list-button").clicked += () =>
            {
                using var xcode = CreateBuilder();
                xcode.Info.GoogleSignInConfiguration("Assets/Config/GoogleService-Info.plist");
            };
        }

        private static IXcodeProject CreateBuilder()
        {
            var xcode = XcodeProject.FromPostProcess(BuildTarget.iOS, "build");
            return xcode;
        }
    }
}