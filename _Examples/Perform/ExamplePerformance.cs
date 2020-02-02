using System.Collections.Generic;
using UnityEngine;
using Zeef.Sound;

namespace Zeef.Perform.Example
{
    class ExamplePerformance : Performance
    {
		[SerializeField] SoundEffectScriptable tone;
		
		private TextBoxUIPartialModel BranchOptions { get =>
			new TextBoxUIPartialModel(
				speaker: "Mrs. Zeef",
				crawlTime: 0.05f,
				tone: tone
			);
		}

		// ---
		// Branches

        protected override Branch BranchStart()
        {

			return new Branch(
				textBoxModel: BranchOptions,

				sections: new List<Section>()
                {
					new Section(
						textBoxModel: new TextBoxUIFullModel(
							text: "Hey what's good my dude. I hope you're enjoying Zeef. I'd like to think it's pretty neat..."
						)
					),

					new Section(
						textBoxModel: new TextBoxUIFullModel(
							text: "There's a few cool things you can do with the Zeef.Perform namespace."
						),
						startAction: async () => {
							Debug.Log("You can even execute actions mid performance!");
						}
					),

					new Section(
						textBoxModel: new TextBoxUIFullModel(
							text: "Would you like to see?"
						)
					)
				},

				paths: new List<Path>()
                {
					new Path("No", No),
					new Path("Yes", Yes),
				}
			);
        }

		private Branch Yes => new Branch(
			textBoxModel: BranchOptions,

			sections: new List<Section>()
            {
				new Section(
					textBoxModel: new TextBoxUIFullModel(
						text: "Okay so that was one of them. You know, being able to answer questions is pretty cool..."
					)
				),
				new Section(
					textBoxModel: new TextBoxUIFullModel(
						text: "Okay I'm done for now actually... I'll come up with more examples later."
					)
				)
			}
		);
		

        private Branch No => new Branch(
            textBoxModel: BranchOptions,

            sections: new List<Section>()
            {
                new Section(
                    textBoxModel: new TextBoxUIFullModel(
                        text: "Oh okay, that's fair I guess."
                    )
                ),
                new Section(
                    textBoxModel: new TextBoxUIFullModel(
                        text: "Bye."
                    )
                ),
            }
        );	
    }
}