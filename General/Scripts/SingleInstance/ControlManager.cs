using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Zeef
{
    public class AxisInfo
    {
        public string Name { get; set; }

        public bool IsPressedNegative { get; set; }
        public bool IsHeldNegative { get; set; }
        public bool IsReleasedNegative { get; set; }

        public bool IsPressedPositive { get; set; }
        public bool IsHeldPositive { get; set; }
        public bool IsReleasedPositive { get; set; }

        public AxisInfo() { }
        public AxisInfo(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Provides strongly typed references to input from the player.
    /// </summary>
    public class ControlManager : SingleInstance<ControlManager>
    {
        // Directions
        [SerializeField] private List<string> up = new List<string>() { "up" };
        public static List<string> Up { get { return Instance.up; } }

        [SerializeField] private List<string> down = new List<string>() { "down" };
        public static List<string> Down { get { return Instance.down; } }

        [SerializeField] private List<string> left = new List<string>() { "left" };
        public static List<string> Left { get { return Instance.left; } }

        [SerializeField] private List<string> right = new List<string>() { "right" };
        public static List<string> Right { get { return Instance.right; } }

        // Axises
        [SerializeField] private List<string> horizontal = new List<string>() { "Horizontal" };
        public static List<string> Horizontal { get { return Instance.horizontal; } }

        [SerializeField] private List<string> vertical = new List<string>() { "Vertical" };
        public static List<string> Vertical { get { return Instance.vertical; } }

        [SerializeField] private List<string> horizontal2 = new List<string>() { "Horizontal2" };
        public static List<string> Horizontal2 { get { return Instance.horizontal2; } }

        [SerializeField] private List<string> vertical2 = new List<string>() { "Vertical2" };
        public static List<string> Vertical2 { get { return Instance.vertical2; } }

        // Face Buttons
        [SerializeField] private List<string> accept = new List<string>() { "z" };
        public static List<string> Accept { get { return Instance.accept; } }

        [SerializeField] private List<string> deny = new List<string>() { "x" };
        public static List<string> Deny { get { return Instance.deny; } }

        [SerializeField] private List<string> special = new List<string>() { "c" };
        public static List<string> Special { get { return Instance.special; } }

        [SerializeField] private List<string> special2 = new List<string>() { "v" };
        public static List<string> Special2 { get { return Instance.special2; } }

        // Middle
        [SerializeField] private List<string> start = new List<string>() { "enter" };
        public static List<string> Start { get { return Instance.start; } }

        [SerializeField] private List<string> select = new List<string>() { "enter" };
        public static List<string> Select { get { return Instance.select; } }

        // 0: Super sensitive
        // 1: Never trigger
        [Range(0, 1)]
        [SerializeField] private static float axisSensitivity = 0.5f;

        /// <summary>
        /// List of relevant axes to listen to.
        /// </summary>
        private static List<AxisInfo> axisInfos = new List<AxisInfo>();

        // ---

        protected override void Awake()
        {
            // Call base.
            base.Awake();

            // Don't destroy on load.
            DontDestroyOnLoad(gameObject);

            // Register axes.

            axisInfos = new List<AxisInfo>();

            foreach (var axis in horizontal)
                axisInfos.Add(new AxisInfo(axis));
            foreach (var axis in horizontal2)
                axisInfos.Add(new AxisInfo(axis));
            foreach (var axis in vertical)
                axisInfos.Add(new AxisInfo(axis));
            foreach (var axis in vertical2)
                axisInfos.Add(new AxisInfo(axis));
        }

        private void Update()
        {
            ListenForAxes();
        }

        private void ListenForAxes()
        {
            float threshold = 0.35f;

            foreach (var axis in axisInfos)
            {
                // Positive Press/ Hold
                if (Input.GetAxisRaw(axis.Name) > threshold)
                {
                    if (!axis.IsPressedPositive && !axis.IsHeldPositive)
                    {
                        axis.IsPressedPositive = true;
                    }
                    else if (axis.IsPressedPositive)
                    {
                        axis.IsPressedPositive = false;
                        axis.IsHeldPositive = true;
                    }
                }
                // Positive Release
                else
                {
                    if (axis.IsPressedPositive || axis.IsHeldPositive)
                        axis.IsReleasedPositive = true;
                    else if (axis.IsReleasedPositive)
                        axis.IsReleasedPositive = false;

                    axis.IsPressedPositive = false;
                    axis.IsHeldPositive = false;
                }

                // Negative Press/ Hold
                if (Input.GetAxisRaw(axis.Name) < -threshold)
                {
                    if (!axis.IsPressedNegative && !axis.IsHeldNegative)
                    {
                        axis.IsPressedNegative = true;
                    }
                    else if (axis.IsPressedNegative)
                    {
                        axis.IsPressedNegative = false;
                        axis.IsHeldNegative = true;
                    }
                }
                // Negative Release
                else
                {
                    if (axis.IsPressedNegative || axis.IsHeldNegative)
                        axis.IsReleasedNegative = true;
                    else if (axis.IsReleasedNegative)
                        axis.IsReleasedNegative = false;

                    axis.IsPressedNegative = false;
                    axis.IsHeldNegative = false;
                }
            }
        }

        // ---
        // Get Input

        // Down / Press
        public static bool GetInputPressed(IEnumerable<string> inputs)
        {
            foreach (string input in inputs)
            {
                if (GetInputPressed(input)) return true;
            }
            return false;
        }
        public static bool GetInputPressed(string input)
        {
            try { if (Input.GetKeyDown(input)) return true; } 
            catch(ArgumentException e) { };

            try { if (Input.GetButtonDown(input)) return true; } 
            catch(ArgumentException e) { };
            
            return false;
        }

        // Held
        public static bool GetInputHeld(IEnumerable<string> inputs)
        {
            foreach (string input in inputs) 
                if (GetInputHeld(input)) return true;
            
            return false;
        }
        public static bool GetInputHeld(string input)
        {
            try { if (Input.GetKey(input)) return true; } 
            catch(ArgumentException e) { };

            try { if (Input.GetButton(input)) return true; } 
            catch(ArgumentException e) { };

            return false;
        }

        // Up/ Release
        public static bool GetInputReleased(IEnumerable<string> inputs)
        {
            foreach (string input in inputs)
            {
                if (GetInputReleased(input))
                    return true;
            }

            return false;
        }
        public static bool GetInputReleased(string input)
        {
            try { if (Input.GetKeyUp(input)) return true; }
            catch(ArgumentException e) { };

            try { if (Input.GetButtonUp(input)) return true; }
            catch(ArgumentException e) { };

            return false;
        }

        // ---
        // Axis

        /// <summary>
        /// Whether or not the user has pressed an axis. "Pressed" meaning that the duration of input was relatively short.
        /// </summary>
        /// <param name="inputs">Axes to check.</param>
        /// <param name="positive">Check negative or positive direction of axis?</param>
        public static bool GetAxisPressed(IEnumerable<string> inputs, bool positive)
        {
            foreach (var input in inputs)
            {
                if (GetAxisPressed(input, positive))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Whether or not the user has pressed an axis. "Pressed" meaning that the duration of input was relatively short.
        /// </summary>
        /// <param name="input">Axis to check.</param>
        /// <param name="positive">Check negative or positive direction of axis?</param>
        public static bool GetAxisPressed(string input, bool positive)
        {
            AxisInfo axisInfo = axisInfos.FirstOrDefault(a => a.Name == input);

            if (axisInfo == null)
                return false;

            if (positive)
            {
                return axisInfo.IsPressedPositive;
            }
            else
            {
                return axisInfo.IsPressedNegative;
            }
        }
        
        /// <summary>
        /// Whether or not the user has been holding any given axis.
        /// </summary>
        public static bool GetAxisHeld(IEnumerable<string> inputs, bool positive)
        {
            foreach (var input in inputs)
            {
                if (GetAxisHeld(input, positive))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Whether or not the user has been holding any given axis.
        /// </summary>
        public static bool GetAxisHeld(string input, bool positive)
        {
            AxisInfo axisInfo = axisInfos.FirstOrDefault(a => a.Name == input);

            if (axisInfo == null)
                return false;

            if (positive)
            {
                return axisInfo.IsHeldPositive;
            }
            else
            {
                return axisInfo.IsHeldNegative;
            }
        }

        /// <summary>
        /// Whether or not the user has been holding any given axis.
        /// </summary>
        public static bool GetAxisReleased(IEnumerable<string> inputs, bool positive)
        {
            foreach (var input in inputs)
            {
                if (GetAxisReleased(input, positive))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Whether or not the user has been holding any given axis.
        /// </summary>
        public static bool GetAxisReleased(string input, bool positive)
        {
            AxisInfo axisInfo = axisInfos.FirstOrDefault(a => a.Name == input);

            if (axisInfo == null)
                return false;

            if (positive)
            {
                return axisInfo.IsReleasedPositive;
            }
            else
            {
                return axisInfo.IsReleasedNegative;
            }
        }

        /// <summary>
        /// Returns the sum of the provided axes. Returned value will be capped between -1 and 1.
        /// </summary>
        /// <param name="axes">The axis in the Input Manager to get input from.</param>
        public static float GetSumOfAxes(IEnumerable<string> axes)
        {
            float dir = 0;

            foreach (var axis in axes)
            {
                try { dir += Input.GetAxisRaw(axis); }
                catch { }
            }

            if (dir < -1)
                dir = -1;

            if (dir > 1)
                dir = 1;

            return dir;
        }

        // ---
        // Async
 
        /// <summary>
        /// Wait for any input.
        /// </summary>
        public static async Task WaitForAnyInputAsync()
        {
            while (true)
            {
                if (Input.anyKeyDown) break;
                await new WaitForUpdate();
            } 
        }

        /// <summary>
        /// Wait for input from provided input. Method will return on pressed.
        /// </summary>
        public static async Task WaitForInputPressedAsync(string input)
        {
            await new WaitForUpdate();
            while (true)
            {
                if (GetInputPressed(input)) return;
                await new WaitForUpdate();
            } 
        }

        /// <summary>
        /// Wait for input from provided inputs. Method will return on pressed.
        /// </summary>
        public static async Task WaitForInputPressedAsync(IEnumerable<string> inputs)
        {
            await new WaitForUpdate();
            while (true)
            {
                foreach (string input in inputs) if (GetInputPressed(input)) return;
                await new WaitForUpdate();
            } 
        }

        // Menu Selection

        public static async Task<int?> HorizontalSelectAsync(int max)
        {
            int? result = 0;

            while (true)
            {
                if (GetInputPressed(Left)) result--;
                if (GetInputPressed(Right)) result++;

                if (GetInputPressed(Accept)) return result;
                if (GetInputPressed(Deny)) return null;

                await new WaitForUpdate();
            }
        }

        public static async Task<int?> VerticalSelectAsync(int max)
        {
            int? result = 0;

            while (true)
            {
                if (GetInputPressed(Up)) result--;
                if (GetInputPressed(Down)) result++;

                if (GetInputPressed(Accept)) return result;
                if (GetInputPressed(Deny)) return null;

                await new WaitForUpdate();
            }
        }

        public static async Task<Coordinates> MatrixSelectAsync(int columnMax, int rowMax, Action<Coordinates> changeAction)
        {
            Coordinates result = new Coordinates();

            while (true)
            {
                var old = new Coordinates(result.Col, result.Row);

                if (GetInputPressed(Up)) result.Row--;
                if (GetInputPressed(Down)) result.Row++;
                if (GetInputPressed(Left)) result.Col--;
                if (GetInputPressed(Right)) result.Col++;

                if (result.Col < 0) result.Col = 0;
                if (result.Col > columnMax - 1) result.Col = columnMax - 1;
                if (result.Row < 0) result.Row = 0;
                if (result.Row > rowMax - 1) result.Row = rowMax - 1;

                if (!old.SameAs(result)) changeAction(result);

                if (GetInputPressed(Accept)) return result;
                if (GetInputPressed(Deny)) return null;

                await new WaitForUpdate();
            }
        }
    }
}