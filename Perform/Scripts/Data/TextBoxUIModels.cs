using UnityEngine;
using Zeef.Menu;
using Zeef.Sound;

namespace Zeef.Perform
{    
	public class TextBoxUIPartialModel
    {
		public string Speaker { get; private set; }
		public bool? Auto { get; private set; }
		public SoundEffectScriptable Tone { get; private set; }
		public float? CrawlTime { get; private set; }
        public bool? CloseWhenDone { get; private set; }
        public int? ToneIntervalMax { get; private set; }

		public TextBoxUIPartialModel(
            string speaker = "",
            bool? auto = false,
            SoundEffectScriptable tone = null,
            float? crawlTime = null,
            Vector3? position = null,
            bool? closeWhenDone = false,
            int? toneIntervalMax = null
        ) {
            Speaker = speaker;
			Auto = auto;
			Tone = tone;
			CrawlTime = crawlTime ?? PerformanceContent.DefaultCrawlTime;
            CloseWhenDone = closeWhenDone;
            ToneIntervalMax = toneIntervalMax;
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
            bool? closeWhenDone = false,
            int? toneIntervalMax = null
        ) 
            : base(speaker, auto, tone, crawlTime, position, closeWhenDone, toneIntervalMax)
        {
			Text = text;
		}
	}
}