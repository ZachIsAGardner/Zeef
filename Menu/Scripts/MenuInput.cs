using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ---
using Zeef.GameManagement;

namespace Zeef.Menu {
    // Understanding Matrix and Coordinates...

    //       | col 0 | col 1 | col 2 |
    // row 0 | item 0| item 1| item 2|
    // row 1 | item 0| item 1|

    public class SelectionResult {
        public Coordinates Focus { get; set; }
        public SelectionStatesEnum SelectionState { get; set; }

        public SelectionResult(Coordinates focus, SelectionStatesEnum selectionState) {
            Focus = focus;
            SelectionState = selectionState;
        }
    }

    public enum SelectionStatesEnum {
        StillDeciding,
        Confirmed,
        Cancelled
    }

    public static class MenuInput {

        #region SimpleInput

        // like Console.ReadLine();
        public static IEnumerator WaitForInputCoroutine() {
            int? result = null;
            while (result == null) {
                result = ReadKeyPressAsNumber();
                yield return null;
            }
            yield return result;
        }

        // Returns SelectionResult
        public static IEnumerator WaitForSelectionCoroutine(Coordinates currentFocus, Coordinates max) {
            SelectionResult result = null;

            while (true) {
                // Up
                if (Input.GetKeyDown(KeyCode.UpArrow) && currentFocus.Row - 1 > -1) {
                    result = new SelectionResult(
                        new Coordinates(currentFocus.Col, currentFocus.Row - 1), 
                        SelectionStatesEnum.StillDeciding
                    );
                    break;
                }

                // Down
                if (Input.GetKeyDown(KeyCode.DownArrow) && currentFocus.Row + 1 < max.Row) {
                    result = new SelectionResult(
                        new Coordinates(currentFocus.Col, currentFocus.Row + 1), 
                        SelectionStatesEnum.StillDeciding
                    );
                    break;
                }

                // Left
                if (Input.GetKeyDown(KeyCode.LeftArrow) && currentFocus.Col - 1 > -1) {
                    result = new SelectionResult(
                        new Coordinates(currentFocus.Col - 1, currentFocus.Row), 
                        SelectionStatesEnum.StillDeciding
                    );
                    break;
                }

                // Right
                if (Input.GetKeyDown(KeyCode.RightArrow) && currentFocus.Col + 1 < max.Col) {
                    result = new SelectionResult(
                        new Coordinates(currentFocus.Col + 1, currentFocus.Row), 
                        SelectionStatesEnum.StillDeciding
                    );
                    break;
                }

                // Confirm
                if (Input.GetButtonDown("Fire1")) {
                    result = new SelectionResult(
                        currentFocus,
                        SelectionStatesEnum.Confirmed
                    );
                    break;
                }

                // Cancel
                if (Input.GetButtonDown("Fire2")) {
                    result = new SelectionResult(
                        null,
                        SelectionStatesEnum.Cancelled
                    );
                    break;
                }

                yield return null;
            }

            yield return result;
        }

        private static int? ReadKeyPressAsNumber() {
            int result = -1;
            if (Input.anyKeyDown) 
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                    if (Input.GetKey(key)) 
                        Int32.TryParse(key.ToString().Replace("Alpha", ""), out result); 
                
            if (result == -1) return null;
            else return result; 
        }

        #endregion

        #region Matrix

        // Returns SelectionResult
        static public IEnumerator SelectFromListItemContainer(ListItemContainerUI container, MonoBehaviour host) {
            // SelectionResult result = null;
            // Coordinates focus = new Coordinates();

            // container.HighlightListItem(focus);

            // while (true) {
            //     Coordinates oldFocus = new Coordinates(focus.Col, focus.Row);

            //     CoroutineWithData cd = new CoroutineWithData(host, WaitForSelectionCoroutine(
            //         focus, 
            //         new Coordinates (container.Pages.Count, container.Pages[focus.Col].ListItems.Count))
            //     );
            //     yield return cd.Coroutine;
            //     SelectionResult selectionResult = (SelectionResult)cd.Result;

            //     if (selectionResult.SelectionState == SelectionStatesEnum.StillDeciding) {
            //         focus  = selectionResult.Focus;
            //         container.HighlightListItem(focus);
            //     } else {
            //         result = selectionResult;
            //         break;
            //     }
            // }

            // yield return result;
            yield return null;
        }

        // Returns Coordinates
        static public IEnumerator SelectFromMatrixCoroutine(List<List<UIElement>> elements) {
            Coordinates result = null;
            Coordinates focus = new Coordinates();

            HighlightInMatrix(elements, focus.Row, focus.Col);

            while (result == null) {
                Coordinates oldFocus = new Coordinates(focus.Col, focus.Row);
                focus = AdjustFocus(focus, new Coordinates(elements[focus.Row].Count, elements.Count));

                if (!focus.SameAs(oldFocus)) HighlightInMatrix(elements, focus.Row, focus.Col);
                
                if (Input.GetButtonDown("Fire1")) {
                    result = focus;
                    break;
                }

                if (Input.GetButtonDown("Fire2")) {
                    result = null;
                    break;
                }

                yield return null;
            }

            // complete action
            UnHighlightAllInMatrix(elements);

            yield return result;
        }

        // Returns Coordinates
        static public IEnumerator SelectFromMatrixCoroutine(Coordinates max, Action<Coordinates> changeAction) {

            Coordinates result = null; 
            Coordinates focus = new Coordinates();

            while (result == null) {
                Coordinates oldFocus = new Coordinates(focus.Row, focus.Col);
                focus = AdjustFocus(focus, max);
                
                if (!focus.SameAs(oldFocus)) changeAction(focus);

                // Check for yes/ no input 
                if (Input.GetButtonDown("Fire1")) {
                    result = focus;
                    break;
                }

                if (Input.GetButtonDown("Fire2")) {
                    result = null;
                    break;
                }

                yield return null;
            }

            // complete action

            yield return result;
        }
        
        // ---

        static public void UnHighlightAllInMatrix(List<List<UIElement>> matrix) {
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
        
        // When player is deciding selection
        static private Coordinates AdjustFocus(Coordinates focus, Coordinates max) {

            // Change value based on user input
            if (Input.GetKeyDown(KeyCode.UpArrow)) focus.Row -= 1;
            if (Input.GetKeyDown(KeyCode.DownArrow)) focus.Row += 1; 
            if (Input.GetKeyDown(KeyCode.LeftArrow)) focus.Col -= 1;
            if (Input.GetKeyDown(KeyCode.RightArrow)) focus.Col += 1;

            // Correct from max
            if (focus.Row < 0) focus.Row = 0;
            if (focus.Row > max.Row - 1) focus.Row = max.Row - 1;
            if (focus.Col < 0) focus.Col = 0;
            if (focus.Col > max.Col - 1) focus.Col = max.Col - 1;

            return focus;
        }

        #endregion
        
        #region VerticalList

        //return int?
        static public IEnumerator SelectFromVerticalListCoroutine(List<UIElement> elements, MonoBehaviour caller) {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrixCoroutine(ListToVerticalMatrix(elements))
            );
            yield return cd.Coroutine;

            Coordinates result = (Coordinates)cd.Result;
            if (result != null) {
                yield return result.Row;
            } else {
                yield return null;
            }
        }

        // Returns int?
        static public IEnumerator SelectFromVerticalListCoroutine(int max, MonoBehaviour caller, Action<Coordinates> changeAction) {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrixCoroutine(new Coordinates(max, 0), changeAction)
            );
            yield return cd.Coroutine;

            Coordinates result = (Coordinates)cd.Result;
            if (result != null) {
                yield return result.Row;
            } else {
                yield return null;
            }
        }

        static public void UnHighlightAllInVerticalList(List<UIElement> elements) {
            UnHighlightAllInMatrix(ListToVerticalMatrix(elements));
        }

        static private List<List<UIElement>> ListToVerticalMatrix(List<UIElement> elements) {
            List<List<UIElement>> result = new List<List<UIElement>>();
            elements.ForEach(e => {
                result.Add(new List<UIElement>() { e });
            });
            return result;
        }
        
        #endregion

        #region HorizontalList

        // returns int?
        static public IEnumerator SelectFromHorizontalListCoroutine(List<UIElement> elements, MonoBehaviour caller) {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrixCoroutine(ListToHorizontalMatrix(elements))
            );
            yield return cd.Coroutine;

            Coordinates result = (Coordinates)cd.Result;
            if (result != null) {
                yield return result.Col;
            } else {
                yield return null;
            }
        }

        // returns int?
        static public IEnumerator SelectFromHorizontalListCoroutine(int max, MonoBehaviour caller, Action<Coordinates> changeAction) {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrixCoroutine(new Coordinates(0, max), changeAction)
            );
            yield return cd.Coroutine;

            Coordinates result = (Coordinates)cd.Result;
            if (result != null) {
                yield return result.Col;
            } else {
                yield return null;
            }
        }

        static public void UnHighlightAllInHorizontalList(List<UIElement> elements) {
            UnHighlightAllInMatrix(ListToHorizontalMatrix(elements));
        }

        static private List<List<UIElement>> ListToHorizontalMatrix(List<UIElement> elements) {
            return new List<List<UIElement>>(){ elements };
        }

        #endregion
    }
}