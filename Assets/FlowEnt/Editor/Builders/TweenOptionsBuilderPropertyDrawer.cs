using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FriedSynapse.FlowEnt.Editor
{
    [CustomPropertyDrawer(typeof(TweenOptionsBuilder))]
    public class TweenOptionsBuilderPropertyDrawer : PropertyDrawer
    {
        private enum PropertiesEnum
        {
            name,
            autoStart,
            skipFrames,
            delay,
            timeScale,
            time,
            easing,
            loopCount,
            loopType
        }

        private List<PropertiesEnum> properties;
        private List<PropertiesEnum> Properties => properties ??= Enum.GetValues(typeof(PropertiesEnum)).Cast<PropertiesEnum>().ToList();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
                    => property.isExpanded ? (EditorGUIUtility.singleLineHeight + FlowEntConstants.DrawerSpacing) * (Properties.Count + 1) : EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(FlowEntDrawers.GetRect(position, 0), property.isExpanded, label);

            if (!property.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < Properties.Count; i++)
            {
                PropertiesEnum prop = Properties[i];
                switch (prop)
                {
                    case PropertiesEnum.loopCount:
                        DrawLoopCount(position, property, i);
                        break;
                    default:
                        EditorGUI.PropertyField(FlowEntDrawers.GetRect(position, i + 1), property.FindPropertyRelative(prop.ToString()));
                        break;
                }
            }
            EditorGUI.indentLevel--;
        }

        private void DrawLoopCount(Rect position, SerializedProperty property, int i)
        {
            Rect loopCountRect = FlowEntDrawers.GetRect(position, i + 1);
            loopCountRect.width /= 2f;

            SerializedProperty isLoopCountInfiniteProperty = property.FindPropertyRelative("isLoopCountInfinite");

            GUI.enabled = !isLoopCountInfiniteProperty.boolValue;
            EditorGUI.PropertyField(loopCountRect, property.FindPropertyRelative(nameof(PropertiesEnum.loopCount)));
            GUI.enabled = true;

            loopCountRect.x += loopCountRect.width;
            EditorGUI.PropertyField(loopCountRect, isLoopCountInfiniteProperty);
        }
    }
}
