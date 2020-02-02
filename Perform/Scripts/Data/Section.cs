using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform
{
	public class Section
    {
		public TextBoxUIFullModel TextBoxUIModel { get; set; }
		public Func<Task> StartAction { get; set; }
		public Func<Task> EndAction { get; set; }
		
		/// <summary>
		/// An individual Section of a Rranch of a Performance.
		/// <param name="textBoxModel">The model describing the text box to be generated.</param>
		/// <param name="startAction">The awaited task that runs before the text box is generated and displayed.</param>
		/// <param name="endAction">The awaited task that runs after the text box is generated and displayed.</param>
		/// </summary>
		public Section(TextBoxUIFullModel textBoxModel, Func<Task> startAction = null, Func<Task> endAction = null)
        {
			TextBoxUIModel = textBoxModel;
			StartAction = startAction;
			EndAction = endAction;
		}
	}
}
