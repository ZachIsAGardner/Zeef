using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class Camera2D : MonoBehaviour {

		[SerializeField] Vector3 offset;
		[SerializeField] Transform target;
		[SerializeField] float acc = .5f;

		float normalZoom;
		bool zooming;
		float zoom;

		Bounds bounds;
		bool noBounds;

		Camera cam;

		float shakeTime;
		float shakeTick;
		Vector3 shakeStart;

		void Start () {
			cam = GetComponent<Camera>();

			normalZoom = cam.orthographicSize;

			GetTarget();
			GetBoundaries();
			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			GetBoundaries();
		}

		public void GetTarget() {
			GameObject player = Identifier.FindIdentifierObject(IdentifiersEnum.Player);
			target = (player) ? 
				player.transform : 
				null;
		}

		public void GetBoundaries() {
			GameObject boundsObject = Identifier.FindIdentifierObject(IdentifiersEnum.Bounds);
			if (boundsObject) {
				bounds = boundsObject.GetComponent<BoxCollider2D>().bounds;
				noBounds = false;
			} else {
				noBounds = true;
			}
		}
		
		void LateUpdate() {
			if (GameManager.IsPaused()) return;

			if (target) {
				Move();
			} else {
				GetTarget();
			}

			shakeTime -= 1 * Time.deltaTime;
			if (shakeTime > 0) {
				Shaking();
			} 
			if (zooming) {
				Zooming();
			}

			CheckBoundaries();
		}

		public void ChangeZoom(float newZoom) {
			zooming = true;
			zoom = newZoom;
		}

		public void Shake(float newShakeTime) {
			shakeTime = newShakeTime;
			shakeTick = 0;
			shakeStart = transform.position;
		}

		void Shaking() {
			shakeTick += 1 * Time.deltaTime;
			if (shakeTick >= 0.01f) {
				Vector2 offset;
				offset.x = (Random.Range(0,2) == 1) ? 10 : -10;
				offset.y = (Random.Range(0,2) == 1) ? 10 : -10;

				transform.position = new Vector3(
					shakeStart.x + offset.x * shakeTime,	
					shakeStart.y + offset.y * shakeTime,	
					transform.position.z
				);
				shakeTick = 0;
			}
		}

		void Zooming() {
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, 0.125f);
			if (Mathf.Abs(cam.orthographicSize - zoom) < 1) {
				cam.orthographicSize = zoom;
				zooming = false;
			}
		}

		public void Move() {
			Vector2 moveTo = Vector2.zero;
			moveTo.x = Mathf.Lerp(transform.position.x, target.position.x + offset.x, acc); 
			moveTo.y = Mathf.Lerp(transform.position.y, target.position.y + offset.y, acc); 
		
			transform.position = new Vector3(moveTo.x, moveTo.y, transform.position.z);
		}

		void CheckBoundaries() {	
			if (noBounds) return;
			CheckHorizontal();
			CheckVertical();
		}

		void CheckHorizontal() {
			float radius = cam.orthographicSize * cam.aspect;

			FloatRange camWidth = new FloatRange(
				transform.position.x - radius,
				transform.position.x + radius
			);
			FloatRange boundsWidth = new FloatRange(
				bounds.center.x - bounds.extents.x,
				bounds.center.x + bounds.extents.x
			);

			bool left = false;
			bool right = false;

			if (camWidth.Min < boundsWidth.Min) {
				left = true;
			}

			if (camWidth.Max > boundsWidth.Max) {
				right = true;
			}

			if (left && right) {
				transform.position = new Vector3(
					bounds.center.x + offset.x,
					transform.position.y,
					transform.position.z
				);
				return;
			}

			if (left) {
				transform.position = new Vector3(
					boundsWidth.Min + radius,
					transform.position.y, 
					transform.position.z
				);
			}

			if (right) {
				transform.position = new Vector3(
					boundsWidth.Max - radius, 
					transform.position.y, 
					transform.position.z
				);
			}
		}

		void CheckVertical() {
			float radius = cam.orthographicSize;

			FloatRange camHeight = new FloatRange(
				transform.position.y - radius,
				transform.position.y + radius
			);
			FloatRange boundsHeight = new FloatRange(
				bounds.center.y - bounds.extents.y,
				bounds.center.y + bounds.extents.y
			);

			bool down = false;
			bool up = false;


			if (camHeight.Min < boundsHeight.Min) {
				down = true;
			}

			if (camHeight.Max > boundsHeight.Max) {
				up = true;
			}

			if (down && up) {
				transform.position = new Vector3(
					transform.position.x,
					bounds.center.y + offset.y,
					transform.position.z
				);
				return;
			}

			if (down) {
				transform.position = new Vector3(
					transform.position.x, 
					bounds.center.y - bounds.extents.y + radius, 
					transform.position.z
				);
			}

			if (up) {
				transform.position = new Vector3(
					transform.position.x, 
					bounds.center.y + bounds.extents.y - radius, 
					transform.position.z
				);
			}
		}

	}

}
