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
                var xcode = CreateBuilder();

                var info = xcode.ReadInfoPlist();
                info.GoogleSignInConfiguration("Assets/Config/GoogleService-Info.plist");
                info.WriteToFile();
                xcode.WriteToFile();
            };

            rootVisualElement.Q<Button>("add-push").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WritePushNotifications();
            };

            rootVisualElement.Q<Button>("add-fcm-push").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WriteFirebasePushNotifications();
            };

            rootVisualElement.Q<Button>("add-background-modes").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WriteBackgroundModes(BackgroundModesOptions.BackgroundFetch);
            };
            rootVisualElement.Q<Button>("add-game-center").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WriteGameCenter();
            };
            rootVisualElement.Q<Button>("add-in-app").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WriteInAppPurchase();
            };
            rootVisualElement.Q<Button>("all").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WritePushNotifications();
                capability.WriteGameCenter();
                capability.WriteInAppPurchase();
            };
            rootVisualElement.Q<Button>("test-case-1").clicked += () =>
            {
                var capability = XcodeProject.ReadCapability("build");
                capability.WriteSignIn();
                capability.WriteFirebasePushNotifications();
                capability.WriteInAppPurchase();
            };
            
        }

        private static XcodeProject CreateBuilder()
        {
            var xcode = XcodeProject.ReadProject("build");
            return xcode;
        }
    }
}