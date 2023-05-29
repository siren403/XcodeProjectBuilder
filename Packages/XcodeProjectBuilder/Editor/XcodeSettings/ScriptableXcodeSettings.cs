using System;
using System.Collections.Generic;
using UnityEngine;

namespace XcodeProjectBuilder.XcodeSettings
{
    [CreateAssetMenu(menuName = "XcodeSettings", fileName = "XcodeSettings")]
    public class ScriptableXcodeSettings : ScriptableObject
    {
        [SerializeReference] private List<BuildSettingsProperty> buildSettings = new List<BuildSettingsProperty>()
        {
            new BitcodeProperty(),
            new OtherLinkerFlagsProperty()
        };

        public List<BuildSettingsProperty> BuildSettings => buildSettings;
    }
}