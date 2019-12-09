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
        SerializedProperty weakness;

        void OnEnable() 
        {
            weakness = serializedObject.FindProperty(nameof(HurtBox2D.Weakness));
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();
            
            DrawDefaultInspector();
            
            int selected = (InteractionTypeConstant.All.Any(i => i == weakness.stringValue)) 
                ? InteractionTypeConstant.All.IndexOf(weakness.stringValue)
                : 0;

            GUILayout.BeginHorizontal();

            GUILayout.Label(nameof(HurtBox2D.Weakness));
            selected = EditorGUILayout.Popup(selected, InteractionTypeConstant.All.ToArray());

            GUILayout.EndHorizontal();

            weakness.stringValue = InteractionTypeConstant.All[selected];

            serializedObject.ApplyModifiedProperties();
        }
    }
}