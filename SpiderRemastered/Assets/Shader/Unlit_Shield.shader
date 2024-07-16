Shader "Unlit/Shield" {
	Properties {
		_Texture ("Texture", 2D) = "white" {}
		_Speed ("Speed", Float) = 0
		_Fresnel ("Fresnel", Float) = 0
		_Mask ("Mask", 2D) = "white" {}
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