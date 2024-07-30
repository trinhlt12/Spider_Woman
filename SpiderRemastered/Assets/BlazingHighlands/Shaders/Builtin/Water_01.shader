// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/DragonsNotAllowed/BlazingHighlands/Water_01"
{
	Properties
	{
		_WaterColor("Water Color", Color) = (0.2784314,0.3215686,0.4352941,1)
		_DepthColor("Depth Color", Color) = (0.1647059,0.2313726,0.3098039,1)
		_Normal("Normal", 2D) = "bump" {}
		_NormalIntensity("Normal Intensity", Float) = 1
		_WaterScale("Water Scale", Float) = 3900
		_Wave1Scale("Wave 1 Scale", Float) = 0.7
		_Wave2Scale("Wave 2 Scale", Float) = 1
		_WaveSpeed("Wave Speed", Float) = 0.02
		_Smoothness("Smoothness", Float) = 0.99
		_Distance("Distance", Float) = 2.5
		_Opacity("Opacity", Range( 0 , 1)) = 0.9
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma surface surf Standard alpha:fade
		struct Input
		{
			float3 worldPos;
			float4 screenPos;
		};

		uniform sampler2D _Normal;
		uniform float _WaveSpeed;
		uniform float _Wave1Scale;
		uniform float _WaterScale;
		uniform float _Wave2Scale;
		uniform float _NormalIntensity;
		uniform float4 _WaterColor;
		uniform float4 _DepthColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Distance;
		uniform float _Smoothness;
		uniform float _Opacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_81_0 = ( (ase_worldPos).xz / _WaterScale );
			float2 panner85 = ( 1.0 * _Time.y * ( float2( 0,1 ) * _WaveSpeed ) + ( _Wave1Scale * temp_output_81_0 ));
			float2 panner92 = ( 1.0 * _Time.y * ( float2( 1,0 ) * _WaveSpeed ) + ( _Wave2Scale * temp_output_81_0 ));
			float3 temp_output_100_0 = ( UnpackNormal( tex2D( _Normal, panner85 ) ) + UnpackNormal( tex2D( _Normal, panner92 ) ) );
			float3 appendResult112 = (float3(( (temp_output_100_0).xy * _NormalIntensity ) , (temp_output_100_0).z));
			o.Normal = appendResult112;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth70 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth70 = abs( ( screenDepth70 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _Distance ) );
			float clampResult124 = clamp( distanceDepth70 , 0.0 , 1.0 );
			float4 lerpResult123 = lerp( _WaterColor , _DepthColor , clampResult124);
			o.Albedo = lerpResult123.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = ( clampResult124 * _Opacity );
		}

		ENDCG
	}
}