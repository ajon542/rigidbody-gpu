#ifndef GRABPASSDISTORTION_GCINC
#define GRABPASSDISTORTION_GCINC

// ----------------------------------------------------------------------------
// Variables
// ----------------------------------------------------------------------------
sampler2D _GrabTexture;
uniform float _DistortionValue;
uniform sampler2D _DistortionTexture;
uniform float4 _DistortionTexture_ST;

// ----------------------------------------------------------------------------
// Data Structures
// ----------------------------------------------------------------------------
struct appdata_t {
    float4 vertex  : POSITION;
    half2 texcoord : TEXCOORD0;
    fixed4 color   : COLOR;
};

struct v2f {
    float4 vertex  : SV_POSITION;
    fixed4 color   : COLOR;
    half2 texcoord : TEXCOORD0;
    half4 screenuv : TEXCOORD1;
};

// ----------------------------------------------------------------------------
// Vertex Shader
// ----------------------------------------------------------------------------
v2f vert (appdata_t v) {
    v2f o = (v2f)0;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.color  = v.color;

    // texcoord.xy stores the distortion texture coordinates.
    o.texcoord = v.texcoord;

    // Compute the grab screen position from the vertex.
    o.screenuv = ComputeGrabScreenPos(o.vertex);

    return o;
}

// ----------------------------------------------------------------------------
// Pixel Shader
// ----------------------------------------------------------------------------
fixed4 frag (v2f i) : COLOR {

    half2 distUV = i.texcoord.xy;

    // Sample the distortion texture.
    float4 distColor = tex2D(_DistortionTexture, TRANSFORM_TEX(distUV, _DistortionTexture));

    // Use the distortion texture to calculate the grab texture coordinates.
    float u = (i.screenuv.xy.r + ((distColor.r - 0.5) * _DistortionValue));
    float v = (i.screenuv.xy.g + ((distColor.g - 0.5) * _DistortionValue));
    float2 grabUV = float2(u, v);

    // Sample the grab texture.
    float4 grabColor = tex2D(_GrabTexture, grabUV);
    return grabColor;
}

#endif // GRABPASSDISTORTION_GCINC
