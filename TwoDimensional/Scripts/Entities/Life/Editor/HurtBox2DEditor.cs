using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
    [CustomEditor(typeof(HurtBox2D))]
    public class HurtBox2DEditor : Editor 
    {
        SerializedProperty weaknessSP;

        void OnEnable() 
        {
            weaknessSP = serializedObject.FindProperty(nameof(HurtBox2D.Weakness));
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();

            // Default UI
            DrawDefaultInspector();

            // Custom UI
            GUILayout.Label("Weaknesses");

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-", GUILayout.Width(50))) 
            {
                if (weaknessSP.arraySize != 0)
                    weaknessSP.arraySize -= 1;
            }
            if (GUILayout.Button("+", GUILayout.Width(50))) 
            {
                weaknessSP.arraySize += 1;
            }

            GUILayout.EndHorizontal();

            for (int i = 0; i < weaknessSP.arraySize; i++)
            {
                // Get index of string value
                int selected = (InteractionTypeConstant.All.Any(itc => itc == weaknessSP.GetArrayElementAtIndex(i).stringValue)) 
                    ? InteractionTypeConstant.All.IndexOf(weaknessSP.GetArrayElementAtIndex(i).stringValue)
                    : 0;


                // UI
                GUILayout.BeginHorizontal();

                GUILayout.Label($"{nameof(HurtBox2D.Weakness)} {i}");
                // Select from list
                selected = EditorGUILayout.Popup(selected, InteractionTypeConstant.All.ToArray()); 

                GUILayout.EndHorizontal();
                // End UI

                // Apply selection to sp
                weaknessSP.GetArrayElementAtIndex(i).stringValue = InteractionTypeConstant.All[selected];
            }
 
            // Apply changes to Serialized Properties to original values.
            serializedObject.ApplyModifiedProperties();
        }
    }
}