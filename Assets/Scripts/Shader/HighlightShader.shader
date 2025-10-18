Shader "Custom/HighlightShader"
{
    Properties
    {
        _Selected("Selected Color", Color) = (1, 1, 1, 1)
        _Hovered("Hovered Color", Color) = (1, 1, 1, 1)
        _Regular("Regular Color", Color) = (1, 1, 1, 1)
        _Corrupted("Corrupted Color", Color) = (1, 1, 1, 1)
        _IsCorrupted("Blah", Float) = 0
        _IsSelected("Blah", Float) = 0
        _IsHovered("Blah", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Cull Off

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
                int _IsCorrupted;
                int _IsSelected;
                int _IsHovered;
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
                float4 Color = 
                    _IsCorrupted ? _Corrupted :
                    _IsSelected ? _Selected : 
                    _IsHovered ? _Hovered : _Regular;
                    
                return Color;
            }
            ENDHLSL
        }
    }
}
