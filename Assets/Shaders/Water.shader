Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("Texture", 2D) = "white" {}
        _Distortion("Distortion Texture",2D) = "white" {}
        _Col1("Shallow water color", Color) = (1,1,1,1)
        _Col2("Deep water color", Color) = (1,1,1,1)
        _Col3("Caustics 1 water color", Color) = (1,1,1,1)
        _Col4("Caustics 2 water color", Color) = (1,1,1,1)
        _MaxDepth("Max water Depth", Range(0,335)) = 0.5
        _CutOff("Caustics cutoff",float) = 0.8
        _SkyboxColor("Skybox color", Color) = (1,1,1,1)
        _ReflectionTex ("Reflection Texture", Cube) = "white" {}
        _ReflectionInt ("Reflection Intensity", Range(0, 1)) = 1
        _ReflectionMet ("Reflection Metallic", Range(0, 1)) = 0
        _ReflectionDet ("Reflection Detail", Range(1, 9)) = 1
        _ReflectionExp ("Reflection Exposure", Range(1, 3)) = 1

    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma shader_feature Skybox
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv  : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD1;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float2 uv3 : TEXCOORD5;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPosition : TEXCOORD2;
                float3 normal_world : TEXCOORD3;
                float3 vertex_world : TEXCOORD4;
                
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Distortion;
            float4 _Distortion_ST;
            sampler2D _MainTex2;
            sampler2D _CameraDepthTexture;
            samplerCUBE _ReflectionTex;
            float4 _SkyboxColor;
            float4 _MainTex2_ST;
            float4 _Col1;
            float4 _Col2;
            float4 _Col3;
            float4 _Col4;
            float _MaxDepth;
            float _Cutoff;
            float _ReflectionInt;
            half _ReflectionDet;
            float _ReflectionExp;
            float _ReflectionMet;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex2);
                o.uv3 = TRANSFORM_TEX(v.uv3, _Distortion);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.normal_world = normalize(mul(unity_ObjectToWorld,
                float4(v.normal, 0))).xyz;
                o.vertex_world = mul(unity_ObjectToWorld, v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float3 ambientReflection(samplerCUBE colorRefl,
                float reflectionInt,half reflectionDot,float3 normal, float3 viewDir,float reflectionExp)
            {
                float3 reflection_world = reflect(viewDir,normal);
                float4 cubemap = texCUBElod(colorRefl,float4(reflection_world,reflectionDot));
                return reflectionInt * cubemap.rbg * (cubemap.a * reflectionExp);
            }

            fixed4 frag (v2f i) : SV_Target
            {
              
                // sample the texture
                i.uv.x -= sin(_Time.x - i.uv.y) * _Cutoff * 5;
                i.uv.y -= cos(_Time.x - i.uv.x) * _Cutoff * 5;
                float2 duv = tex2D(_Distortion, i.uv3 * sin(_Time.x) * UNITY_TWO_PI);

                i.uv -= duv;
                fixed4 caustics  = tex2D(_MainTex, i.uv);
                i.uv2 -= duv;
                fixed4 caustics2  = tex2D(_MainTex2, float2(i.uv2.y,i.uv2.x));
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                float depthDifference = existingDepthLinear - i.screenPosition.w;
                float waterDepthDifference01 = saturate(depthDifference / _MaxDepth);
                float4 waterColor = lerp(_Col1, (_Col2), waterDepthDifference01);
                float surfaceNoise = (caustics *  _SkyboxColor);
                 half3 normal = i.normal_world;
                half3 viewDir = normalize(UnityWorldSpaceViewDir(i.vertex_world));

                half3 reflection = ambientReflection(_ReflectionTex,_ReflectionInt,_ReflectionDet,normal,-viewDir,_ReflectionExp);
                waterColor.rgb += (reflection + _ReflectionMet) ;
                waterColor = saturate(waterColor + surfaceNoise + (caustics2 * _Col4));
               
                return  waterColor;
            }
            ENDCG
        }
    }
}
