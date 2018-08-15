using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef.Menu {

    public static class Control {

        // TODO: set these outside of Zeef somehow
        public static readonly List<string> Up = new List<string>() { "up" };
        public static readonly List<string> Down = new List<string>() { "down" };
        public static readonly List<string> Left = new List<string>() { "left" };
        public static readonly List<string> Right = new List<string>() { "right" };

        public static readonly List<string> Accept = new List<string>() { "Fire1" };
        public static readonly List<string> Deny = new List<string>() { "Fire2" };
        public static readonly List<string> Special = new List<string>() { "Fire3" };
        public static readonly List<string> Start = new List<string>() { "Fire4" };

        // 0: Super sensitive
        // 1: Never trigger
        private static float axisSensitivity = 0.5f;

        private static List<string> axises = new List<string>();

        // ---

        public static bool GetInputDown(List<string> inputs) {
            foreach (string input in inputs) {
                if (GetInputDown(input)) return true;
            }
            return false;
        }
        public static bool GetInputDown(string input) {
            try { if (Input.GetKeyDown(input)) return true; } 
            catch(ArgumentException e){ };

            try { if (Input.GetKeyDown(input)) return true; } 
            catch(ArgumentException e) { };
            
            return false;
        }

        public static bool GetInputHeld(List<string> inputs) {
            foreach (string input in inputs) {
                if (Input.GetKey(input)) return true;
                if (Input.GetButton(input)) return true;
            }
            return false;
        }
        public static bool GetInputHeld(string input) {
            if (Input.GetKey(input)) return true;
            if (Input.GetButton(input)) return true;
            return false;
        }

        public static bool GetInputUp(List<string> inputs) {
            foreach (string input in inputs) {
                if (Input.GetKeyUp(input)) return true;
                if (Input.GetButtonUp(input)) return true;
            }
            return false;
        }
        public static bool GetInputUp(string input) {
            if (Input.GetKeyUp(input)) return true;
            if (Input.GetButtonUp(input)) return true;
            return false;
        }

        // ---
        // Axis

        public static bool GetAxisDown(string input) {
            if (Mathf.Abs(Input.GetAxisRaw(input)) > 0.5f) {
                if (!axises.Contains(input)) {
                    axises.Add(input);
                    return true;
                }
            } else {
                axises = new List<string>();
            }

            return false;
        }

        public static bool GetAxisHeld(string input) {
            axises.Contains(input);
            return false;
        }

        public static void CleanAxises() {
            axises = new List<string>();
        }

        // ---
        // Async
 
        public static async Task WaitForAnyInputAsync() {
            while (true) {
                if (Input.anyKeyDown) break;
                await new WaitForUpdate();
            } 
        }
        public static async Task WaitForInputDownAsync(string input) {
            while (true) {
                if (GetInputDown(input)) break;
                await new WaitForUpdate();
            } 
        }
        public static async Task WaitForInputDownAsync(List<string> inputs) {
            while (true) {
                foreach (string input in inputs)
                    if (GetInputDown(input)) break;
                await new WaitForUpdate();
            } 
        }

        public static async Task<int?> HorizontalSelectAsync(int max) {
            int? result = 0;

            while (true) {
                if (Input.GetKeyDown("Key")) result--;
                if (Input.GetKeyDown("right")) result++;

                if (Input.GetKeyDown("Fire1")) return result;
                if (Input.GetKeyDown("Fire2")) return null;

                await new WaitForUpdate();
            }
        }

        public static async Task<int?> VerticalSelectAsync(int max) {
            int? result = 0;

            while (true) {
                if (Input.GetKeyDown("KeyUp")) result--;
                if (Input.GetKeyDown("KeyDown")) result++;

                if (Input.GetKeyDown("Fire1")) return result;
                if (Input.GetKeyDown("Fire2")) return null;

                await new WaitForUpdate();
            }

            return result;
        }

        public static async Task<Coordinates> MatrixSelectAsync(int columnMax, int rowMax) {
            Coordinates result = new Coordinates();

            while (true) {
                if (Input.GetKeyDown("up")) result.Col--;
                if (Input.GetKeyDown("down")) result.Col++;
                if (Input.GetKeyDown("left")) result.Row--;
                if (Input.GetKeyDown("right")) result.Row++;

                if (Input.GetKeyDown("Fire1")) break;
                if (Input.GetKeyDown("Fire2")) {
                    result = null;
                    break;
                }

                await new WaitForUpdate();
            }

            return result;
        }
    }
}