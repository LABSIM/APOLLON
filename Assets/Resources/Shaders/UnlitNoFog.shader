﻿Shader "Unlit/UnlitNoFog"
{
    Properties
    {
		_Color("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
			ZWrite Off
			ColorMask RGB
			Blend One SrcAlpha // additive no blend
			//Offset - 1, -1
			Fog {
				Mode Off
			}

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= _Color.rgb;
				col.a = 1.0 - _Color.a;
                return col;

            }

            ENDCG
        }
    }
}
