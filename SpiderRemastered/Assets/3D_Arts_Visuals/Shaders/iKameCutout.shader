Shader "iGame/iKameCutout"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200
			Cull Off

            CGPROGRAM
            #pragma surface surf Lambert alphatest:_Cutoff addshadow

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
            };


            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
}
