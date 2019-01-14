Shader "Custom/SpriteTransition" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_GridColor("GridColor", Color) = (1, 1, 1, 1)
		_GridLineWidth("GridLineWidth", Float) = 0.05
		_GridSize("GridSize", Float) = 0.5
		_BorderColor("BorderColor", Color) = (1, 1, 1, 1)
		_BorderLineWidth("BorderLineWidth", Float) = 0.1
		_FadeWidth("FadeWidth", Float) = 0.1
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
				float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; float4 _MainTex_ST;
			sampler2D _SecondTex; float4 _SecondTex_ST;
			float4 _GridColor;
			float _GridLineWidth;
			float _GridSize;
			float4 _BorderColor;
			float _BorderLineWidth;
			float _FadeWidth;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
				float l = 1 - step(_GridLineWidth, frac(i.worldPos.y / _GridSize));
				l += 1 - step(_GridLineWidth, frac(i.worldPos.x / _GridSize));
				l = clamp(l, 0, 1);
				float b = 1 - step(_BorderLineWidth, frac(i.uv.y - (1 - i.uv.x) / 2));
				b += 1 - step(_BorderLineWidth, frac(1 - i.uv.y - (1 - i.uv.x) / 2));
				b = clamp(b, 0, 1);
				l *= 1 - b;
				float fade = 1 - (i.uv.x - (1 - _FadeWidth)) / _FadeWidth;
				fixed4 col = clamp(_GridColor * l + _BorderColor * b, 0, 1);
				col.a *= fade;
                return col;
            }
            ENDCG
        }
    }
}
