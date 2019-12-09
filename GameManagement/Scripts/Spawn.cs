using UnityEngine;

namespace Zeef.GameManagement
{
    [ExecuteInEditMode]
	public class Spawn : MonoBehaviour
    {
        public int ID;

        void Update()
        {
            gameObject.name = $"Spawn {ID}";
        }
	}
}
