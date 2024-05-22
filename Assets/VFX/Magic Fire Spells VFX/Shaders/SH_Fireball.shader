// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Fireball"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Offset("Offset", Range( 0 , 5)) = 1
		_2("2", 2D) = "white" {}
		_Color("Color", Vector) = (1.2,0.5,0.2,0)
		_Untitled3("Untitled-3", 2D) = "white" {}
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
		};

		uniform float3 _Color;
		uniform sampler2D _Untitled3;
		uniform float _Offset;
		uniform sampler2D _2;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1.05,1.2 ) + float2( -0.15,-0.9 );
			o.texcoord_1.xy = v.texcoord.xy * float2( 0.75,0.75 ) + float2( 0,0 );
		}

		inline fixed4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return fixed4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 componentMask7 = tex2D( _2, (abs( i.texcoord_1+_Time[1] * float2(-1.5,0 ))) ).xy;
			float4 tex2DNode38 = tex2D( _Untitled3, ( ( float2( -0.1,-0.3 ) + i.texcoord_0 ) + ( _Offset * componentMask7 ) ) );
			o.Emission = ( ( i.vertexColor * float4( _Color , 0.0 ) ) * tex2DNode38 ).rgb;
			o.Alpha = ( i.vertexColor.a * tex2DNode38.r );
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
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
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
Version=12001
127;265;1565;808;1579.342;322.9429;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3012.141,752.2548;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.75,0.75;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;2;-2733.944,735.5549;Float;False;-1.5;0;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;6;-2518.445,708.5549;Float;True;Property;_2;2;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;7;-2200.142,707.5546;Float;True;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;9;-2391.141,472.6548;Float;False;Property;_Offset;Offset;0;0;1;0;5;0;1;FLOAT
Node;AmplifyShaderEditor.Vector2Node;17;-1946.541,81.15272;Float;False;Constant;_Vector0;Vector 0;2;0;-0.1,-0.3;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1990.34,277.552;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.05,1.2;False;1;FLOAT2;-0.15,-0.9;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1895.942,613.9545;Float;True;2;2;0;FLOAT;0,0;False;1;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1680.741,105.952;Float;True;2;2;0;FLOAT2;0.0;False;1;FLOAT2;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-1181.045,582.8572;Float;True;2;2;0;FLOAT2;0.0;False;1;FLOAT2;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.Vector3Node;32;-1057.035,-119.0452;Float;False;Property;_Color;Color;2;0;1.2,0.5,0.2;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.VertexColorNode;33;-1329.437,-133.1461;Float;False;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-720.2352,-329.9458;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;38;-886.7455,325.858;Float;True;Property;_Untitled3;Untitled-3;3;0;Assets/VFX/Textures/Untitled-3.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-305.6563,264.9574;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-472.5424,-70.74225;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;Unlit;FX_Shaders/SH_WeaponShell_01;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Off;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;1;0
WireConnection;6;1;2;0
WireConnection;7;0;6;0
WireConnection;8;0;9;0
WireConnection;8;1;7;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;39;0;16;0
WireConnection;39;1;8;0
WireConnection;34;0;33;0
WireConnection;34;1;32;0
WireConnection;38;1;39;0
WireConnection;37;0;33;4
WireConnection;37;1;38;1
WireConnection;40;0;34;0
WireConnection;40;1;38;0
WireConnection;0;2;40;0
WireConnection;0;9;37;0
ASEEND*/
//CHKSM=750613189AC1A9B7E343F0C89B681F65689E5E63