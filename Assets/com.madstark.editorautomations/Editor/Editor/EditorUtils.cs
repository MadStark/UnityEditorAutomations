using UnityEditor;
using UnityEngine;

namespace MadStark.EditorAutomations
{
    public static class EditorUtils
    {
        public static Color AccentColor => EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(127, 127, 127, 255);
    }
}
