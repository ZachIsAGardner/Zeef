using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManagement {
	// A container is like a folder
	// It only contains things, it doesnt do anything on its own
	public class Container : MonoBehaviour {
		[SerializeField] private ContainersEnum id;

		public ContainersEnum ID() {
			return id;
		}

		public static Container FindContainer(ContainersEnum id) {
			return FindObjectsOfType<Container>().FirstOrDefault(c => c.id == id);
		}

		public static List<Container> FindContainers(ContainersEnum id) {
			return FindObjectsOfType<Container>().Where(c => c.id == id).ToList();
		}

		public static GameObject FindContainerObject(ContainersEnum id) {
			Container container = FindObjectsOfType<Container>().FirstOrDefault(c => c.id == id);
			return (container != null) ? container.gameObject : null;
		}

		public static GameObject FindContainerObjectInParent(ContainersEnum id, Transform parent) {
			Container container = parent.GetComponentsInChildren<Container>().FirstOrDefault(c => c.id == id);
			return (container != null) ? container.gameObject : null;
		}

		public static GameObject PlaceCopyInContainer(GameObject go, ContainersEnum id) {
			Container container = FindContainer(id);

			if (container == null) {
				container = new GameObject().AddComponent<Container>();
				container.id = id;
				container.name = id.ToString();
			}
			GameObject copy = Instantiate(go, container.transform);

			return copy;
		}		
	}
}
