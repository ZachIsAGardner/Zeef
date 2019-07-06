using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Zeef.TwoDimensional 
{
    [CustomEditor(typeof(HitBox2D))]
    public class HitBox2DEditor : Editor 
    {
        SerializedProperty interactionType;

        void OnEnable() 
        {
            interactionType = serializedObject.FindProperty(nameof(HitBox2D.InteractionType));
        }

        public override void OnInspectorGUI() 
        {
            serializedObject.Update();
            
            DrawDefaultInspector();
            
            int selected = (InteractionTypeConstants.All.Any(i => i == interactionType.stringValue)) 
                ? InteractionTypeConstants.All.IndexOf(interactionType.stringValue)
                : 0;

            GUILayout.BeginHorizontal();

            GUILayout.Label(nameof(HitBox2D.InteractionType));
            selected = EditorGUILayout.Popup(selected, InteractionTypeConstants.All.ToArray());

            GUILayout.EndHorizontal();

            interactionType.stringValue = InteractionTypeConstants.All[selected];

            serializedObject.ApplyModifiedProperties();
        }
    }
}