Shader "Custom/PostProcess"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_VRadius("Vignette Radius", Range(0.0, 1.0)) = 0.8
		_VSoft("Vignette Softness", Range(0.0, 1.0)) = 0.5
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
            #include "UnityCG.cginc"
			
			// Properties
			sampler2D _MainTex;
			float4 _Color;
			float4 _GlitchColor;
			float _VRadius;
			float _VSoft;

			float4 frag(v2f_img input) : COLOR
			{
                // sample texture for color
				float4 base = tex2D(_MainTex, input.uv);
				// average original color and new color
                //base = base * _Color;

				// add vignette
				float distFromCenter = distance(input.uv.xy, float2(0.5, 0.5));
				float vignette = smoothstep(_VRadius, _VRadius - _VSoft, distFromCenter);
				base = saturate(base * vignette);
				
				return base;
			}

			ENDCG
		}
	}
}