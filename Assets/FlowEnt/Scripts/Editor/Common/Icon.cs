using UnityEditor;
using UnityEngine;

namespace FriedSynapse.FlowEnt.Editor
{
    public static class Icon
    {
        public static GUIContent Menu = EditorGUIUtility.IconContent("_Menu@2x", "Menu");
        public static GUIContent Select = EditorGUIUtility.IconContent("d_icon dropdown@2x", "Select");
        public static GUIContent Info = EditorGUIUtility.IconContent("console.infoicon@2x", "Info");
        public static GUIContent Warning = EditorGUIUtility.IconContent("console.warnicon@2x", "Warning");

        public static GUIStyle Style = new GUIStyle(EditorStyles.miniButton) { padding = new RectOffset(2, 2, 2, 2) };
    }
}