Shader "Custom/DragonsNotAllowed/BlazingHighlands/Water_01" {
	Properties {
		_WaterColor ("Water Color", Vector) = (0.2784314,0.3215686,0.4352941,1)
		_DepthColor ("Depth Color", Vector) = (0.1647059,0.2313726,0.3098039,1)
		_Normal ("Normal", 2D) = "bump" {}
		_NormalIntensity ("Normal Intensity", Float) = 1
		_WaterScale ("Water Scale", Float) = 3900
		_Wave1Scale ("Wave 1 Scale", Float) = 0.7
		_Wave2Scale ("Wave 2 Scale", Float) = 1
		_WaveSpeed ("Wave Speed", Float) = 0.02
		_Smoothness ("Smoothness", Float) = 0.99
		_Distance ("Distance", Float) = 2.5
		_Opacity ("Opacity", Range(0, 1)) = 0.9
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
}