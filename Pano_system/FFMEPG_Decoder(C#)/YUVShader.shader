// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/YUVRender"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_UVTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _UVTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 uv4 = tex2D(_UVTex, i.uv);
				float y = 1.1643 * (col.a - 0.0625);
				float u = (uv4.r * 15 * 16 + uv4.g * 15) / 255 - 0.5;
				float v = (uv4.b * 15 * 16 + uv4.a * 15) / 255 - 0.5;

				//float r = y + 1.596 * v;
				//float g = y - 0.391  * u - 0.813 * v;
				//float b = y + 2.018  * u;
				float r = y + 1.403 * v;
				float g = y - 0.344  * u - 0.714 * v;
				float b = y + 1.770  * u;
				col.rgba = float4(r, g, b, 1.f);
				return col;
			}
			ENDCG
		}
	}
}

