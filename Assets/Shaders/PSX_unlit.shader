Shader "Unlit/PSX_unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VPos ("Vertex Position", Vector) = (0, 0, 0, 1)
        _Color("Color", Color) = (1,1,1,1)
        _ColorStep("step", int) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
                float2 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _VPos;
            float4 _Color;
            int _ColorStep;

            v2f vert (appdata v)
            {
                v2f o;
              
               
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeGrabScreenPos(o.vertex);
               
                // o.vertex.x /= o.vertex.w;
                // o.vertex.y /= o.vertex.w;
                //
                //  o.vertex.x  = (floor(o.vertex.x * _VPos.x)/_VPos.x)*_VPos.w;
                //  o.vertex.y  = (floor(o.vertex.y * _VPos.y)/_VPos.y)*_VPos.w;
                //
                // o.vertex.x *= o.vertex.w;
                // o.vertex.y *= o.vertex.w;


                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
             const float4x4 psx_dither_table = float4x4
            (
                0,    8,    2,    10,
                12,    4,    14,    6, 
                3,    11,    1,     9, 
                15,    7,    13,    5
            );

              float3 Dither(float3 col, float2 p)
            {
                col *= 250.0;
                int dither = psx_dither_table[p.x%4][p.y%4];
                col += (dither/2 - 4);
               
                col = lerp((uint3(col) & 0xf8), 0xf8, step(0xf8,col));
                col /= 255;
                return col;
                
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float3 q_col = round(col.rgb/_ColorStep) * _ColorStep;
                col.rgb = Dither(col.rgb,floor(i.uv * 256));
              //  col.rgb = q_col;
                return col;
            }
            ENDCG
        }
    }
}
