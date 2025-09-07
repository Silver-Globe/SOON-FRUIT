Shader "Custom/PieSliceShaderMulti"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ActiveColor ("Active Color", Color) = (0,1,0,1)
        _TotalSections ("Total Sections", Float) = 5
        _ActiveMask ("Active Mask", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

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

            fixed4 _BaseColor;
            fixed4 _ActiveColor;
            float _TotalSections;
            float _ActiveMask;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Remap UV 0..1 → -1..1 so circle is centered
                o.uv = v.uv * 2 - 1;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // radius check → keep circular
                float r = length(uv);
                if (r > 1.0) discard;

                // angle in degrees, 0° at top
                float angle = 360 - atan2(uv.y, uv.x) * 57.2958 + 90;
                if (angle < 0) angle += 360;
                if (angle >= 360) angle -= 360;

                // determine slice index
                int slice = (int)(angle / (360.0 / _TotalSections));

                // check bitmask for active slices
                int mask = (int)_ActiveMask;
                int maskBit = 1 << slice;
                if ((mask & maskBit) != 0)
                    return _ActiveColor;
                else
                    return _BaseColor;
            }
            ENDCG
        }
    }
}
