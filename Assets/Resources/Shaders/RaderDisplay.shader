Shader "UI/GlassRadarLightReactive"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint Color", Color) = (0, 1, 0, 0.3)
        _GlowStrength("Glow Strength", Range(0, 1)) = 0.2
        _Specular("Specular Highlight", Range(0, 1)) = 0.5
        _LightPos("Light Position (UV)", Vector) = (0.5, 0.5, 0, 0) // 라이트 위치 (UI UV 공간)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color;
            float _GlowStrength;
            float _Specular;
            float4 _LightPos;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 lightUV = _LightPos.xy;  // 0~1 좌표
                float2 diff = i.uv - lightUV;
                float dist = length(diff);

                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                float glow = pow(saturate(1.0 - dist), 2.0) * _GlowStrength;

                float specular = pow(saturate(dot(normalize(diff), float2(0.5, 0.5))), 16.0) * _Specular;

                col.rgb += glow + specular;

                return col;
            }
            ENDCG
        }
    }
}
