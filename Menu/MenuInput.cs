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
        #region SimpleInput

        // like Console.ReadLine();
        public static IEnumerator WaitForInput() 
        {
            int? result = null;
            while (result == null)
            {
                result = ReadKeyPressAsNumber();
                yield return null;
            }
            yield return result;
        }

        private static int? ReadKeyPressAsNumber() 
        {
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

        #endregion

        #region Matrix

        // Returns Coordinates
        static public IEnumerator SelectFromMatrix(List<List<UIElement>> elements) 
        {
            Coordinates result = null;
            Coordinates focus = new Coordinates();

            HighlightInMatrix(elements, focus.row, focus.col);

            while (result == null) {
                Coordinates oldFocus = new Coordinates(focus.row, focus.col);
                focus = AdjustFocus(focus, new Coordinates(elements.Count, elements[focus.row].Count));

                if (focus.SameAs(oldFocus)) HighlightInMatrix(elements, focus.row, focus.col);
                
                if (Input.GetButtonDown("Fire1")) {
                    result = focus;
                    break;
                }

                if (Input.GetButtonDown("Fire2")) {
                    result = new Coordinates(-1, -1);
                    break;
                }

                yield return null;
            }

            // complete action
            UnHighlightAllInMatrix(elements);

            yield return result;
        }

        // Returns Coordinates
        static public IEnumerator SelectFromMatrix(Coordinates max, Action<Coordinates> changeAction) 
        {
            Coordinates result = null; 
            Coordinates focus = new Coordinates();

            while (result == null) {
                Coordinates oldFocus = new Coordinates(focus.row, focus.col);
                focus = AdjustFocus(focus, max);
                
                if (!focus.SameAs(oldFocus)) changeAction(focus);

                // Check for yes/ no input 
                if (Input.GetButtonDown("Fire1")) {
                    result = focus;
                    break;
                }

                if (Input.GetButtonDown("Fire2")) {
                    result = new Coordinates(-1, -1);
                    break;
                }

                yield return null;
            }

            // complete action

            yield return result;
        }
        
        // ---

        static public void UnHighlightAllInMatrix(List<List<UIElement>> matrix) 
        {
            matrix.ForEach(m => {
                m.ForEach(el => {
                    el.UnHighlight();
                });
            });
        }

        static private void HighlightInMatrix(List<List<UIElement>> matrix, int row, int col) 
        {
            UnHighlightAllInMatrix(matrix);
            matrix[row][col].Highlight();
        }

        static private Coordinates AdjustFocus(Coordinates focus, Coordinates max) {
            // Change value based on user input
            if (Input.GetKeyDown(KeyCode.UpArrow)) focus.row -= 1;
            if (Input.GetKeyDown(KeyCode.DownArrow)) focus.row += 1; 
            if (Input.GetKeyDown(KeyCode.LeftArrow)) focus.col -= 1;
            if (Input.GetKeyDown(KeyCode.RightArrow)) focus.col += 1;

            // Correct from max
            if (focus.row < 0) focus.row = 0;
            if (focus.row > max.row - 1) focus.row = max.row - 1;
            if (focus.col < 0) focus.col = 0;
            if (focus.col > max.col - 1) focus.col = max.col - 1;

            return focus;
        }

        #endregion
        
        #region VerticalList

        //return int
        static public IEnumerator SelectFromVerticalList(List<UIElement> elements, MonoBehaviour caller) 
        {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrix(ListToVerticalMatrix(elements))
            );
            yield return cd.coroutine;
            Coordinates result = (Coordinates)cd.result;
            yield return result.row;
        }

        // Returns int
        static public IEnumerator SelectFromVerticalList(int max, MonoBehaviour caller, Action<Coordinates> changeAction) 
        {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrix(new Coordinates(max, 0), changeAction)
            );
            yield return cd.coroutine;
            Coordinates result = (Coordinates)cd.result;
            yield return result.row;
        }

        static public void UnHighlightAllInVerticalList(List<UIElement> elements) 
        {
            UnHighlightAllInMatrix(ListToVerticalMatrix(elements));
        }

        static private List<List<UIElement>> ListToVerticalMatrix(List<UIElement> elements) 
        {
            List<List<UIElement>> result = new List<List<UIElement>>();
            elements.ForEach(e => {
                result.Add(new List<UIElement>() { e });
            });
            return result;
        }
        
        #endregion

        #region HorizontalList

        // returns int
        static public IEnumerator SelectFromHorizontalList(List<UIElement> elements, MonoBehaviour caller) 
        {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrix(ListToHorizontalMatrix(elements))
            );

            yield return cd.coroutine;

            Coordinates result = (Coordinates)cd.result;
            yield return result.col;
        }

        // returns int
        static public IEnumerator SelectFromHorizontallList(int max, MonoBehaviour caller, Action<Coordinates> changeAction) 
        {
            CoroutineWithData cd = new CoroutineWithData(
                caller,
                SelectFromMatrix(new Coordinates(0, max), changeAction)
            );

            yield return cd.coroutine;

            Coordinates result = (Coordinates)cd.result;
            yield return result.col;
        }

        static public void UnHighlightAllInHorizontalList(List<UIElement> elements) 
        {
            UnHighlightAllInMatrix(ListToHorizontalMatrix(elements));
        }

        static private List<List<UIElement>> ListToHorizontalMatrix(List<UIElement> elements) 
        {
            return new List<List<UIElement>>(){ elements };
        }

        #endregion
    }
}