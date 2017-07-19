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
			_bumpTex("bump", 2D) = "white" {}

		}
			SubShader{
			Pass{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma target 3.0

#include "UnityCG.cginc"
#include "UnityUI.cginc"


#define sign(x1, y1, x2, y2, x3, y3) \
(x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3)

		uniform sampler2D _MainTex;
		uniform sampler2D _backgroundTex;
		uniform sampler2D _bumpTex;
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

			float normal = tex2D(_bumpTex, float2(xBump, yBump)).rgb;
			float xReflect = ((i.uv + normal).x + _xOffset);
			float yReflect = ((i.uv + normal).y + _yOffset);
			//y = y + sin(_Time * _rippleSpeed + i.uv.y * _rippleSize) / 2 + .5;
			xReflect = xReflect % 1;
			yReflect = yReflect % 1;

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