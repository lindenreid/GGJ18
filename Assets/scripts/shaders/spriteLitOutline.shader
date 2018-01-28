Shader "Custom/SpriteLit"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        
        Pass
        {
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

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

            struct Input
            {
                float2 uv_MainTex;
                fixed4 color;
            };

            void vert (inout appdata_full v, out Input o)
            {
                UNITY_INITIALIZE_OUTPUT(Input, o);
                o.color = v.color;
            }

            float4 frag (Input input)
            {
                fixed4 o = fixed4(0,0,0,0);

                fixed4 sprite = SampleSpriteTexture (input.uv_MainTex) * input.color;
                o.Albedo = sprite.rgb * sprite.a * _Color.rgb;
                o.Alpha = sprite.a * _Color.a;
            }
            ENDCG
        }
    }

Fallback "Sprites/Diffuse"
}