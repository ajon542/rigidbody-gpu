Shader "Custom/GSPassThrough"
{
    Properties 
    {
        _SpriteTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader 
    {
        Pass
        {
            Tags {
                "RenderType" = "Opaque"
            }
            LOD 200


            CGPROGRAM
                #pragma target 5.0
                #pragma vertex VS_Main
                #pragma fragment FS_Main
                #pragma geometry GS_Main
                #include "UnityCG.cginc"

                // **************************************************************
                // Data structures                                              *
                // **************************************************************
                struct GS_INPUT
                {
                    float4  pos     : POSITION;
                    float3  normal  : NORMAL;
                    float2  tex0    : TEXCOORD0;
                };
                struct FS_INPUT
                {
                    float4  pos     : POSITION;
                    float2  tex0    : TEXCOORD0;
                };

                // **************************************************************
                // Vars                                                         *
                // **************************************************************
                Texture2D _SpriteTex;
                SamplerState sampler_SpriteTex;

                // **************************************************************
                // Shader Programs                                              *
                // **************************************************************

                // Vertex Shader
                //
                GS_INPUT VS_Main(appdata_base v)
                {
                    GS_INPUT output = (GS_INPUT)0;
                    output.pos =  mul(UNITY_MATRIX_MVP, v.vertex);
                    output.normal = v.normal;
                    output.tex0 = TRANSFORM_UV(0);
                    return output;
                }

                // Geometry Shader
                //
                [maxvertexcount(3)]
                void GS_Main(triangle GS_INPUT p[3], inout TriangleStream<FS_INPUT> triStream)
                {
                    FS_INPUT pIn;
                    pIn.pos = p[0].pos;
                    pIn.tex0 = p[0].tex0;
                    triStream.Append(pIn);
                    pIn.pos = p[1].pos;
                    pIn.tex0 = p[1].tex0;
                    triStream.Append(pIn);
                    pIn.pos = p[2].pos;
                    pIn.tex0 = p[2].tex0;
                    triStream.Append(pIn);
                }

                // Fragment Shader
                //
                float4 FS_Main(FS_INPUT input) : COLOR
                {
                    return _SpriteTex.Sample(sampler_SpriteTex, input.tex0);
                }
            ENDCG
        }
    } 
}