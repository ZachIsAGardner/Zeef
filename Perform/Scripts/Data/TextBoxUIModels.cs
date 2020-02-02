using UnityEngine;
using Zeef.Menu;
using Zeef.Sound;

namespace Zeef.Perform
{    
	// This is in a seperate file because for whatever reason providing 
	// default values to arguments in a constructor will causes issues with 
	// attaching MonoBehaviours to a GameObject
	public class TextBoxUIPartialModel
    {
		public string Speaker { get; private set; }
		public bool? Auto { get; private set; }
		public SoundEffectScriptable Tone { get; private set; }
		public float? CrawlTime { get; private set; }
        public bool? CloseWhenDone { get; private set; }

		public TextBoxUIPartialModel(
            string speaker = "",
            bool? auto = false,
            SoundEffectScriptable tone = null,
            float? crawlTime = null,
            Vector3? position = null,
            bool? closeWhenDone = false
        ) {
            Speaker = speaker;
			Auto = auto;
			Tone = tone;
			CrawlTime = crawlTime ?? PerformanceContent.DefaultCrawlTime;
            CloseWhenDone = closeWhenDone;
		}
	}

    public class TextBoxUIFullModel : TextBoxUIPartialModel
    {	
		public string Text { get; private set; }
	
		public TextBoxUIFullModel(
            string text = "", 

            string speaker = "", 
            bool? auto = false, 
            SoundEffectScriptable tone = null, 
            float? crawlTime = null, 
            Vector3? position = null,
            bool? closeWhenDone = false
        ) 
            : base(speaker, auto, tone, crawlTime, position, closeWhenDone)
        {
			Text = text;
		}
	}
}