using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Menu {

    public class Example : MonoBehaviour { 

        // Lists of lists are not serialized in the inspector, so the inner list is wrapped in a class
        [SerializeField] List<ListWrapper> matrixElements;
        [SerializeField] List<UIElement> verticalElements;
        [SerializeField] List<UIElement> horizontalElements;

        void Start() {
            StartCoroutine(RunMatrixCoroutine());
        }

        void Update() {
            Listen();
        }

        void Listen() {
            if (Input.GetKeyDown("1")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunMatrixCoroutine());
            }
            if (Input.GetKeyDown("2")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunVerticalCoroutine());
            }
            if (Input.GetKeyDown("3")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunHorizontalCoroutine());
            }
        }

        void UnHighlightAll() {
            MenuInput.UnHighlightAllInMatrix(ListWrapper.ToListOfLists(matrixElements));
            MenuInput.UnHighlightAllInVerticalList(verticalElements);
            MenuInput.UnHighlightAllInVerticalList(horizontalElements);
        }

        IEnumerator RunMatrixCoroutine() {

            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromMatrixCoroutine(ListWrapper.ToListOfLists(matrixElements))
            );

            yield return cd.Coroutine;

            if (cd.Result != null) {
                Coordinates result = (Coordinates)cd.Result;
                print($"Chose element at coordintates [{result.Row}, {result.Col}]");
            }

            StartCoroutine(RunMatrixCoroutine());
        }

        IEnumerator RunVerticalCoroutine() {

            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromVerticalListCoroutine(verticalElements, this)
            );

            yield return cd.Coroutine;

            if (cd.Result != null) {
                int result = (int)cd.Result;
                print($"Chose element at index [{result}]");
            }

            StartCoroutine(RunVerticalCoroutine());
        }

        IEnumerator RunHorizontalCoroutine() {

            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromHorizontalListCoroutine(horizontalElements, this)
            );

            yield return cd.Coroutine;

            if (cd.Result != null) {
                int result = (int)cd.Result;
                print($"Chose element at index [{result}]");
            }

            StartCoroutine(RunHorizontalCoroutine());
        }
    }

    [System.Serializable]
    public class ListWrapper {

        public List<UIElement> List;

        public static List<List<UIElement>> ToListOfLists(List<ListWrapper> listWrappers) {
            List<List<UIElement>> result = new List<List<UIElement>>();
            listWrappers.ForEach(row => {
                result.Add(row.List);
            });
            return result;
        }

        public UIElement this[int index] {
            get { return List[index]; }
            set { List[index] = value; }
        }
    }
}