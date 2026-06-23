Shader "FPCT/Outline"
{
    Properties
    {
        _OutlineScale ("Outline Scale", Float) = 1
        _OutlineColor ("Outline Color", Color) = (1, 0.7843137, 0.5019608, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1" }
        Pass
        {
            Cull Front
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _OutlineScale;
            fixed4 _OutlineColor;
            struct appdata
            {
                float4 vertex : POSITION;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };
            v2f vert(appdata v)
            {
                v2f o;
                float3 scaledPos = v.vertex.xyz * _OutlineScale;
                o.vertex = UnityObjectToClipPos(float4(scaledPos, 1));
                return o;
            }
            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    Fallback Off
}