using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Zeef {

    public static class Utility {

        // max exclusive
        public static int RandomInt(int max) {
            System.Random r = new System.Random();
            return r.Next(0, max);
        }
        public static int RandomInt(int min, int max) {
            System.Random r = new System.Random();
            return r.Next(min, max);
        }

        // 1 in {odds} change of returning true
        public static bool RandomChance(int odds) {
            return RandomInt(odds) == 0;
        }

        public static string SplitCamelCase(string str) {
            var r = new Regex(
                @"(?<=[A-Z])(?=[A-Z][a-z]) |
                (?<=[^A-Z])(?=[A-Z]) |
                (?<=[A-Za-z])(?=[^A-Za-z])", 
                RegexOptions.IgnorePatternWhitespace
            );
            return r.Replace(str, " ");
        }

        // ex)
        // Zeef.GameManager
        // GameManager
        public static string TrimType(object type) {
            return type.ToString().Split('.')[type.ToString().Split('.').Length - 1];
        }

        // ---

        public static T FindObjectOfTypeWithError<T> () where T : UnityEngine.Object {
            T result = UnityEngine.Object.FindObjectOfType<T>();
            if (result == null) throw new Exception($"Could not find any objects with the '{typeof(T).ToString()}' component attached.");
            return result;
        }

        // ---

        public static Color Color255(float r, float g, float b) {
            return new Color(r / 255, g / 255, b / 255);
        }
        public static Color Color255(float r, float g, float b, float a) {
            return new Color(r / 255, g / 255, b / 255, a / 255);
        }

        // ---

        // KirillKuzyk
        public static void ClearConsole() {      
            var assembly = Assembly.GetAssembly(typeof(SceneView));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}