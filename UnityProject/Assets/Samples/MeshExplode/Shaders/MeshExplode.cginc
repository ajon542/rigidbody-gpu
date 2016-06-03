#ifndef MESHEXPLODE_GCINC
#define MESHEXPLODE_GCINC

// ----------------------------------------------------------------------------
// Variables
// ----------------------------------------------------------------------------
float _Size;
float4x4 _VP;
Texture2D _SpriteTex;
SamplerState sampler_SpriteTex;

// ----------------------------------------------------------------------------
// Data Structures
// ----------------------------------------------------------------------------
struct GS_INPUT {
    float4  pos     : POSITION;
    float3  normal  : NORMAL;
    float2  tex0    : TEXCOORD0;
};

struct FS_INPUT {
    float4  pos     : POSITION;
    float2  tex0    : TEXCOORD0;
};

// ----------------------------------------------------------------------------
// Vertex Shader
// ----------------------------------------------------------------------------
GS_INPUT VS_Main(appdata_base v) {

    GS_INPUT output = (GS_INPUT)0;

    // Calculate the vertex position in world space.
    output.pos =  mul(_Object2World, v.vertex);
    //output.normal = mul(_Object2World, v.normal);
    output.normal = v.normal;
    output.tex0 = TRANSFORM_UV(0);

    return output;
}

// ----------------------------------------------------------------------------
// Geometry Shader
// ----------------------------------------------------------------------------
[maxvertexcount(3)]
void GS_Main(triangle GS_INPUT p[3], inout TriangleStream<FS_INPUT> triStream) {

    // Move each vertex along its normal in world space.
    float4 v[3];
    v[0] = p[0].pos + float4(p[0].normal, 0) * _Size;
    v[1] = p[1].pos + float4(p[0].normal, 0) * _Size;
    v[2] = p[2].pos + float4(p[0].normal, 0) * _Size;

    // Output the vertices, multiplying by the view-projection matrix.
    FS_INPUT pIn;
    pIn.pos = mul(UNITY_MATRIX_VP, v[0]);
    pIn.tex0 = p[0].tex0;
    triStream.Append(pIn);

    pIn.pos =  mul(UNITY_MATRIX_VP, v[1]);
    pIn.tex0 = p[1].tex0;
    triStream.Append(pIn);

    pIn.pos =  mul(UNITY_MATRIX_VP, v[2]);
    pIn.tex0 = p[2].tex0;
    triStream.Append(pIn);
}

// ----------------------------------------------------------------------------
// Pixel Shader
// ----------------------------------------------------------------------------
float4 FS_Main(FS_INPUT input) : COLOR {

    return _SpriteTex.Sample(sampler_SpriteTex, input.tex0);
}

#endif // MESHEXPLODE_GCINC
