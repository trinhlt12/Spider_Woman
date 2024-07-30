Shader "Tree/Custom"
{
    Properties
    {
        [NoScaleOffset]_ColorMask("Color Mask", 2D) = "white" {}
        _Cutoff("Mask Clip Value", Float) = 0
        _LeavesBaseColor("Leaves Base Color", Color) = (0.3254902, 0.4352941, 0.1215686, 1)
        _LeavesDetailColor("Leaves Detail Color", Color) = (0.4705882, 0.5647059, 0.2666667, 1)
        _BranchBaseColor("Branch Base Color", Color) = (0.2862745, 0.282353, 0.2470588, 1)
        _BranchDetailColor("Branch Detail Color", Color) = (0.4627451, 0.4470588, 0.4078431, 1)
        [NoScaleOffset]_WindNoiseGray("Wind Noise Gray", 2D) = "white" {}
        _WindIntensity("Wind Intensity", Float) = 1
        _WindTimeScale("Wind Time Scale", Float) = 1.5
        _WindWaveScale("Wind Wave Scale", Float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
			#pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                fixed4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                fixed2 uv : TEXCOORD0;
				fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                fixed4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _ColorMask;
			fixed _Cutoff;
			fixed4 _LeavesBaseColor;
			fixed4 _LeavesDetailColor;
			fixed4 _BranchBaseColor;
			fixed4 _BranchDetailColor;
			sampler2D _WindNoiseGray;
			fixed _WindIntensity;
			fixed _WindTimeScale;
			fixed _WindWaveScale;
fixed _BrightTree;

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
				fixed2 aaa = fixed2(_Time.y*_WindTimeScale*0.1,_Time.y*_WindTimeScale*0.1)+(v.uv*fixed2(_WindWaveScale,_WindWaveScale));
				float4 uvnoise = float4(aaa.xy,0,0);
				float noise = tex2Dlod(_WindNoiseGray,uvnoise).r*_WindIntensity;
                o.vertex = UnityObjectToClipPos(lerp(v.vertex,v.vertex+fixed4(noise,noise,noise,1),v.color.r));
                o.uv = v.uv;
				o.color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_ColorMask, i.uv);
				fixed4 final = lerp(col,lerp(lerp(lerp(_LeavesBaseColor,_LeavesDetailColor,col.r),_BranchBaseColor,col.g),_BranchDetailColor,col.b)*(i.color.r+0.6),i.color.g);
				clip(col.a-_Cutoff);
                UNITY_APPLY_FOG(i.fogCoord, final);
    return final;
}
            ENDCG
        }
    }
}
