Shader "Hidden/DaylightCycle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
    _daylight ("Texture", 2D) = "white" {}
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
      sampler2D _daylight;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv) * tex2D(_daylight, fixed2(_time, 0));
				return col;
			}
			ENDCG
		}
	}
}
