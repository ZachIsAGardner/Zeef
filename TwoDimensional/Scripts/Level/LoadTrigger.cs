using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeef.TwoDimensional;
using Zeef.GameManager;

namespace Zeef.TwoDimensional {

  [RequireComponent (typeof(BoxCollider2D))]
  public class LoadTrigger : InteractableObject {

    public SceneInfo sceneInfo;

      protected override void TriggerAction()
      {
        Game.Main().LoadScene(sceneInfo);
      }
  }

}
