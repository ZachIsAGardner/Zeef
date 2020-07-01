using System.Collections.Generic;
using UnityEngine;
using Zeef.Menu;
using Zeef.Sound;

namespace Zeef.Perform
{    
    [System.Serializable]
	public class TextBoxUIPartialModel
    {
		public string Speaker;
		public bool? Auto;
		public SoundEffectScriptable Tone;
		public float? CrawlTime;
        public bool? CloseWhenDone;
        public int? ToneIntervalMax;
        public List<string> ProceedInputs = null;

		public TextBoxUIPartialModel(
            string speaker = "",
            bool? auto = false,
            SoundEffectScriptable tone = null,
            float? crawlTime = null,
            bool? closeWhenDone = false,
            int? toneIntervalMax = null,
            List<string> proceedInputs = null
        ) {
            Speaker = speaker;
			Auto = auto;
			Tone = tone;
			CrawlTime = crawlTime ?? PerformanceContent.DefaultCrawlTime;
            CloseWhenDone = closeWhenDone;
            ToneIntervalMax = toneIntervalMax;
            ProceedInputs = proceedInputs;
		}
	}

    [System.Serializable]
    public class TextBoxUIFullModel : TextBoxUIPartialModel
    {	
		public string Text;
	
		public TextBoxUIFullModel(
            string text = null, 

            string speaker = null, 
            bool? auto = false, 
            SoundEffectScriptable tone = null, 
            float? crawlTime = null, 
            bool? closeWhenDone = false,
            int? toneIntervalMax = null,
            List<string> proceedInputs = null
        ) 
            : base(speaker, auto, tone, crawlTime, closeWhenDone, toneIntervalMax, proceedInputs)
        {
			Text = text;
		}
	}
}