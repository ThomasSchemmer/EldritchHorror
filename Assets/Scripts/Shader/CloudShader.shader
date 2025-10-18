Shader "Custom/CloudShader"
{
    Properties
    {
        _LightInfluence("Influence", Range(1, 5)) = 2
        _LightColor("Light Color", Color) = (0,0,0,0)
        _Ambient("Ambient", Float) = 0.5
        _Color("Color", Color) = (0,0,0,0)
        _Density("Density", Float) = .5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        
        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

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

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Density;
                half4 _LightColor;
                float _Ambient;
                float _LightInfluence;
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
                return float4(_Color.xyz * L, min(_Density, saturate(L)));
            }
            ENDHLSL
        }
    }
}
