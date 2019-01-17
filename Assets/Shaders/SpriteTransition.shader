Shader "Custom/SpriteTransition" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
		_SecondTex("SecondaryTexture", 2D) = "white" {}
		_MainTint("MainTint", Color) = (1, 1, 1, 1)
		_SecondTint("SecondaryTint", Color) = (1, 1, 1, 1)
		_ReplaceColor("ReplaceColor", Color) = (1, 1, 1, 1)
		_ReplaceAmount("ReplaceAmount", Range(0, 1)) = 0
		_TransitionHeight("TransitionHeight", Range(0, 1)) = 0
		_TransitionLineWidth("TransitionLineWidth", Range(0, 1)) = 0.05
		_TransitionLineColor("TransitionLineColor", Color) = (0.5, 1, 0.5, 1)
		_DisolveAmount("DisolveAmount", Range(0, 1)) = 0
		_DisolveScale("DisolveScale", Float) = 3
		_DisolveEdgeColor("DisolveEdgeColor", Color) = (1, 0, 0, 1)
		_DisolvePower("DisolvePower", Range(1, 20)) = 3
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
			#include "Simplex2D.hlsl"

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
			half4 _MainTint;
			half4 _SecondTint;
			half4 _ReplaceColor;
			float _ReplaceAmount;
			float _TransitionHeight;
			float _TransitionLineWidth;
			half4 _TransitionLineColor;
			float _DisolveAmount;
			float _DisolveScale;
			half4 _DisolveEdgeColor;
			float _DisolvePower;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
				//float height = (sin(_Time.z) + 1) / 2;
				float height = _TransitionHeight;
				//float2 centerUV = i.uv - float2(0.5, 0.5);
				float dist = i.uv.y;
				//float dist = distance(float2(0.5, 0.5), i.uv);
				//float dist = abs(centerUV.x) + abs(centerUV.y);

                fixed4 mainCol = tex2D(_MainTex, i.uv) * _MainTint;
				float mainMult = step(height, dist);
				float lineMult = step(height - _TransitionLineWidth, dist);
				float secMult = 1 - lineMult;
				lineMult *= 1 - mainMult;
				float inLineHeight = (dist - height + _TransitionLineWidth) / _TransitionLineWidth;
				fixed4 secondCol = tex2D(_SecondTex, i.uv) * _SecondTint;

				fixed4 col = mainCol;
				col.rgb = col.rgb * (1 - _ReplaceAmount) + _ReplaceColor * _ReplaceAmount;
				col *= mainMult;
				col += _TransitionLineColor * lineMult * (mainCol.a * inLineHeight + secondCol.a * (1 - inLineHeight));
				col += secondCol * secMult;

				if (_DisolveAmount > 0.001) {
					//_DisolveAmount = (sin(_Time.z) + 1) / 2;
					float disolve = (snoise(i.worldPos * _DisolveScale) + 1) * 0.5 - _DisolveAmount;
					col.a *= step(0, disolve);
					disolve = pow(1 - disolve, _DisolvePower);
					col.rgb = col.rgb * (1 - disolve) + _DisolveEdgeColor.rgb * disolve;
				}

                return col;
            }
            ENDCG
        }
    }
}
