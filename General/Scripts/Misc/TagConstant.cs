using System.Collections.Generic;

namespace Zeef 
{
    public static partial class TagConstant 
    {
        // Unity default
        public const string Respawn = "Respawn";
        public const string Finish = "Finish";
        public const string EditorOnly = "EditorOnly";
        public const string MainCamera = "MainCamera";
        public const string Player = "Player";
        public const string GameController = "GameController";

        // Zeef default
        public const string SceneCanvas = "SceneCanvas";
        public const string CameraBounds = "CameraBounds";
        public const string DynamicFolder = "DynamicFolder";
        public const string DynamicCanvasFolder = "DynamicCanvasFolder"; 
        public const string ParticlesFolder = "ParticlesFolder";

        public static List<string> All() 
        {
            // Default tags
            List<string> result = new List<string>() {
                // Unity default
                Respawn,
                Finish,
                EditorOnly,
                MainCamera,
                Player,
                GameController,

                // Zeef default
                SceneCanvas,
                CameraBounds, 
                DynamicFolder,
                DynamicCanvasFolder,
                ParticlesFolder
            };

            // Add extra extended elements
            Extra(ref result);

            return result;
        } 

        static partial void Extra(ref List<string> original);
    }
}