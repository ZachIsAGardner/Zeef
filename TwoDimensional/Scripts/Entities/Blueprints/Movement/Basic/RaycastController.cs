using UnityEngine;
using Zeef.GameManager;

namespace Zeef.TwoDimensional {

	// This is heavily influenced by Sebastian Lague's platformer repo
	// https://github.com/SebLague/2DPlatformer-Tutorial
	public class RaycastController : MonoBehaviour {

		protected Game game;
		[HideInInspector]
		public BoxCollider2D col;
		protected float skin = 1f;
		public OriginInfo origins;
		public int rayCount = 4;

		protected virtual void Start () {
			col = GetComponent<BoxCollider2D>();
			game = Game.Main();
			skin = 1f / game.ppu;
		}

		protected void GetRayOrigins() {
			Bounds bounds = col.bounds;
			bounds.Expand (skin * -2);
			origins.bottomLeft = new Vector2(col.bounds.min.x, col.bounds.min.y);
			origins.bottomRight = new Vector2(col.bounds.max.x, col.bounds.min.y);
			origins.topLeft = new Vector2(col.bounds.min.x, col.bounds.max.y);
			origins.topRight = new Vector2(col.bounds.max.x, col.bounds.max.y);
		}

		public struct OriginInfo {
			public Vector2 bottomLeft, bottomRight, topLeft, topRight;
		}
	}

}