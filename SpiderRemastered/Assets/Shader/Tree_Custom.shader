Shader "Tree/Custom" {
	Properties {
		[NoScaleOffset] _ColorMask ("Color Mask", 2D) = "white" {}
		_Cutoff ("Mask Clip Value", Float) = 0
		_LeavesBaseColor ("Leaves Base Color", Vector) = (0.3254902,0.4352941,0.1215686,1)
		_LeavesDetailColor ("Leaves Detail Color", Vector) = (0.4705882,0.5647059,0.2666667,1)
		_BranchBaseColor ("Branch Base Color", Vector) = (0.2862745,0.282353,0.2470588,1)
		_BranchDetailColor ("Branch Detail Color", Vector) = (0.4627451,0.4470588,0.4078431,1)
		[NoScaleOffset] _WindNoiseGray ("Wind Noise Gray", 2D) = "white" {}
		_WindIntensity ("Wind Intensity", Float) = 1
		_WindTimeScale ("Wind Time Scale", Float) = 1.5
		_WindWaveScale ("Wind Wave Scale", Float) = 1
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