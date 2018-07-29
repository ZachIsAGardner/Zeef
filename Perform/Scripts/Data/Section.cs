using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeef.Perform{

	public class Section {

		public string text;
		public string speaker;
		public TextBoxOptions options;
		public Action action;
		
		public Section(string text = "", TextBoxOptions options = null, Action action = null, string speaker = null) {
			this.text = text;
			this.options = options;
			this.action = action;
			this.speaker = speaker;
		}
	}
}
