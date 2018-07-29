using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform {

	public class TextBoxOptions {
		
		public bool Auto { get; private set; }
		public SoundEffectsEnum Tone { get; private set; }
		public float Speed { get; private set; }
		public Vector3? Position { get; private set; }
		public Transform Parent { get; private set; }

		public TextBoxOptions(bool auto = false, SoundEffectsEnum tone = 0, float speed = 1, Vector3? position = null, Vector2? size = null, Transform parent = null)
		{
			this.Auto = auto;
			this.Tone = tone;
			this.Speed = speed;
			this.Position = position;
			this.Parent = parent;
		}

		public static TextBoxOptions Default() {
			return new TextBoxOptions(
				auto: false,
				tone: 0,
				speed: 1
			);
		}
	}

}