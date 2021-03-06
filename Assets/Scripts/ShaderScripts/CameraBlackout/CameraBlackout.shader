﻿Shader "Unlit/CameraBlackout"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black & White blend", Range (0, 1)) = 0
		_x1 ("x1", Float) = 0
		_y1 ("y1", Float) = 0
		_x2 ("x2", Float) = 100
		_y2 ("y2", Float) = 0
		_x3 ("x3", Float) = 100
		_y3 ("y3", Float) = 100
		_x4 ("x4", Float) = 0
		_y4 ("y4", Float) = 100
		_backgroundTex("Backgroud texture", 2D) = "white" {}
			
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

#define sign(x1, y1, x2, y2, x3, y3) \
(x1 - x3) * (y2 - y3) - (x2 - x3) * (y1 - y3)
 
			uniform sampler2D _MainTex;
			uniform float _bwBlend;
			uniform float _x1;
			uniform float _y1;
			uniform float _x2;
			uniform float _y2;
			uniform float _x3;
			uniform float _y3;
			uniform float _x4;
			uniform float _y4;
 
			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				float3 black = float3( 0, 0, 0 ); 


				float lum = c.r*.3 + c.g*.59 + c.b*.11;
				float3 bw = float3(lum, lum, lum);
				c.rgb = lerp(c.rgb, bw, clamp(_bwBlend * 3, 0, 1));
				c.rgb = lerp(c.rgb, black, clamp(_bwBlend, 0, 1));



				bool b1;
				bool b2;
				bool b3;
				int tri1;
				int tri2;

				b1 = sign(i.pos.x, i.pos.y, _x1, _y1, _x2, _y2) <= 0.0f;
				b2 = sign(i.pos.x, i.pos.y, _x2, _y2, _x3, _y3) <= 0.0f;
				b3 = sign(i.pos.x, i.pos.y, _x3, _y3, _x1, _y1) <= 0.0f;
				
				tri1 = (b1 == b2) && (b2 == b3);


				b1 = sign(i.pos.x, i.pos.y, _x1, _y1, _x3, _y3) <= 0.0f;
				b2 = sign(i.pos.x, i.pos.y, _x3, _y3, _x4, _y4) <= 0.0f;
				b3 = sign(i.pos.x, i.pos.y, _x4, _y4, _x1, _y1) <= 0.0f;
				tri2 = (b1 == b2) && (b2 == b3);



				float4 clipped = c;
				clipped.rgb = lerp(c.rgb, black, clamp(!(tri1 | tri2), 0, 1));

				return clipped;
			}
			ENDCG
		}
	}
}