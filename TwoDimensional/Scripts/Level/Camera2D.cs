using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ---
using Zeef.GameManagement;

namespace Zeef.TwoDimensional 
{
	public class Camera2D : MonoBehaviour 
	{
		public Vector3 Offset;
		public Transform Target;

		// 0: Never move
		// 1: Insta snap
		[Range (0, 1)]
		public float Acceleration = .5f;

		private float normalZoom;
		public float NormalZoom { get => normalZoom; }
		private bool zooming;
		private float destinationZoom;
		private float zoomTime = 0.125f;

		private Bounds? bounds;

		private Camera cam;

		private float shakeTime;
		private float shakeTick;
		private Vector3 shakeStart;

		// ---

		void Start () 
		{
			cam = GetComponent<Camera>();

			normalZoom = cam.orthographicSize;

			GetBoundaries();
			
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
		{
			GetBoundaries();
		}

		void GetBoundaries() 
		{
			GameObject boundsObject = GameObject.FindGameObjectWithTag("CameraBounds");

			if (boundsObject) 
				bounds = boundsObject.GetComponent<BoxCollider2D>().bounds;
			else 
				bounds = null;	
		}

		// ---
		
		void LateUpdate() 
		{
			if (!GameState.IsPlaying) return;

			if (Target != null) 
				Move();
			
			shakeTime -= 1 * Time.deltaTime;
			if (shakeTime > 0) 
				Shaking();
			
			if (zooming) 
				Zooming();
			
			CheckBoundaries();
		}

		// ---

		public void ResetZoom(float zoomTime = 0.125f) 
		{
			this.zoomTime = zoomTime;
			zooming = true;
			destinationZoom = normalZoom;
		}

		public void ChangeZoom(float newZoom, float zoomTime = 0.125f) 
		{
			this.zoomTime = zoomTime;
			zooming = true;
			destinationZoom = newZoom;
		}

		void Zooming() 
		{
			cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, destinationZoom, zoomTime);
			if (Mathf.Abs(cam.orthographicSize - destinationZoom) < 0.01f) 
			{
				cam.orthographicSize = destinationZoom;
				zooming = false;
			}
		}

		// ---

		public void Shake(float newShakeTime) 
		{
			shakeTime = newShakeTime;
			shakeTick = 0;
			shakeStart = transform.position;
		}

		void Shaking() 
		{
			shakeTick += 1 * Time.deltaTime;
			if (shakeTick >= 0.01f) 
			{
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

		public virtual void Move() 
		{
			Vector2 moveTo = Vector2.zero;

			if (Acceleration < 1) 
			{	
				moveTo.x = Mathf.Lerp(
					transform.position.x, 
					Target.position.x + Offset.x, 
					Acceleration
				); 

				moveTo.y = Mathf.Lerp(
					transform.position.y, 
					Target.position.y + Offset.y, 
					Acceleration
				); 
			} 
			else 
			{
				moveTo.x = Target.position.x + Offset.x;
				moveTo.y = Target.position.y + Offset.y;
			}

			transform.position = new Vector3(moveTo.x, moveTo.y, transform.position.z);
		}

		void CheckBoundaries() 
		{	
			if (bounds == null) 
				return;

			CheckHorizontal();
			CheckVertical();
		}

		void CheckHorizontal() 
		{
			float camExtents = cam.orthographicSize * cam.aspect;

			if (camExtents >= bounds.Value.extents.x) 
			{
				transform.position = new Vector3(
					bounds.Value.center.x + Offset.x,
					transform.position.y,
					transform.position.z
				);
			} 
			else 
			{
				float camLeft = transform.position.x - camExtents;
				float camRight = transform.position.x + camExtents;

				float boundsLeft = bounds.Value.center.x - bounds.Value.extents.x;
				float boundsRight = bounds.Value.center.x + bounds.Value.extents.x;

				if (camLeft < boundsLeft) 
				{
					transform.position = new Vector3(
						boundsLeft + camExtents,
						transform.position.y, 
						transform.position.z
					);
				} 
				else if (camRight > boundsRight) 
				{
					transform.position = new Vector3(
						boundsRight - camExtents, 
						transform.position.y, 
						transform.position.z
					);
				}
			}
		}

		void CheckVertical() 
		{
			float camExtents = cam.orthographicSize;

			if (camExtents > bounds.Value.extents.y) 
			{
				transform.position = new Vector3(
					transform.position.x,
					bounds.Value.center.y + Offset.y,
					transform.position.z
				);
			} 
			else 
			{
				float camBottom = transform.position.y - camExtents;
				float camTop = transform.position.y + camExtents;

				float boundsBottom = bounds.Value.center.y - bounds.Value.extents.y;
				float boundsTop = bounds.Value.center.y + bounds.Value.extents.y;

				if (camBottom < boundsBottom) 
				{
					transform.position = new Vector3(
						transform.position.x, 
						bounds.Value.center.y - bounds.Value.extents.y + camExtents, 
						transform.position.z
					);
				} 
				else if (camTop > boundsTop) 
				{
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
