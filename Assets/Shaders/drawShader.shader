Shader "Custom/HeightMapDrawShader"
{
    Properties
    {
        _MainTex ("Height Map", 2D) = "white" {}
        _BrushPos ("Brush Position", Vector) = (0,0,0,0)
        _BrushStrength ("Brush Strength", Float) = 0.1
        _BrushSize ("Brush Size", Float) = 0.05
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _BrushPos;
            float _BrushStrength;
            float _BrushSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float dist = distance(i.uv, _BrushPos.xy);
                float4 texColor = tex2D(_MainTex, i.uv);
                if (dist < _BrushSize)
                {
                    texColor.r += _BrushStrength; // Modifying the red channel as an example; adjust as necessary for your use case.
                }
                return texColor;
            }
            ENDCG
        }
    }
}
