Shader "SPM/Skybox Master" {
Properties {
    _Tint ("Tint Color", Color) = (.5, .5, .5, .5)
    [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
    _Rotation ("Rotation", Range(0, 360)) = 0
    [NoScaleOffset] _Tex1 ("Night", Cube) = "grey" {}
	[NoScaleOffset] _Tex2 ("SunRise", Cube) = "grey" {}
	[NoScaleOffset] _Tex3 ("Day", Cube) = "grey" {}
	[NoScaleOffset] _Tex4 ("SunSet", Cube) = "grey" {}
	_NtoSR ("Night to Sunrise", Range(0,1)) = 0
	_SRtoD ("Sunrise to Day", Range(0,1)) = 0
	_DtoSS ("Day to Sunset", Range(0,1)) = 0
	_SStoN ("Sunset to Night", Range(0,1)) = 0
}

	SubShader {
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
		Cull Off ZWrite Off

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			samplerCUBE _Tex1;
			samplerCUBE _Tex2;
			samplerCUBE _Tex3;
			samplerCUBE _Tex4;
			half4 _Tint;
			half _Exposure;
			half _Rotation;
			half _NtoSR;
			half _SRtoD;
			half _DtoSS;
			half _SStoN;

			float3 RotateAroundYInDegrees (float3 vertex, float degrees)
			{
				half alpha = degrees * UNITY_PI / 180.0;
				half sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float3(mul(m, vertex.xz), vertex.y).xzy;
			}

			struct appdata_t {
				half4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				half4 vertex : SV_POSITION;
				half3 texcoord : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				half3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
				o.vertex = UnityObjectToClipPos(rotated);
				o.texcoord = v.vertex.xyz;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half4 tex1 = texCUBE (_Tex1, i.texcoord);
				half4 tex2 = texCUBE (_Tex2, i.texcoord);
				half4 tex3 = texCUBE (_Tex3, i.texcoord);
				half4 tex4 = texCUBE (_Tex4, i.texcoord);
				half4 c = lerp(lerp(lerp(lerp(tex1,tex2,saturate(_NtoSR)),tex3,saturate(_SRtoD)),tex4,saturate(_DtoSS)),tex1,saturate(_SStoN));
				c = c * _Tint * unity_ColorSpaceDouble;
				c *= _Exposure;
				return c;
			}
			ENDCG
		}
	}
}