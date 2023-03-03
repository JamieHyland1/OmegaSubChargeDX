Shader "Unlit/DitherTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col1("Color 1", Color) = (1,1,1,1)
        _Col2("Color 2", Color) = (1,1,1,1)
        _NumCol("Shades",Range(1,256)) = 1
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

            static const float4x4 dither_table = float4x4
            (
                0,    8,    2,    10,
                12,    4,    14,    6, 
                3,    11,    1,    9, 
                15,    7,    13,    5
            );
            float3 DitherCrunch(float3 col, int2 p){
              col*=255.0; //extrapolate 16bit color float to 16bit integer space
             
                int dither = dither_table[p.x % 4][p.y % 4];
                col += (dither / 2.0 - 4.0); //dithering process as described in PSYDEV SDK documentation
              
              col = lerp((uint3(col) & 0xf8), 0xf8, step(0xf8,col)); 
              //truncate to 5bpc precision via bitwise AND operator, and limit value max to prevent wrapping.
              //PS1 colors in default color mode have a maximum integer value of 248 (0xf8)
              col /= 255; //bring color back to floating point number space
              return col;
            }

            float4 quantize_color(float4 col, int _NumCol)
            {
                float grey= max(col.r,max(col.g,col.b));
                float lower = floor(grey * _NumCol)/_NumCol;
                float lowDiff = abs(grey-lower);
                float upper     = ceil(grey * _NumCol) / _NumCol;
                float upperDiff = abs(upper - grey);
                float level = lowDiff <= upperDiff ? lower:upper;
                float adjustment = level/grey;
                col.rgb *= adjustment;
                return col;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Col1;
            float4 _Col2;
            int _NumCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(_Col1,_Col2,i.uv.y);
               
                col.rgb = DitherCrunch(col.rgb,i.screenPos*_NumCol);
                
                return col;
            }
            ENDCG
        }
    }
}
