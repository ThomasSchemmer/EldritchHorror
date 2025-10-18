Shader "Custom/MarsShader"
{
    Properties
    {
        _ColorA("ColorA", Color) = (0,0,0,0)
        _ColorB("ColorB", Color) = (0,0,0,0)
        _ColorC("ColorC", Color) = (0,0,0,0)
        _LowScale("Low Scale", Float) = 1
        _HighScale("High Scale", Float) = 1
        _LowOverlay ("LowOverlay", Range(1, 5)) = 1
        _HighOverlay ("HighOverlay", Range(5, 15)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma geometry geom
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

            struct v2g{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct g2f
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 positionOS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _ColorA;
                half4 _ColorB;
                half4 _ColorC;
                float _LowScale;
                float _HighScale;
                float _LowOverlay;
                float _HighOverlay;
            CBUFFER_END

            v2g vert(Attributes IN)
            {
            //pass through
                v2g OUT;
                OUT.vertex = IN.vertex;
                OUT.uv = IN.uv;
                OUT.normal = IN.normal;
                return OUT;
            }

            
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream){
                g2f OUT;
                
                float3 Pos = (IN[0].vertex.xyz + IN[1].vertex.xyz + IN[2].vertex.xyz) / 3.0;
                for (int i = 0; i < 3; i++){
                    VertexNormalInputs NormalInputs = GetVertexNormalInputs(IN[i].normal);
                    OUT.positionHCS = TransformObjectToHClip(IN[i].vertex.xyz);
                    OUT.uv = IN[i].uv;
                    OUT.normal = NormalInputs.normalWS;
                    OUT.positionOS = Pos;
                    triStream.Append(OUT);
                }
                triStream.RestartStrip();
            }

            half4 frag(g2f IN) : SV_Target
            {
                Light Light = GetMainLight();
                float L = LightingLambert(Light.color, Light.direction, IN.normal.xyz);

                float NoiseA = snoise((IN.positionOS.xyz) * _LowScale);
                float NoiseB = snoise((IN.positionOS.xyz + 5) * _HighScale);
                NoiseA = NoiseA / 2.0 + 0.5;
                NoiseA = pow(NoiseA, _LowOverlay);
                NoiseB = NoiseB / 2.0 + 0.5;
                NoiseB = pow(NoiseB, _HighOverlay);
                float4 Color = lerp(_ColorA, _ColorB, NoiseA);
                Color = lerp(Color, _ColorC, NoiseB);
                return Color * L;
            }
            ENDHLSL
        }
    }
}
