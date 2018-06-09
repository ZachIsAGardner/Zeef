using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---
using Zeef.GameManager;

namespace Zeef.Menu 
{
    public static class MenuInput 
    {
        // Console.ReadLine();
        public static IEnumerator WaitForInput() {
            int? result = null;
            while (result == null)
            {
                result = ReadKeyPressAsNumber();
                yield return null;
            }
            yield return result;
        }

        private static int? ReadKeyPressAsNumber() {
            int result = -1;
            if (Input.anyKeyDown) {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key)) {
                        Int32.TryParse(key.ToString().Replace("Alpha", ""), out result);
                    }
                }
            }

            if (result == -1) {
                return null;
            } else {
                return result;
            }
        }

        #region Matrix

        static private void UnHighlightAllInMatrix(List<List<UIElement>> matrix) {
            matrix.ForEach(m => {
                m.ForEach(el => {
                    el.UnHighlight();
                });
            });
        }

        static private void HighlightInMatrix(List<List<UIElement>> matrix, int row, int col) {
            UnHighlightAllInMatrix(matrix);
            matrix[row][col].Highlight();
        }

        // Returns Coordinates
        static public IEnumerator SelectFromMatrix(List<List<UIElement>> matrix) {
            Coordinates result = null;

            int col = 0;
            int row = 0;

            while (result == null) {
                if (row < 0) {
                    row = 0;
                } 
                if (row > matrix.Count - 1) {
                    row = matrix.Count - 1;
                }

                if (col < 0) {
                    col = 0;
                }
                if (col > matrix[row].Count - 1) {
                    col = matrix[row].Count - 1;
                }

                HighlightInMatrix(matrix, row, col);
                
                if (Input.GetButtonDown("Fire1")) {
                    result = new Coordinates(col, row);
                    break;
                }

                if (Input.GetButtonDown("Fire2")) {
                    result = new Coordinates(-1, -1);
                    break;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    col -= 1;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    col += 1;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    row -= 1;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    row += 1;
                }

                yield return null;
            }

            UnHighlightAllInMatrix(matrix);

            yield return result;
        }

        #endregion
        
        #region VerticalList

        // Make this take in a list of ui elements instead or make overload
        // Returns Int
        static public IEnumerator VerticalList(int max, Action<int> changeAction) {
            int? result = null;
            int idx = 0;
            
            while (result == null) {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    idx -= 1;
                    if (idx < 0) {
                        idx = 0;
                    }
                    changeAction(idx);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    idx += 1;
                    if (idx > max) {
                        idx = max;
                    }
                    changeAction(idx);
                }

                if (Input.GetButtonDown("Fire1")) {
                    result = idx;
                }

                if (Input.GetButtonDown("Fire2")) {
                    // result = -1;
                }

                yield return null;
            }
            yield return result;
        }
        
        #endregion
    }
}