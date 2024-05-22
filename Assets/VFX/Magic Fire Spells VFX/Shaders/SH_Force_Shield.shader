// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_ForceShield"
{
	Properties
	{
		_T_SmokeSoft_01("T_SmokeSoft_01", 2D) = "white" {}
		_Alpha("Alpha", Range( 0 , 5)) = 3.252886
		_Intensity("Intensity", Range( 0 , 10)) = 3.252886
		_Color("Color", Vector) = (7,7,7,0)
		_Float1("Float 1", Range( 0 , 10)) = 3.252886
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_T_SmokeSoft_04("T_SmokeSoft_04", 2D) = "white" {}
		_T_Ring("T_Ring", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 texcoord_0;
			float2 texcoord_1;
			float2 uv_texcoord;
		};

		uniform sampler2D _T_SmokeSoft_04;
		uniform sampler2D _T_SmokeSoft_01;
		uniform float _Float1;
		uniform sampler2D _TextureSample1;
		uniform float _Intensity;
		uniform float3 _Color;
		uniform sampler2D _T_Ring;
		uniform float4 _T_Ring_ST;
		uniform float _Alpha;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 0.4,0.4 ) + float2( 0,0 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float cos41 = cos( 0.2 * _Time.y );
			float sin41 = sin( 0.2 * _Time.y );
			float2 rotator41 = mul( i.texcoord_1 - float2( 0,0.001 ) , float2x2( cos41 , -sin41 , sin41 , cos41 )) + float2( 0,0.001 );
			float2 panner4 = ( i.texcoord_1 + 1.0 * _Time.y * float2( -0.01,0.01 ));
			float2 lerpResult49 = lerp( rotator41 , panner4 , i.texcoord_1.x);
			float4 temp_output_18_0 = ( float4( i.texcoord_0, 0.0 , 0.0 ) + ( tex2D( _T_SmokeSoft_01, lerpResult49 ) * _Float1 ) );
			float cos50 = cos( 0.01 * _Time.y );
			float sin50 = sin( 0.01 * _Time.y );
			float2 rotator50 = mul( temp_output_18_0.rg - float2( 0,0.0001 ) , float2x2( cos50 , -sin50 , sin50 , cos50 )) + float2( 0,0.0001 );
			float2 panner23 = ( rotator50 + 1.0 * _Time.y * float2( -0.1,0 ));
			float2 panner24 = ( temp_output_18_0.rg + 1.0 * _Time.y * float2( 0,0.1 ));
			float4 temp_output_28_0 = ( ( tex2D( _T_SmokeSoft_04, panner23 ) * tex2D( _TextureSample1, panner24 ) ) * _Intensity );
			o.Emission = ( i.vertexColor * ( temp_output_28_0 * float4( _Color , 0.0 ) ) ).rgb;
			float2 uv_T_Ring = i.uv_texcoord * _T_Ring_ST.xy + _T_Ring_ST.zw;
			o.Alpha = ( tex2D( _T_Ring, uv_T_Ring ) * ( i.vertexColor.a * ( _Alpha * temp_output_28_0 ) ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
278;535;1565;798;2479.744;321.5916;1.740572;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-4104.465,630.0327;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;4;-3757.998,1007.506;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.01,0.01;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RotatorNode;41;-3783.331,369.8116;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0.001;False;2;FLOAT;0.2;False;1;FLOAT2
Node;AmplifyShaderEditor.LerpOp;49;-3316.814,464.3849;Float;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.0,0;False;2;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;2;-3113.801,775.1633;Float;True;Property;_T_SmokeSoft_01;T_SmokeSoft_01;0;0;Assets/VFX/Textures/T_Blobs.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;21;-3123.345,1038.099;Float;False;Property;_Float1;Float 1;4;0;3.252886;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2733.901,765.3002;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2706.784,611.6705;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.4,0.4;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-2277.228,599.53;Float;True;2;2;0;FLOAT2;0,0,0,0;False;1;COLOR;0,0;False;1;COLOR
Node;AmplifyShaderEditor.RotatorNode;50;-2102.65,368.525;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0.0001;False;2;FLOAT;0.01;False;1;FLOAT2
Node;AmplifyShaderEditor.PannerNode;24;-1826.55,810.4495;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.1;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.PannerNode;23;-1922.288,533.7106;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;22;-1722.122,435.9196;Float;True;Property;_T_SmokeSoft_04;T_SmokeSoft_04;6;0;Assets/VFX/Textures/T_SmokeSoft_04.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;25;-1538.047,806.3497;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Assets/VFX/Textures/T_SmokeSoft_04.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1137.898,670.8997;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;15;-1162.9,422.7004;Float;False;Property;_Intensity;Intensity;2;0;3.252886;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;30;-1170.089,279.7238;Float;False;Property;_Alpha;Alpha;1;0;3.252886;0;5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-872.7003,647.4996;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-592.6997,384;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.Vector3Node;11;-947.5352,-146.2162;Float;False;Property;_Color;Color;3;0;7,7,7;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;31;-1653.936,-224.5478;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-658.757,-236.3773;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;45;-412.0533,47.90662;Float;True;Property;_T_Ring;T_Ring;8;0;Assets/VFX/Textures/T_Ring.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-336.5998,275.2;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;27.36592,266.7337;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;39;-809.9324,-641.6494;Float;True;Property;_T_Lightning_Main_01a;T_Lightning_Main_01a;7;0;Assets/VFX/Textures/T_Lightning_Main_01a.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-403.8139,-443.4302;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;381.1835,109.4137;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;FX_Shaders/SH_ForceShield_Idle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;3;0
WireConnection;41;0;3;0
WireConnection;49;0;41;0
WireConnection;49;1;4;0
WireConnection;49;2;3;0
WireConnection;2;1;49;0
WireConnection;20;0;2;0
WireConnection;20;1;21;0
WireConnection;18;0;19;0
WireConnection;18;1;20;0
WireConnection;50;0;18;0
WireConnection;24;0;18;0
WireConnection;23;0;50;0
WireConnection;22;1;23;0
WireConnection;25;1;24;0
WireConnection;26;0;22;0
WireConnection;26;1;25;0
WireConnection;28;0;26;0
WireConnection;28;1;15;0
WireConnection;14;0;30;0
WireConnection;14;1;28;0
WireConnection;13;0;28;0
WireConnection;13;1;11;0
WireConnection;33;0;31;4
WireConnection;33;1;14;0
WireConnection;47;0;45;0
WireConnection;47;1;33;0
WireConnection;32;0;31;0
WireConnection;32;1;13;0
WireConnection;0;2;32;0
WireConnection;0;9;47;0
ASEEND*/
//CHKSM=49C7D6217D028E9986D5CAF1928EB22D0DBED93E