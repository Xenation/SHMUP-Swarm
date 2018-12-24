Shader "Custom/SpriteColorReplacable" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_ReplaceColor("ReplaceColor", Color) = (1, 1, 1, 1)
		_ReplaceAmount("ReplaceAmount", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"IgnoreProjectors"="True"
		}

        Pass {
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; float4 _MainTex_ST;
			half4 _ReplaceColor;
			float _ReplaceAmount;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = col.rgb * (1 - _ReplaceAmount) + _ReplaceColor * _ReplaceAmount;
                return col;
            }
            ENDCG
        }
    }
}
