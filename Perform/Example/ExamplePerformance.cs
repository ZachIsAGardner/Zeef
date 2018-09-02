using System.Collections.Generic;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform.Example {

    class ExamplePerformance : Performance {

		[SerializeField] SoundEffectScriptable tone;

        protected override Branch BranchStart() {

			return new Branch(
				textBoxUIModel: new TextBoxUIModel(
					speaker: "Pebbles",
					crawlTime: 0.05f,
					tone: tone
				),
				sections: new List<Section>() {
					new Section(new TextBoxUIModel(
						text: "Hey what's good my dude. I hope you're enjoying Zeef. I'd like to think it's pretty neat..."
					)),

					new Section(
						textBoxUIModel: new TextBoxUIModel(
							text: "There's a few cool things you can do with the Zeef.Perform namespace."
						),
						action: () => {
							Debug.Log("You can even execute actions mid performance!");
						}
					),

					new Section(new TextBoxUIModel(
						text: "TODO: Show them a few cool things they can do with the Zeef.Performe namespace."
					)),

					new Section(new TextBoxUIModel(
						text: "TODO: Fix the typo in that last section."
					))
				}
			);
        }
    }
}