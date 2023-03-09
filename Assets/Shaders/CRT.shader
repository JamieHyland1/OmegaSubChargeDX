Shader "Unlit/CRT"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetteColor ("Vignette Color", Color) = (0, 0, 0, 1)
        _VignetteSize ("Vignette Size", Range(0, 1)) = 0.5
        _Distortion ("Distortion", Vector) = (0,0,0,0)
        _ScanOpacity("Scanline opactiy", float) = 0.5
        _Resolution ("Resolution", Vector) = (0,0,0,0)
        _Spread("Spread", Range(0,1)) = 0.05
        _NumCol("Number of Colors",Range(1,1024)) = 2
        _WaterTint("Under water color tint", Color) = (1,1,1,1)
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

            //sampler2D _MainTex;
           
            sampler2D _MainTex;
            float4 _MainTex_ST;
             float4 _VignetteColor;
            float4 _MainTex_TexelSize;
            float _VignetteSize;
            float3 _Distortion;
            float _ScanOpacity;
            float4 _Resolution;
            float _Spread;
            float4 _WaterTint;
        
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
          

            float4 scanLineIntensity(float uv, float resolution, float opacity)
             {
                  float intensity = sin(uv * resolution * UNITY_PI * 2.0);
                 intensity = ((0.5 * intensity) + 0.5) * 0.9 + 0.1;
                 return fixed4(fixed3(pow(intensity, opacity),pow(intensity, opacity),pow(intensity, opacity)), 1.0);
             }

            float2 remapUV(float2 uv)
            {
                uv = uv * 2.0 - 1;
                float2 offset = abs(uv.yx) / _Distortion.xy;//float2(_Distortion.x, _Distortion.y);
                uv = uv + uv * offset * offset;
                uv = uv * 0.5 + 0.5;
                return uv;
            }

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

            fixed4 frag (v2f i) : SV_Target
            {
                float2 centre = (0.5,0.5);
                float2 dist = i.uv - centre;
                float d = length(dist);
                float vignette = smoothstep(1-_VignetteSize,1,d);

                float2 newUv = remapUV(i.uv);
               
                
                
                fixed4 texColor = tex2D(_MainTex, newUv);
              
                fixed4 output; //= texColor + _Spread * GetBayer4(width,height);
                output = texColor;
                output = quantize_color(output,_NumCol);
                i.screenPos.x *= _Resolution.x;
                i.screenPos.y *= _Resolution.y;
                output.rgb = DitherCrunch(output.rgb,i.screenPos);
                //
                
                output *= scanLineIntensity(newUv.x,_Resolution.y,_Resolution.z);
                output *= scanLineIntensity(newUv.y,_Resolution.x,_Resolution.w);
                output = quantize_color(output,_NumCol);
               
                
                fixed4 vignetteColor = (1.0 - _VignetteColor) * vignette;
                fixed4 finalColor = output - vignetteColor * _Distortion.z;
              
                
               // output.rbg += 0.5;
                return saturate(finalColor + _WaterTint);
            }
            ENDCG
        }
    }
}
