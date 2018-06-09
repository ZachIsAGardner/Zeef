using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Menu 
{
    public class Example : MonoBehaviour
    { 
        [SerializeField] List<ListWrapper> matrixElements;
        [SerializeField] List<UIElement> verticalElements;
        [SerializeField] List<UIElement> horizontalElements;

        void Start() 
        {
            StartCoroutine(RunMatrix());
        }

        void Update() 
        {
            Listen();
        }

        void Listen() 
        {
            if (Input.GetKeyDown("1")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunMatrix());
            }
            if (Input.GetKeyDown("2")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunVertical());
            }
            if (Input.GetKeyDown("3")) {
                StopAllCoroutines();
                UnHighlightAll();
                StartCoroutine(RunHorizontal());
            }
        }

        void UnHighlightAll() 
        {
            MenuInput.UnHighlightAllInMatrix(ListWrapper.ToListOfLists(matrixElements));
            MenuInput.UnHighlightAllInVerticalList(verticalElements);
            MenuInput.UnHighlightAllInVerticalList(horizontalElements);
        }

        IEnumerator RunMatrix() 
        {
            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromMatrix(ListWrapper.ToListOfLists(matrixElements))
            );

            yield return cd.coroutine;

            if (cd.result.GetType() == typeof(Coordinates)) {
                Coordinates result = (Coordinates)cd.result;
                print($"Chose element at coordintates [{result.row}, {result.col}]");
            }

            StartCoroutine(RunMatrix());
        }

        IEnumerator RunVertical() 
        {
            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromVerticalList(verticalElements, this)
            );

            yield return cd.coroutine;

            if (cd.result.GetType() == typeof(int)) {
                int result = (int)cd.result;
                print($"Chose element at index [{result}]");
            }

            StartCoroutine(RunVertical());
        }

        IEnumerator RunHorizontal() 
        {
            CoroutineWithData cd = new CoroutineWithData (
                this,
                MenuInput.SelectFromHorizontalList(horizontalElements, this)
            );

            yield return cd.coroutine;

            if (cd.result.GetType() == typeof(int)) {
                int result = (int)cd.result;
                print($"Chose element at index [{result}]");
            }

            StartCoroutine(RunHorizontal());
        }
    }

    [System.Serializable]
    public class ListWrapper 
    {
        public List<UIElement> list;

        public static List<List<UIElement>> ToListOfLists(List<ListWrapper> listWrappers) 
        {
            List<List<UIElement>> result = new List<List<UIElement>>();
            listWrappers.ForEach(row => {
                result.Add(row.list);
            });
            return result;
        }

        public UIElement this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }
    }
}