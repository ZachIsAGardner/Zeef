using System.Collections.Generic;

namespace Zeef.Perform.Examples {

    class ExamplePerformance : Performance {

        protected override Branch BranchStart() {
			return new Branch(new List<Section>() {
				new Section(new TextBoxUIModel(
					text: "Hey what's good my dude. I hope you're enjoying Zeef. I'd like to think it's pretty neat...",
					speaker: "Pebbles"
				)),
				new Section(new TextBoxUIModel(
					text: "There's a few cool things you can do with the Zeef.Perform namespace.",
					speaker: "Pebbles"
				)),
				new Section(new TextBoxUIModel(
					text: "TODO: Show them a few cool things they can do with the Zeef.Performe namespace.",
					speaker: "Pebbles"
				)),
				new Section(new TextBoxUIModel(
					text: "TODO: Fix the typo in that last section.",
					speaker: "Pebbles"
				))
			});
        }
    }
}