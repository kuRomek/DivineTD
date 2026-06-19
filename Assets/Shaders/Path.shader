Shader "Custom/Path"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Stop1 ("Stop1", Float) = 0.25
        _Stop2 ("Stop2", Float) = 0.50
        _Stop3 ("Stop3", Float) = 0.75

        _Color1 ("Color1", Color) = (1,0,0,1)
        _Color2 ("Color2", Color) = (0,1,0,1)
        _Color3 ("Color3", Color) = (0,0,1,1)
        _Color4 ("Color4", Color) = (1,1,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float _Stop1;
            float _Stop2;
            float _Stop3;

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float x = i.uv.x;

                float4 tint;

                if (x < _Stop1)
                    tint = _Color1;
                else if (x < _Stop2)
                    tint = _Color2;
                else if (x < _Stop3)
                    tint = _Color3;
                else
                    tint = _Color4;

                fixed4 tex = tex2D(_MainTex, i.uv);

                //return tex * tint;
                return float4(i.uv.x, i.uv.x, i.uv.x, 1);
            }

            ENDCG
        }
    }
}