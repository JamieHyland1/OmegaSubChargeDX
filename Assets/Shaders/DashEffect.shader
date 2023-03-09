Shader "Unlit/DashEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
       
        _Color("After Image tint", Color) = (1,1,1,1)
        _Opacity("After Image opacity ", Range(0,1)) = 1
    }
    SubShader
    {
       Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        ZWrite Off
       Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
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
                float4 prev_vertex : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
           
            float4 _Color;
            float _Opacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.prev_vertex = o.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
             
                // apply fog
                fixed4 newCol = col*_Color;
                 newCol.a = _Opacity;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return newCol;
            }
            ENDCG
        }
    }
}
