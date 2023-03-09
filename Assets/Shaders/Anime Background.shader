Shader "Unlit/Anime Background"
{
    Properties
    {
        _MainTex ("Speed lines 1", 2D) = "white" {}
        _MainTex2 ("Speed lines 2", 2D) = "white" {}
        _LineColor1("Lines color 1",Color) = (1,1,1,1)
        _LineColor2("Lines color 2",Color) = (1,1,1,1)
        _BG_Color("Background color", Color) = (1,1,1,1)
        _Scroll1("Speed lines 1 scroll speed", float) = 1
        _Scroll2("Speed lines 2 scroll speed", float) = 1
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
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
             sampler2D _MainTex2;
            float4 _MainTex2_ST;
            float4 _LineColor1;
            float4 _LineColor2;
            float4 _BG_Color;
            float _Scroll1;
            float _Scroll2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _MainTex2);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                i.uv.y += _Time.x * _Scroll1;
                i.uv2.y += _Time.x * _Scroll2;
                fixed4 lines1 = tex2D(_MainTex, i.uv);
                fixed4 lines2 = tex2D(_MainTex2, i.uv2);

                lines1 *= _LineColor1;
                lines2 *= _LineColor2;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return _BG_Color + (lines1 + lines2);
            }
            ENDCG
        }
    }
}
