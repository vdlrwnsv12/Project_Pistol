Shader "Custom/RedDotSight"
{
    Properties
    {
        _DotColor ("Dot Color", Color) = (5, 0, 0, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+10" }
        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Blend One One // Additive
            Cull Off

            Stencil
            {
                Ref 1
                Comp Equal
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _DotColor;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _DotColor;
            }
            ENDCG
        }
    }
}