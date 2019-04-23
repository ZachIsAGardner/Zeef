using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zeef;

namespace Zeef 
{
    // TODO:
    // * Arrow key movement
    // * Paste
    // * Up down scrolling (When entered too many statement should scroll up automatically)

    public class KeyboardInput : MonoBehaviour
    { 
        [SerializeField]
        private string red = "#f55";
        [SerializeField]
        private string white = "#fff";
        [SerializeField]
        private string dimRed = "#900";
        [SerializeField]
        private string dimWhite = "#aaa";

        [SerializeField]
        private string sign = "~$";

        [SerializeField] 
        private int lineCountMax = 3;

        private GameObject console;
        private TextMeshProUGUI textDisplay;

        private string oldText = "";
        private string currentLine = "";

        private bool active;

        private string ColoredText(string text, string color) => $"<color={color}>{text}</color>";

        void Start()
        {
            console = this.GetComponentInChildrenWithError<Image>().gameObject;
            textDisplay = this.GetComponentInChildrenWithError<TextMeshProUGUI>();
            textDisplay.text = ColoredText(sign, red);
        }

        private void UpdateDisplay() 
        {
            textDisplay.text = $"{oldText}{ColoredText(sign, red)} {ColoredText(currentLine, white)}";
            textDisplay.ForceMeshUpdate();
        }

        async void Update()
        {
            // Active/Deactive Debugger
            if (ControlManager.GetInputDown("escape")) 
            {
                console.SetActive(!console.activeSelf);
                return;
            }

            if (!console.activeSelf) return;

            // Process key presses
            foreach (char character in Input.inputString)
            {
                // Delete character
                if (character == '\b') 
                {
                    if (currentLine.Length > 0) 
                    {
                        currentLine = currentLine.Substring(0, currentLine.Length - 1);
                        UpdateDisplay();
                    }
                }
                // Process statement
                else if (character == '\n' || character == '\r')
                {
                    oldText = $"{oldText}{ColoredText(sign, dimRed)} {ColoredText(currentLine, dimWhite)}\n";
                    textDisplay.text += "\n" + ColoredText("...", red) + "\n";

                    // Wait so that the display is updated before compiling.
                    await new WaitForSeconds(0.001f);

                    try 
                    {
                        string code = RuntimeCode.Shell(currentLine);
                        Assembly assembly = RuntimeCode.Compile(code);
                        RuntimeCode.ExecuteMethodFromAssembly(assembly: assembly);
                    }
                    catch(Exception e)
                    {
                        oldText += ColoredText($"~! {e.Message}", dimRed);
                    }

                    currentLine = "";

                    UpdateDisplay();

                    // Ensure max line count
                    int attempt = 0;
                    while(textDisplay.textInfo.lineCount > lineCountMax)
                    {
                        // Remove lines until satisfied
                        oldText = string.Join("\n", oldText.Split('\n').Skip(1));
                        UpdateDisplay();

                        // Make sure Unity doesn't crash.
                        attempt++;
                        if (attempt >= 20) break;
                    }
                }
                // Type character
                else 
                {
                    currentLine += character;
                    UpdateDisplay();
                }
            }
        }
    }
}
