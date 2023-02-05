Shader "Unlit/Shadow_map"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
     

        Pass
        {


            Name "Shadow Caster"
            Tags{
            "RenderType"="Opaque"
            "LightMode"="ShadowCaster"
            }


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #include "UnityCG.cginc"
            #pragma multi_compile_shadowcaster

           struct v2f{
            
            V2F_SHADOW_CASTER;
            
            };

            v2f vert (appdata_full v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target{
            
                SHADOW_CASTER_FRAGMENT(i)
            
            }

            ENDCG
        }

        pass{

            Name "Shadow Map Texture"
            Tags{
            "RenderType"="Opaque"
            "LightMode"="ForwardBase"
            }


             CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
            float2 uv : TEXCOORD0;
            UNITY_FOG_COORDS(1)
       
           
            float4 vertex : SV_POSITION;
            // declare the UV coordinates for the shadow map
            float4 shadowCoord : TEXCOORD1;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            // declare a sampler for the shadow map
            sampler2D _ShadowMapTexture;


           float4 NDCToUV(float4 clipPos)
            {
            float4 o = clipPos * 0.5;
            #if defined(UNITY_HALF_TEXEL_OFFSET )
            o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w * _ScreenParams.zw;
           
            #else
            o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
            #endif
            o.zw = clipPos.zw;
            return o;
            }



            v2f vert (appdata v)
            {
              v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shadowCoord = NDCToUV(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
