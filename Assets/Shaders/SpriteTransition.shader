Shader "Custom/SpriteTransition" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_SecondTex("SecondaryTexture", 2D) = "white" {}
		_ReplaceColor("ReplaceColor", Color) = (1, 1, 1, 1)
		_ReplaceAmount("ReplaceAmount", Range(0, 1)) = 0
		_TransitionHeight("TransitionHeight", Range(0, 1)) = 0
		_TransitionLineWidth("TransitionLineWidth", Range(0, 1)) = 0.05
		_TransitionLineColor("TransitionLineColor", Color) = (0.5, 1, 0.5, 1)
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
			sampler2D _SecondTex; float4 _SecondTex_ST;
			half4 _ReplaceColor;
			float _ReplaceAmount;
			float _TransitionHeight;
			float _TransitionLineWidth;
			half4 _TransitionLineColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
				//float height = (sin(_Time.z) + 1) / 2;
				float height = _TransitionHeight;
				//float2 centerUV = i.uv - float2(0.5, 0.5);
				float dist = i.uv.y;
				//float dist = distance(float2(0.5, 0.5), i.uv);
				//float dist = abs(centerUV.x) + abs(centerUV.y);

                fixed4 mainCol = tex2D(_MainTex, i.uv);
				float mainMult = step(height, dist);
				float lineMult = step(height - _TransitionLineWidth, dist);
				float secMult = 1 - lineMult;
				lineMult *= 1 - mainMult;
				float inLineHeight = (dist - height + _TransitionLineWidth) / _TransitionLineWidth;
				fixed4 secondCol = tex2D(_SecondTex, i.uv);

				fixed4 col = mainCol;
				col.rgb = col.rgb * (1 - _ReplaceAmount) + _ReplaceColor * _ReplaceAmount;
				col *= mainMult;
				col += _TransitionLineColor * lineMult * (mainCol.a * inLineHeight + secondCol.a * (1 - inLineHeight));
				col += secondCol * secMult;
                return col;
            }
            ENDCG
        }
    }
}
