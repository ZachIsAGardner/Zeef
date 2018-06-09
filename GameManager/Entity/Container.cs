using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zeef.GameManager {
	// A container is like a folder
	// It only contains things, it doesnt do anything on its own
	public class Container : MonoBehaviour {
		[SerializeField] private ContainerID id;

		public ContainerID ID() {
			return id;
		}

		public static Container FindContainer(ContainerID id) {
			return FindObjectsOfType<Container>().FirstOrDefault(c => c.id == id);
		}

		public static List<Container> FindContainers(ContainerID id) {
			return FindObjectsOfType<Container>().Where(c => c.id == id).ToList();
		}

		public static GameObject FindContainerObject(ContainerID id) {
			Container container = FindObjectsOfType<Container>().FirstOrDefault(c => c.id == id);
			return (container != null) ? container.gameObject : null;
		}

		public static GameObject FindContainerObjectInParent(ContainerID id, Transform parent) {
			Container container = parent.GetComponentsInChildren<Container>().FirstOrDefault(c => c.id == id);
			return (container != null) ? container.gameObject : null;
		}

		public static GameObject PlaceCopyInContainer(GameObject go, ContainerID id) {
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
