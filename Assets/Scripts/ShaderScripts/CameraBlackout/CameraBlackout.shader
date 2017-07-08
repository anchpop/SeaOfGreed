/*Shader "MyShader"
{
	Properties
	{
		_MyFloat("X", Float) = 0
		_MyFloat("Y", Float) = 0
		_MyFloat("W", Float) = 64
		_MyFloat("H", Float) = 64
	}

	SubShader
	{
		float _X;
		float _Y;
		float _W;
		float _H;
		// The code of your shaders
		// - surface shader
		//    OR
		// - vertex and fragment shader
		//    OR
		// - fixed function shader
	}
}*/

Shader "Unlit/CameraBlackout"
{
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black & White blend", Range (0, 1)) = 0
		_x1 ("x1", Float) = 0
		_y1 ("y1", Float) = 0
		_x2 ("x2", Float) = 100
		_y2 ("y2", Float) = 100
		_backgroundTex("Backgroud texture", 2D) = "white" {}
			
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
			uniform float _bwBlend;
			uniform float _x1;
			uniform float _y1;
			uniform float _x2;
			uniform float _y2;
 
			float4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);
				
				float lum = c.r*.3 + c.g*.59 + c.b*.11;
				float3 black = float3( 0, 0, 0 ); 
				
				float4 left = c;
				left.rgb = lerp(left.rgb, black, clamp(_x1 - i.pos.x, 0, 1));
				float4 right = left;
				right.rgb = lerp(right.rgb, black, clamp(i.pos.x - _x2, 0, 1));
				float4 top = right;
				top.rgb = lerp(top.rgb, black, clamp(_y1 - i.pos.y, 0, 1));
				float4 bottom = top;
				bottom.rgb = lerp(top.rgb, black, clamp(i.pos.y - _y2, 0, 1));
				return bottom;
			}
			ENDCG
		}
	}
}