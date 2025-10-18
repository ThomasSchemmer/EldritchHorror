Shader "Custom/PlanetShader"
{
    Properties
    {
        _LightColor("Light Color", Color) = (0,0,0,0)
        _LightInfluence("Influence", Range(1, 5)) = 2
        _Ambient("Ambient", Float) = 0.5
         _ColorTex("Colors", 2D) = "white"
         [Enum(Earth, 0, Jupiter, 1, Saturn, 2, Cthulu, 3)] _Type("Type", float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Util.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 positionWS : TEXCOORD1;
            };

            sampler _ColorTex;

            CBUFFER_START(UnityPerMaterial)
                float _Type;
                float _LightInfluence;
                half4 _LightColor;
                float _Ambient;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexNormalInputs NormalInputs = GetVertexNormalInputs(IN.normal);
                VertexPositionInputs VertexInputs = GetVertexPositionInputs(IN.vertex.xyz);
                OUT.positionHCS = TransformObjectToHClip(IN.vertex.xyz);
                OUT.uv = IN.uv;
                OUT.normal = NormalInputs.normalWS;
                OUT.positionWS = VertexInputs.positionWS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 LightPos = 0;
                float L = dot(normalize(LightPos - IN.positionWS), IN.normal.xyz) + _Ambient;
                
                float2 uv = IN.uv + float2(0, _Type / 16.0);
                half4 color = tex2D(_ColorTex, uv);
                return (color * _LightInfluence + _LightColor) / (1 + _LightInfluence) * L;
            }
            ENDHLSL
        }
    }
}
