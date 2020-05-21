using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.GameManagement;

namespace Zeef.Menu 
{
    public class TargetTransformUI : MonoBehaviour 
    {
        [SerializeField] private Transform target;

        public static TargetTransformUI Initialize(TargetTransformUI prefab, Transform target) 
        {
			TargetTransformUI instance = SpawnManager.SpawnCanvasElement(prefab.gameObject).GetComponent<TargetTransformUI>();

            instance.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(target.position);

            instance.target = target;

			return instance;
		}

        void LateUpdate() 
        {
            if (target == null)
                return;

            GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}
