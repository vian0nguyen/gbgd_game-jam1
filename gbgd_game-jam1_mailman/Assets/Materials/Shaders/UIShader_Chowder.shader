// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
//Credit: https://www.reddit.com/r/Unity3D/comments/75vzrx/help_with_the_final_step_of_an_unmoving_plaid_ui/do9dg2u/?context=8&depth=9 By u/EmmetOT and u/Devil_Spawn on Reddit
//Overlay Code: https://elringus.me/blend-modes-in-unity/ By ARTYOM SOVETNIKOV
Shader "Custom/UnmovingPlaid"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Detail("Detail", 2D) = "white" {}
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
		ZWrite Off
		ZTest Always
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _Detail;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 Overlay(fixed4 a, fixed4 b)
			{
				fixed4 r = a < .5 ? 2.0 * a * b : 1.0 - 2.0 * (1.0 - a) * (1.0 - b);
				r.a = b.a;
				return r;
			}

			float4 frag(v2f i) : SV_Target
			{
				//multiplies screen position by the overlay image
				fixed4 col = tex2D(_Detail, 2 * i.screenPos);
				
				col = Overlay(tex2D(_MainTex, i.uv), tex2D(_Detail, 2 * i.screenPos));
				//returns the overlay image's colors + main texture's alpha using the main texture alpha and uv's (alpha is used for mask)
				return fixed4(col.r, col.g, col.b, tex2D(_MainTex, i.uv).a);
			}

			ENDCG

		}
	}
}