Shader "Electric Shader" {
	Properties {
		[NoScaleOffset] _MainTexaaa ("Texture", 2D) = "white" {}
		[NoScaleOffset] _Mask ("Mask", 2D) = "white" {}
		[HideInInspector] _UnscaledTime ("UnScale Time", Float) = 0
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