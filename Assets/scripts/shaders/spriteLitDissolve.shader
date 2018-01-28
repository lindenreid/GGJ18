Shader "Custom/SpriteLitDissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
		_Dissolve("Dissolve", int) = 0
		_DissolveSpeed("Dissolve Speed", float) = 1.0
		_DissolveColor1("Dissolve Color 1", Color) = (1, 1, 1, 1)
		_DissolveColor2("Dissolve Color 2", Color) = (1, 1, 1, 1)
		_ColorThreshold1("Color Threshold 1", float) = 1.0
		_ColorThreshold2("Color Threshold 2", float) = 1.0
		_StartTime("Start Time", float) = 1.0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap noinstancing
        #include "UnitySprites.cginc"

		// Properties
		float4 _DissolveColor1;
		float4 _DissolveColor2;
		sampler2D _NoiseTex;
		float _DissolveSpeed;
		float _ColorThreshold1;
		float _ColorThreshold2;
		int _Dissolve;
		float _StartTime;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
			fixed4 texcoord;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color;
			o.texcoord = v.texcoord;
        }

        void surf (Input input, inout SurfaceOutput o)
        {
            fixed4 sprite = SampleSpriteTexture (input.uv_MainTex) * input.color;
			fixed3 albedo = fixed3(0,0,0);
			fixed alpha = 0;

			if (_Dissolve == 1)
			{
				fixed4 color = sprite;

				// sample noise texture
				fixed noiseSample = tex2Dlod(_NoiseTex, input.texcoord);

				// dissolve colors
				fixed thresh2 = (_Time.y - _StartTime) * _ColorThreshold2;
				fixed useDissolve2 = noiseSample - thresh2 < 0;
				color = (1-useDissolve2)*color + useDissolve2*_DissolveColor2;

				fixed thresh1 = (_Time.y - _StartTime) * _ColorThreshold1;
				fixed useDissolve1 = noiseSample - thresh1 < 0;
				color = (1-useDissolve1)*color + useDissolve1*_DissolveColor1;

				fixed threshold = (_Time.y - _StartTime) * _DissolveSpeed;
				clip(noiseSample - threshold);

				albedo = color.rgb * _Color.rgb;
				alpha = color.a * _Color.a;
			}
			else
			{
				albedo = sprite.rgb * sprite.a * _Color.rgb;
				alpha = sprite.a * _Color.a;
			}

            o.Albedo = albedo;
            o.Alpha = alpha;
        }
        ENDCG
    }

Fallback "Sprites/Diffuse"
}
