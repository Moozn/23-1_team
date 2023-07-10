// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MossyRuins"
{
	Properties
	{
		[NoScaleOffset]_MainTex("Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_BumpMap("Normal", 2D) = "bump" {}
		[NoScaleOffset]_MetallicGlossMap("Metallic (R)", 2D) = "white" {}
		_Glossiness("Smoothness (A)", Range( 0 , 1)) = 0.7
		[NoScaleOffset]_OcclusionMap("AmbientOcclusion (G)", 2D) = "white" {}
		_AOStrength("AO Strength", Range( 0 , 5)) = 1
		[Header(Moss Properties)]_MossColor("Moss Color", Color) = (0.5803922,0.7098039,0.3647059,1)
		_MossAmount("Moss Amount", Range( 0 , 2)) = 2
		_MossMaskScale("Moss Mask Scale", Range( 0 , 2)) = 1.07
		_MossMaskPower("Moss Mask Power", Range( 0 , 2)) = 0
		_Offset("Offset", Vector) = (0,0,0,0)
		_Scale("Scale", Vector) = (3,3,0,0)
		[NoScaleOffset]_MossAlbedo("Moss Albedo", 2D) = "white" {}
		[NoScaleOffset][Normal]_MossNormal("Moss Normal", 2D) = "bump" {}
		[NoScaleOffset]_MossSmoothnessA("Moss Smoothness (A)", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZTest LEqual
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _BumpMap;
		uniform sampler2D _MossNormal;
		uniform float2 _Scale;
		uniform float2 _Offset;
		uniform float _MossMaskScale;
		uniform float _MossMaskPower;
		uniform float _MossAmount;
		uniform sampler2D _MainTex;
		uniform float4 _MossColor;
		uniform sampler2D _MossAlbedo;
		uniform sampler2D _MetallicGlossMap;
		uniform sampler2D _MossSmoothnessA;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float _AOStrength;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap4 = i.uv_texcoord;
			float3 tex2DNode4 = UnpackNormal( tex2D( _BumpMap, uv_BumpMap4 ) );
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_59_0 = ((ase_worldPos).xz*_Scale + _Offset);
			float simplePerlin2D52 = snoise( (ase_worldPos).xz*_MossMaskScale );
			simplePerlin2D52 = simplePerlin2D52*0.5 + 0.5;
			float temp_output_18_0 = saturate( ( (WorldNormalVector( i , tex2DNode4 )).y * saturate( ( pow( simplePerlin2D52 , _MossMaskPower ) * _MossAmount ) ) ) );
			float3 lerpResult15 = lerp( tex2DNode4 , BlendNormals( tex2DNode4 , UnpackNormal( tex2D( _MossNormal, temp_output_59_0 ) ) ) , temp_output_18_0);
			o.Normal = lerpResult15;
			float2 uv_MainTex1 = i.uv_texcoord;
			float4 blendOpSrc26 = _MossColor;
			float4 blendOpDest26 = tex2D( _MossAlbedo, temp_output_59_0 );
			float4 lerpBlendMode26 = lerp(blendOpDest26,(( blendOpDest26 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest26 ) * ( 1.0 - blendOpSrc26 ) ) : ( 2.0 * blendOpDest26 * blendOpSrc26 ) ),_MossColor.a);
			float4 lerpResult10 = lerp( tex2D( _MainTex, uv_MainTex1 ) , ( saturate( lerpBlendMode26 )) , temp_output_18_0);
			o.Albedo = lerpResult10.rgb;
			float2 uv_MetallicGlossMap2 = i.uv_texcoord;
			float4 tex2DNode2 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap2 );
			float4 tex2DNode16 = tex2D( _MossSmoothnessA, temp_output_59_0 );
			float lerpResult112 = lerp( tex2DNode2.r , tex2DNode16.r , temp_output_18_0);
			o.Metallic = lerpResult112;
			float lerpResult109 = lerp( ( _Glossiness * tex2DNode2.a ) , tex2DNode16.a , temp_output_18_0);
			o.Smoothness = lerpResult109;
			float2 uv_OcclusionMap23 = i.uv_texcoord;
			o.Occlusion = pow( tex2D( _OcclusionMap, uv_OcclusionMap23 ).g , _AOStrength );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma exclude_renderers xboxseries playstation switch nomrt 
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=18930
288;128;886;655;1014.191;1524.108;1.431109;True;False
Node;AmplifyShaderEditor.CommentaryNode;58;-2750.419,-209.2723;Inherit;False;1308.895;420.5837;Moss Ammount;9;12;21;52;50;54;57;55;56;22;;0.3859071,1,0.3160377,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;55;-2700.419,-19.58124;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;56;-2509.854,-24.31502;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2591.284,-110.9078;Inherit;False;Property;_MossMaskScale;Moss Mask Scale;8;0;Create;True;0;0;0;False;0;False;1.07;1.07;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2274.316,-26.72094;Inherit;False;Property;_MossMaskPower;Moss Mask Power;9;0;Create;True;0;0;0;False;0;False;0;-2.75;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;52;-2256.337,-137.1115;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;50;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;54;-2032.837,-159.2723;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;48;-1418.595,-570.0618;Inherit;False;662.2186;330.0691;World Position UV;5;36;60;61;59;37;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-2065.247,95.31134;Float;False;Property;_MossAmount;Moss Amount;7;0;Create;True;0;0;0;False;0;False;2;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-1389.726,-222.435;Inherit;False;673.9896;289;Moss Mask (from Height);3;18;19;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;36;-1384.196,-527.028;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;4;-1698.714,227.0588;Inherit;True;Property;_BumpMap;Normal;1;2;[NoScaleOffset];[Normal];Create;False;0;0;0;False;0;False;-1;None;f51225d2417ae9340a9865129556b925;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1747.246,-42.15965;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;-1606.524,-33.78569;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;61;-1218.974,-434.2384;Float;False;Property;_Scale;Scale;11;0;Create;True;0;0;0;False;0;False;3,3;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;60;-1080.679,-371.0251;Float;False;Property;_Offset;Offset;10;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ComponentMaskNode;37;-1178.031,-520.0619;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;19;-1276.726,-175.4351;Inherit;True;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;39;-606.7402,-851.5814;Inherit;False;850.701;527.6512;;6;2;16;109;110;111;41;Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;59;-972.6052,-521.419;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;43;-316.7236,105.4241;Inherit;False;658.9256;496.9547;Normal;4;15;47;14;81;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;38;-636.3472,-1423.786;Inherit;False;1073.173;533.6521;Albedo;5;10;26;1;24;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-1043.832,-50.74417;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-256.9099,-763.9251;Inherit;False;Property;_Glossiness;Smoothness (A);3;0;Create;False;0;0;0;False;0;False;0.7;0.7;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-464.2156,-1309.75;Inherit;False;Property;_MossColor;Moss Color;6;1;[Header];Create;True;1;Moss Properties;0;0;False;0;False;0.5803922,0.7098039,0.3647059,1;0.5773126,0.7075471,0.363786,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;18;-882.7365,-126.5153;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;114;-165.239,656.1701;Inherit;False;663.5433;434.5272;Occlusion;3;23;90;92;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;2;-551.2078,-783.0671;Inherit;True;Property;_MetallicGlossMap;Metallic (R);2;1;[NoScaleOffset];Create;False;0;0;0;False;0;False;-1;None;2dbcae5191d939a4e807bf5f0ecd0d84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-280.5873,292.8396;Inherit;True;Property;_MossNormal;Moss Normal;13;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;-1;None;20947c24b67c80545896c235fc54b475;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-539.4125,-1108.595;Inherit;True;Property;_MossAlbedo;Moss Albedo;12;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;9b26210a36a8d384f80a695548c137e4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;113;-84.30206,-285.8954;Inherit;False;315;304;Metallic;1;112;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;23;-115.239,706.1701;Inherit;True;Property;_OcclusionMap;AmbientOcclusion (G);4;1;[NoScaleOffset];Create;False;0;0;0;False;0;False;-1;None;2dbcae5191d939a4e807bf5f0ecd0d84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;41;-235.255,-527.6562;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-159.7618,-1375.164;Inherit;True;Property;_MainTex;Albedo;0;1;[NoScaleOffset];Create;False;0;0;0;False;0;False;-1;None;dfc73faefe6c372469be5b4329673d7e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;81;39.77948,331.2585;Inherit;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;16;-571.2563,-550.2592;Inherit;True;Property;_MossSmoothnessA;Moss Smoothness (A);14;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;bafbb9fd6c9a5c144acfc5c3dce0bb9d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;26;-89.53929,-1132.291;Inherit;False;Overlay;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-104.6651,974.6973;Inherit;False;Property;_AOStrength;AO Strength;5;0;Create;True;0;0;0;False;0;False;1;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;47;-302.776,147.7088;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;27.08092,-696.5927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;109;-9.604736,-561.2874;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;10;283.4652,-1036.554;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;15;174.3947,168.4828;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;92;238.3041,830.101;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;112;-34.30207,-235.8955;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;731.937,-116.9672;Float;False;True;-1;2;;0;0;Standard;MossyRuins;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;d3d9;d3d11_9x;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;ps4;psp2;n3ds;wiiu;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;56;0;55;0
WireConnection;52;0;56;0
WireConnection;52;1;57;0
WireConnection;54;0;52;0
WireConnection;54;1;50;0
WireConnection;21;0;54;0
WireConnection;21;1;12;0
WireConnection;22;0;21;0
WireConnection;37;0;36;0
WireConnection;19;0;4;0
WireConnection;59;0;37;0
WireConnection;59;1;61;0
WireConnection;59;2;60;0
WireConnection;11;0;19;2
WireConnection;11;1;22;0
WireConnection;18;0;11;0
WireConnection;14;1;59;0
WireConnection;9;1;59;0
WireConnection;41;0;18;0
WireConnection;81;0;4;0
WireConnection;81;1;14;0
WireConnection;16;1;59;0
WireConnection;26;0;24;0
WireConnection;26;1;9;0
WireConnection;26;2;24;4
WireConnection;47;0;18;0
WireConnection;111;0;110;0
WireConnection;111;1;2;4
WireConnection;109;0;111;0
WireConnection;109;1;16;4
WireConnection;109;2;18;0
WireConnection;10;0;1;0
WireConnection;10;1;26;0
WireConnection;10;2;41;0
WireConnection;15;0;4;0
WireConnection;15;1;81;0
WireConnection;15;2;47;0
WireConnection;92;0;23;2
WireConnection;92;1;90;0
WireConnection;112;0;2;1
WireConnection;112;1;16;1
WireConnection;112;2;18;0
WireConnection;0;0;10;0
WireConnection;0;1;15;0
WireConnection;0;3;112;0
WireConnection;0;4;109;0
WireConnection;0;5;92;0
ASEEND*/
//CHKSM=AEF2992EC25EA8EB9DAF38FEA3C8819F2D5B433A