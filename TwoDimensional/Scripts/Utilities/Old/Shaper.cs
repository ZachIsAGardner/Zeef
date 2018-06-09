using UnityEngine;
using UnityEngine.UI;

// Good for making shapes
namespace ZeeUtil {

    public static class Shaper {

        public static GameObject CreateSquare() {
            GameObject shape = new GameObject();
            SpriteRenderer sprite = shape.AddComponent<SpriteRenderer>();
            sprite.sprite = Resources.Load<Sprite>("Images/Shapes/16x16/square");
            return shape;
        }
        
    }

}