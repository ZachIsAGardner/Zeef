using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform {
    
	// This is in a seperate file because for whatever reason providing 
	// default values to arguments in a constructor will causes issues with 
	// attaching MonoBehaviours to a GameObject

	public class TextBoxUIPartialModel {

		public string Speaker { get; private set; }

		public bool? Auto { get; private set; }
		public SoundEffectScriptable Tone { get; private set; }
		public float? CrawlTime { get; private set; }

		public TextBoxUIPartialModel(string speaker = "", bool? auto = false, SoundEffectScriptable tone = null, float? crawlTime = 0.05f, Vector3? position = null) {
			Speaker = speaker;

			Auto = auto;
			Tone = tone;
			CrawlTime = crawlTime;
		}
	}

    public class TextBoxUIFullModel : TextBoxUIPartialModel {
		
		public string Text { get; private set; }
	
		public TextBoxUIFullModel(string text = "", string speaker = "", bool? auto = false, SoundEffectScriptable tone = null, float? crawlTime = 0.05f, Vector3? position = null) 
			: base(speaker, auto, tone, crawlTime, position) {
			Text = text;
		}
	}
}