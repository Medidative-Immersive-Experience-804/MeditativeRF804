Shader "Custom/InteractiveSandShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}
        _SandColor ("Sand Color", Color) = (0.76, 0.6, 0.42, 1) // Brown color
        _WaveAmplitude1 ("Wave Amplitude 1", Float) = 0.05
        _WaveFrequency1 ("Wave Frequency 1", Float) = 15
        _WaveDirection1 ("Wave Direction 1", Vector) = (1,1,0,0)
        _WaveAmplitude2 ("Wave Amplitude 2", Float) = 0.03
        _WaveFrequency2 ("Wave Frequency 2", Float) = 20
        _WaveDirection2 ("Wave Direction 2", Vector) = (-1,1,0,0)
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
            sampler2D _HeightMap;
            float4 _SandColor; // Added brown color property
            float _WaveAmplitude1;
            float _WaveFrequency1;
            float2 _WaveDirection1;
            float _WaveAmplitude2;
            float _WaveFrequency2;
            float2 _WaveDirection2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float wave1 = _WaveAmplitude1 * sin(dot(i.uv, _WaveDirection1) * _WaveFrequency1);
                float wave2 = _WaveAmplitude2 * sin(dot(i.uv, _WaveDirection2) * _WaveFrequency2);
                fixed4 col = tex2D(_MainTex, i.uv);
                float height = tex2D(_HeightMap, i.uv).r;
                col.rgb *= _SandColor.rgb;
                col.rgb += wave1 + wave2; // Adding complex wave effects to the color
                return col;
            }
            ENDCG
        }
    }
}
