// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Zeef/Boil"
{
    // Define what shows up in Inspector. 
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

        // Boil Start

        _NoiseTex ("Noise Texture", 2D) = "white" {}

        _DistortionDamper ("Distortion Damper", Float) = 10
        _DistortionSpreader ("Distortion Spreader", Float) = 50
        _TimeMultiplier ("Time Multiplier", Float) = 3

        _MyTime ("My Time", Float) = 0

        // Boil End

    }

    // Higher level shader language. Compiles into a bunch of other lower level shader languages.
    SubShader
    {
        // Options for shader.
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        // Start code/ logic.
        CGPROGRAM
            // pragma is an option.

            #pragma vertex MySpriteVert
            #pragma fragment MySpriteFrag

            // Version
            #pragma target 2.0

            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA

            sampler2D _NoiseTex;
            float _DistortionDamper;
            float _DistortionSpreader;
            float _TimeMultiplier;
            float _MyTime;

            // Include a bunch of re-usable stuff from another file. 
            // If you can't find something, it might be in this file.
            #include "UnitySprites.cginc"

            // ---


            // Used in frag
            fixed4 MySampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                    fixed4 alpha = tex2D (_AlphaTex, uv);
                    color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
                #endif

                return color;
            }

            // ---

            struct my_appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct my_v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // ---

            my_v2f MySpriteVert(my_appdata_t IN)
            {
                my_v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.worldPosition = IN.vertex;

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                // OUT.vertex += 0.1f;
            
                OUT.texcoord = IN.texcoord;

                OUT.color = IN.color * _Color * _RendererColor;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 MySpriteFrag(my_v2f IN) : SV_Target
            {
                // Boil Start
                float2 offset = float2(
                    tex2D( _NoiseTex, float2(IN.worldPosition.y / _DistortionSpreader, (_MyTime * _TimeMultiplier))).r,
                    tex2D( _NoiseTex, float2((_MyTime * _TimeMultiplier), IN.worldPosition.x / _DistortionSpreader)).r
                );

                offset -= 0.5f;

                fixed4 color = tex2D (
                    _MainTex, 
                    IN.texcoord + offset / _DistortionDamper
                ) * IN.color;
                // Boil End


                color.rgb *= color.a;

                return color;
            }

            // ---

            
        ENDCG
        }
    }
}
