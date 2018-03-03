
Shader "Hidden/Swirl"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_time("Time between 0 and 1", Range(0, 1)) = 0
		_strength("Strength",Range(0,1)) = 0
	}

		SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" }
		ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};
	struct v2f
	{
		half2 texcoord  : TEXCOORD0;
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
	};

	sampler2D _MainTex;
	float _time;
	float _strength;



	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color;
		return OUT;
	}

	float4 frag(v2f i) : COLOR
	{
		float2 tc = i.texcoord.xy;
		float4 sum = float4(1.0, 1.0, 1.0 + _strength * 0.25, 1.0);

		//blur radius in pixels
		//float blur = radius / resolution / 4;

		float repeat = 3.0;
		
		float pi = 3.14159265359;

		float2 w = float2(sin(tc.y * pi * 2.0 * repeat + _time), sin(tc.x * pi * 2.0 * repeat + _time)) * (_strength);

		sum *= tex2D(_MainTex, tc + w / 2000.0 * 20.0);

		return float4(sum.rgb, 1);
	}
		ENDCG
	}
	}
		Fallback "Sprites/Default"
}