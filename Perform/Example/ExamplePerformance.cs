using System.Collections.Generic;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform.Example {

    class ExamplePerformance : Performance {

		[SerializeField] SoundEffectScriptable tone;
		
		private TextBoxUIModel BranchOptions() {
			return new TextBoxUIModel(
				speaker: "Pebbles",
				crawlTime: 0.05f,
				tone: tone
			);
		}

		// ---
		// Branches

        protected override Branch BranchStart() {

			return new Branch(
				model: BranchOptions(),

				sections: new List<Section>() {
					new Section(
						model: new TextBoxUIModel(
							text: "Hey what's good my dude. I hope you're enjoying Zeef. I'd like to think it's pretty neat..."
						)
					),

					new Section(
						model: new TextBoxUIModel(
							text: "There's a few cool things you can do with the Zeef.Perform namespace."
						),
						action: () => {
							Debug.Log("You can even execute actions mid performance!");
						}
					),

					new Section(
						model: new TextBoxUIModel(
							text: "Would you like to see?"
						)
					)
				},

				paths: new List<Path>() {
					new Path("No", No()),
					new Path("Yes", Yes()),
				}
			);
        }

		private Branch Yes() {
			return new Branch(
				model: BranchOptions(),

				sections: new List<Section>() {
					new Section(
						model: new TextBoxUIModel(
							text: "Okay so that was one of them. You know, being able to answer questions is pretty cool..."
						)
					),
					new Section(
						model: new TextBoxUIModel(
							text: "Okay I'm done for now actually... I'll come up with more examples later."
						)
					),
					new Section(
						model: new TextBoxUIModel(
							text: "Only so many hours in a day na mean?"
						)
					)
				}
			);
		} 

		private Branch No() {
			return new Branch(
				model: BranchOptions(),

				sections: new List<Section>() {
					new Section(
						new TextBoxUIModel(
							"Oh okay, that's fair I guess."
						)
					),
					new Section(
						new TextBoxUIModel(
							"Bye."
						)
					),
				}
			);
		} 
    }
}