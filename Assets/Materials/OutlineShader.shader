Shader "Custom/OutlineShader"
{
    Properties
    {
        _OutlineWidth ("Outline Width", Range(0.01, 0.1)) = 0.01
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1) // Default yellow outline
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        LOD 100

        // Render the outline only
        Pass
        {
            Name "OutlinePass"
            Cull Front // Hide front-facing polygons to leave only the outline
            ZWrite On
            ZTest Less

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                // Expand vertices outward along normals to create the outline
                v.vertex.xyz += v.normal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor; // Solid outline color
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
