Shader "Custom/DragonsNotAllowed/BlazingHighlands/WaterLily_01" {
	Properties {
		_ColorMask ("Color Mask", 2D) = "white" {}
		_Normal ("Normal", 2D) = "bump" {}
		_Cutoff ("Mask Clip Value", Float) = 0.5
		_ColorA ("Color A", Vector) = (0.1137255,0.227451,0.1176471,1)
		_ColorB ("Color B", Vector) = (0.3333333,0.3529412,0.2117647,1)
		_Specular ("Specular", Float) = 0.1
		_Smoothness ("Smoothness", Float) = 0.1
		_WindNoiseGray16 ("Wind Noise Gray16", 2D) = "white" {}
		_WindIntensity ("Wind Intensity", Float) = 1
		_WindWaveScale ("Wind Wave Scale", Float) = 2000
		_WindTimeScale ("Wind Time Scale", Float) = 1.2
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}