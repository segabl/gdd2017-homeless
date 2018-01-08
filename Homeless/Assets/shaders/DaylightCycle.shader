Shader "Hidden/DaylightCycle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_time ("Time between 0 and 1", Range (0, 1)) = 0
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

			uniform float _time;

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float f = 1;
				if (_time >= 0.5) {
					// TODO: give night some more time
					f = max(abs(((_time - 0.5) * 2) - 0.5) * 2, 0.5);
				}
				return fixed4(col.r * f, col.g * f, col.b, col.a);
			}
			ENDCG
		}
	}
}
