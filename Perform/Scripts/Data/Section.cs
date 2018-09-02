using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform{

	public class Section {

		public TextBoxUIModel TextBoxUIModel { get; set; }
		public Action Action { get; set; }
		
		public Section(TextBoxUIModel model, Action action = null) {
			TextBoxUIModel = model;
			Action = action;
		}
	}
}
