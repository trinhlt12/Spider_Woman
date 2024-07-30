// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/DragonsNotAllowed/BlazingHighlands/WaterLily_01"
{
	Properties
	{
		_ColorMask("Color Mask", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_ColorA("Color A", Color) = (0.1137255,0.227451,0.1176471,1)
		_ColorB("Color B", Color) = (0.3333333,0.3529412,0.2117647,1)
		_Specular("Specular", Float) = 0.1
		_Smoothness("Smoothness", Float) = 0.1
		_WindNoiseGray16("Wind Noise Gray16", 2D) = "white" {}
		_WindIntensity("Wind Intensity", Float) = 1
		_WindWaveScale("Wind Wave Scale", Float) = 2000
		_WindTimeScale("Wind Time Scale", Float) = 1.2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _WindNoiseGray16;
		uniform float _WindTimeScale;
		uniform float _WindWaveScale;
		uniform float _WindIntensity;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform float4 _ColorA;
		uniform float4 _ColorB;
		uniform sampler2D _ColorMask;
		uniform float4 _ColorMask_ST;
		uniform float _Specular;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float mulTime6 = _Time.y * _WindTimeScale;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 worldToObj2 = mul( unity_WorldToObject, float4( ase_worldPos, 1 ) ).xyz;
			float2 panner5 = ( mulTime6 * float2( 0.038095,0.076189 ) + ( (worldToObj2).xz / _WindWaveScale ));
			float3 appendResult22 = (float3(0.0 , ( ( tex2Dlod( _WindNoiseGray16, float4( panner5, 0, 0.0) ).r + -0.5 ) * _WindIntensity ) , 0.0));
			v.vertex.xyz += appendResult22;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float2 uv_ColorMask = i.uv_texcoord * _ColorMask_ST.xy + _ColorMask_ST.zw;
			float4 tex2DNode27 = tex2D( _ColorMask, uv_ColorMask );
			float4 lerpResult31 = lerp( _ColorA , _ColorB , tex2DNode27.r);
			o.Albedo = lerpResult31.rgb;
			float3 temp_cast_1 = (_Specular).xxx;
			o.Specular = temp_cast_1;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
			clip( tex2DNode27.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.CommentaryNode;1;-1896.995,130.0824;Inherit;False;1938.759;392.2166;Wind;13;15;14;7;6;5;4;3;2;20;16;10;9;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;402.6842,-133.3623;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;Custom/DragonsNotAllowed/BlazingHighlands/WaterLily_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-112.2501,286.5139;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-452.8871,266.5845;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-302.7339,310.2342;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-826.5353,236.903;Inherit;True;Property;_WindNoiseGray16;Wind Noise Gray16;7;0;Create;True;0;0;0;False;0;False;-1;3b89873185f2c8142872c0b00a7a43dc;3b89873185f2c8142872c0b00a7a43dc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-505.2697,380.0738;Inherit;False;Property;_WindIntensity;Wind Intensity;8;0;Create;True;0;0;0;False;0;False;1;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;2;-1630.19,182.9514;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;3;-1189.19,222.9513;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1396.19,292.9513;Inherit;False;Property;_WindWaveScale;Wind Wave Scale;9;0;Create;True;0;0;0;False;0;False;2000;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;5;-1044.19,264.9513;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.038095,0.076189;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1237.19,387.9516;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1432.19,382.9516;Inherit;False;Property;_WindTimeScale;Wind Time Scale;10;0;Create;True;0;0;0;False;0;False;1.2;1.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;14;-1841.255,188.1282;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;15;-1397.19,182.9514;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;30;-496.7058,-310.0063;Inherit;False;Property;_ColorB;Color B;4;0;Create;True;0;0;0;False;0;False;0.3333333,0.3529412,0.2117647,1;0.3333333,0.3529412,0.2117647,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-496.6576,-484.0096;Inherit;False;Property;_ColorA;Color A;3;0;Create;True;0;0;0;False;0;False;0.1137255,0.227451,0.1176471,1;0.1137254,0.2274509,0.117647,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;31;-198.8825,-405.4464;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;164.7103,6.181847;Inherit;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;0;False;0;False;0.1;0.75;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;173.8944,-71.52814;Inherit;False;Property;_Specular;Specular;5;0;Create;True;0;0;0;False;0;False;0.1;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;191.4872,-370.9722;Inherit;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;0;False;0;False;0.3979397,0.4622642,0.2907322,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-171.5408,-169.1824;Inherit;True;Property;_Normal;Normal;1;0;Create;True;0;0;0;False;0;False;-1;8ceada8e196f50446ba9566b341769d4;8ceada8e196f50446ba9566b341769d4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-542.8981,-74.40855;Inherit;True;Property;_ColorMask;Color Mask;0;0;Create;True;0;0;0;False;0;False;-1;8bd3d528f554c4f45a16d23af33a35a1;8bd3d528f554c4f45a16d23af33a35a1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;0;0;31;0
WireConnection;0;1;26;0
WireConnection;0;3;23;0
WireConnection;0;4;24;0
WireConnection;0;10;27;4
WireConnection;0;11;22;0
WireConnection;22;1;10;0
WireConnection;9;0;16;1
WireConnection;10;0;9;0
WireConnection;10;1;20;0
WireConnection;16;1;5;0
WireConnection;2;0;14;0
WireConnection;3;0;15;0
WireConnection;3;1;4;0
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;6;0;7;0
WireConnection;15;0;2;0
WireConnection;31;0;29;0
WireConnection;31;1;30;0
WireConnection;31;2;27;1
ASEEND*/
//CHKSM=21542A4E3E9121C880D60A3C3E19FA7F240FCEC8