Shader "Custom/HighlightShader"
{
    Properties
    {
        _Selected("Selected Color", Color) = (1, 1, 1, 1)
        _Hovered("Hovered Color", Color) = (1, 1, 1, 1)
        _Regular("Regular Color", Color) = (1, 1, 1, 1)
        _Corrupted("Corrupted Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };


            CBUFFER_START(UnityPerMaterial)
                float4 _Selected;
                float4 _Hovered;
                float4 _Regular;
                float4 _Corrupted;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _Regular;
            }
            ENDHLSL
        }
    }
}
