Shader "Custom/CloudShader"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,0)
        _Density("Density", Float) = .5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                float _Density;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexNormalInputs NormalInputs = GetVertexNormalInputs(IN.normal);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.normal = NormalInputs.normalWS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return float4(_Color.xyz, _Density);
            }
            ENDHLSL
        }
    }
}
