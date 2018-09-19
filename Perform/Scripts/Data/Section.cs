using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform{

	public class Section {

		public TextBoxUIFullModel TextBoxUIModel { get; set; }
		public Action Action { get; set; }
		
		public Section(TextBoxUIFullModel textBoxModel, Action action = null) {
			TextBoxUIModel = textBoxModel;
			Action = action;
		}
	}
}
