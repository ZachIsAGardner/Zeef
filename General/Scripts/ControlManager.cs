using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef {

    public class ControlManager : MonoBehaviour {

        private static ControlManager controlManager;

        [SerializeField] private List<string> up = new List<string>() { "up" };
        public static List<string> Up { get { return controlManager.up; } }

        [SerializeField] private List<string> down = new List<string>() { "down" };
        public static List<string> Down { get { return controlManager.down; } }

        [SerializeField] private List<string> left = new List<string>() { "left" };
        public static List<string> Left { get { return controlManager.left; } }

        [SerializeField] private List<string> right = new List<string>() { "right" };
        public static List<string> Right { get { return controlManager.right; } }

        [SerializeField] private List<string> accept = new List<string>() { "z" };
        public static List<string> Accept { get { return controlManager.accept; } }

        [SerializeField] private List<string> deny = new List<string>() { "x" };
        public static List<string> Deny { get { return controlManager.deny; } }

        [SerializeField] private List<string> special = new List<string>() { "c" };
        public static List<string> Special { get { return controlManager.special; } }

        [SerializeField] private List<string> start = new List<string>() { "enter" };
        public static List<string> Start { get { return controlManager.start; } }

        // 0: Super sensitive
        // 1: Never trigger
        [Range(0, 1)]
        [SerializeField] private static float axisSensitivity = 0.5f;

        // Axises that are input by the user get added to 
        // this list to further interpret what type of input is happening
        private static List<string> usedAxises = new List<string>();

        // ---

        void Awake() {
            this.FindObjectOfTypeWithError<MonoBehaviour>();
            if (controlManager != null) throw new Exception("Only one ControlManager can be loaded at once.");
            controlManager = this;
            DontDestroyOnLoad(gameObject);
        }

        public static ControlManager Main() {
            return controlManager;
        }

        // ---
        // Get Input

        public static bool GetInputDown(string input) {
            try { if (Input.GetKeyDown(input)) return true; } 
            catch(ArgumentException e) { };

            try { if (Input.GetButtonDown(input)) return true; } 
            catch(ArgumentException e) { };
            
            return false;
        }

        public static bool GetInputDown(List<string> inputs) {
            foreach (string input in inputs) {
                if (GetInputDown(input)) return true;
            }
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
                if (!usedAxises.Contains(input)) {
                    usedAxises.Add(input);
                    return true;
                }
            } else {
                usedAxises = new List<string>();
            }

            return false;
        }

        public static bool GetAxisHeld(string input) {
            usedAxises.Contains(input);
            return false;
        }

        public static void CleanAxises() {
            usedAxises = new List<string>();
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
            await new WaitForUpdate();
            while (true) {
                if (GetInputDown(input)) return;
                await new WaitForUpdate();
            } 
        }
        public static async Task WaitForInputDownAsync(List<string> inputs) {
            await new WaitForUpdate();
            while (true) {
                foreach (string input in inputs) if (GetInputDown(input)) return;
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