using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional {

	public class Camera2D : MonoBehaviour {

		[SerializeField] private Vector3 offset;
		[SerializeField] private Transform target;

		// 0: Never move
		// 1: Insta snap
		[Range (0, 1)]
		[SerializeField] private float acc = .5f;

		private float normalZoom;
		private bool zooming;
		private float zoom;

		private Bounds? bounds;

		private Camera cam;

		private float shakeTime;
		private float shakeTick;
		private Vector3 shakeStart;

		// ---

		void Start () {
			cam = GetComponent<Camera>();

			normalZoom = cam.orthographicSize;

			if (target == null) GetTarget();
			GetBoundaries();
			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			GetTarget();
			GetBoundaries();
		}

		void GetTarget() {
			GameObject player = GameObject.FindGameObjectWithTag(TagConstants.Player);
			target = (player) ? player.transform : null;
		}

		void GetBoundaries() {
			GameObject boundsObject = GameObject.FindGameObjectWithTag(TagConstants.CameraBounds);

			if (boundsObject) bounds = boundsObject.GetComponent<BoxCollider2D>().bounds;
			else bounds = null;	
		}

		// ---
		
		void LateUpdate() {
			if (GameManager.IsPaused()) return;

			if (target) Move();
			else GetTarget(); // bad
			
			shakeTime -= 1 * Time.deltaTime;
			if (shakeTime > 0) Shaking();
			
			if (zooming) Zooming();
			
			CheckBoundaries();
		}

		// ---

		public void ResetZoom() {
			zoom = normalZoom;
		}

		public void ChangeZoom(float newZoom) {
			zooming = true;
			zoom = newZoom;
		}

		void Zooming() {
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, 0.125f);
			if (Mathf.Abs(cam.orthographicSize - zoom) < 1) {
				cam.orthographicSize = zoom;
				zooming = false;
			}
		}

		// ---

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

		// ---

		public void Move() {
			Vector2 moveTo = Vector2.zero;
			if (acc < 1) {	
				moveTo.x = Mathf.Lerp(transform.position.x, target.position.x + offset.x, acc); 
				moveTo.y = Mathf.Lerp(transform.position.y, target.position.y + offset.y, acc); 
			} else {
				moveTo.x = target.position.x + offset.x;
				moveTo.y = target.position.y + offset.y;
			}
			transform.position = new Vector3(moveTo.x, moveTo.y, transform.position.z);
		}

		void CheckBoundaries() {	
			if (bounds == null) return;
			CheckHorizontal();
			CheckVertical();
		}

		void CheckHorizontal() {
			float camExtents = cam.orthographicSize * cam.aspect;

			if (camExtents >= bounds.Value.extents.x) {
				transform.position = new Vector3(
					bounds.Value.center.x + offset.x,
					transform.position.y,
					transform.position.z
				);
			} else {
				float camLeft = transform.position.x - camExtents;
				float camRight = transform.position.x + camExtents;

				float boundsLeft = bounds.Value.center.x - bounds.Value.extents.x;
				float boundsRight = bounds.Value.center.x + bounds.Value.extents.x;

				if (camLeft < boundsLeft) {
					transform.position = new Vector3(
						boundsLeft + camExtents,
						transform.position.y, 
						transform.position.z
					);
				} else if (camRight > boundsRight) {
					transform.position = new Vector3(
						boundsRight - camExtents, 
						transform.position.y, 
						transform.position.z
					);
				}
			}
		}

		void CheckVertical() {
			float camExtents = cam.orthographicSize;

			if (camExtents > bounds.Value.extents.y) {
				transform.position = new Vector3(
					transform.position.x,
					bounds.Value.center.y + offset.y,
					transform.position.z
				);
			} else {
				float camBottom = transform.position.y - camExtents;
				float camTop = transform.position.y + camExtents;

				float boundsBottom = bounds.Value.center.y - bounds.Value.extents.y;
				float boundsTop = bounds.Value.center.y + bounds.Value.extents.y;

				if (camBottom < boundsBottom) {
					transform.position = new Vector3(
						transform.position.x, 
						bounds.Value.center.y - bounds.Value.extents.y + camExtents, 
						transform.position.z
					);
				} else if (camTop > boundsTop) {
					transform.position = new Vector3(
						transform.position.x, 
						bounds.Value.center.y + bounds.Value.extents.y - camExtents, 
						transform.position.z
					);
				}
			}
		}
	}
}
