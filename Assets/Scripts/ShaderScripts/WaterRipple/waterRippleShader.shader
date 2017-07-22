Shader "Custom/Water Ripples" {
		Properties{
			_MainTex("Base (RGB)", 2D) = "white" {}
			_reflectionBlend("% of reflection to use", Range(0, 1)) = 0
			_xOffset("x offset", Float) = 1
			_yOffset("y offset", Float) = 1
			_tileX("Tile x", Float) = 1
			_tileY("Tile y", Float) = 1
			_rippleSpeed("ripple speed", Float) = 1
			_rippleSize("ripple size", Float) = 1
			_backgroundTex("Sky", 2D) = "white" {}
			_bumpTex1("bump", 2D) = "white" {}
			_bumpTex2("bump2", 2D) = "white" {}
			_bumpTex3("bump3", 2D) = "white" {}

		}
			SubShader{
			Pass{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma target 3.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"


#define loop1(x) \
fmod(x, 1) * sign(x)

		uniform sampler2D _MainTex;
		uniform sampler2D _backgroundTex;
		uniform sampler2D _bumpTex1;
		uniform sampler2D _bumpTex2;
		uniform sampler2D _bumpTex3;
		uniform float _reflectionBlend;
		uniform float _rippleSize;
		uniform float _rippleSpeed;
		uniform float _xOffset;
		uniform float _yOffset;
		uniform float _tileX;
		uniform float _tileY;

		float4 frag(v2f_img i) : COLOR{
			//float x = (i.uv.x * _tileX + _Time.x) % 1;
			float xWater = (i.uv.x * _tileX) % 1;
			float yWater = (i.uv.y * _tileY) % 1;
			float xBump = (i.uv.x * _tileX) % 1;
			float yBump = (i.uv.y * _tileY) % 1;

			float2 normal1Cord = (float2(xBump, yBump) + float2(0, 1) * _rippleSpeed * _Time.x) * _rippleSize;
			float2 normal2Cord = (float2(xBump, yBump) + float2(1, -1) * _rippleSpeed * _Time.x) * _rippleSize;
			float2 normal3Cord = (float2(xBump, yBump) + float2(-1, -1) * _rippleSpeed * _Time.x) * _rippleSize;
			float3 normal1 = tex2D(_bumpTex1, loop1(normal1Cord)).rgb;
			float3 normal2 = tex2D(_bumpTex2, loop1(normal2Cord)).rgb;
			float3 normal3 = tex2D(_bumpTex3, loop1(normal3Cord)).rgb;
			float xReflect = ((i.uv + normal1 + normal2 + normal3).x - _xOffset);
			float yReflect = ((i.uv + normal1 + normal2 + normal3).y - _yOffset);
			//y = y + sin(_Time * _rippleSpeed + i.uv.y * _rippleSize) / 2 + .5;
			xReflect = fmod(xReflect, 1) * sign(xReflect);
			yReflect = fmod(yReflect, 1) * sign(yReflect);

			float4 reflection = tex2D(_backgroundTex, float2(xReflect, yReflect));
			float4 c = tex2D(_MainTex, float2(xWater, yWater));


			float4 ripples = c;
			ripples.rgb = lerp(c.rgb, reflection.rgb, _reflectionBlend);

			return ripples; 
		}
			ENDCG
		}
		}
	}