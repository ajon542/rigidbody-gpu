#ifndef BLENDMODES_GCINC
#define BLENDMODES_GCINC

#include "UnityCG.cginc"

#define Multiply     0
#define Screen       1
#define SoftLight    2

// ----------------------------------------------------------------------------
// Variables
// ----------------------------------------------------------------------------
sampler2D _MainTex;
sampler2D _GrabTexture;
float4 _GrabTexture_ST;

float4 _MainTex_ST;
fixed4 _TintColor;
int _Blend;

// ----------------------------------------------------------------------------
// Data Structures
// ----------------------------------------------------------------------------
struct appdata_t {
    float4 vertex : POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f {
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    half2 texcoord : TEXCOORD0;
    half4 screencoord : TEXCOORD1;
};

// ----------------------------------------------------------------------------
// Vertex Shader
// ----------------------------------------------------------------------------
v2f vert (appdata_t v)
{
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.screencoord = ComputeGrabScreenPos(o.vertex);
    o.color = v.color;
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    return o;
}

// ----------------------------------------------------------------------------
// Pixel Shader
// ----------------------------------------------------------------------------
fixed4 frag (v2f i) : SV_Target
{
    // get the bottom layer color
    float2 grabUV = i.screencoord.xy / i.screencoord.w;
    fixed4 grabColor = tex2D(_GrabTexture, grabUV);

    fixed4 color = i.color * _TintColor * tex2D(_MainTex, i.texcoord);

    if (_Blend == Multiply)
    {
        color.rgb *= grabColor.rgb;
    }

    if (_Blend == Screen)
    {
        color.rgb = 1 - (1 - color.rgb) * (1 - grabColor.rgb);
    }

    if (_Blend == SoftLight)
    {
        color.rgb = grabColor.rgb > 0.5 ? (1 - (1-grabColor.rgb) * (1-(color.rgb-0.5))) : (grabColor.rgb * (color.rgb+0.5));
    }

    return color;
}

#endif // BLENDMODES_GCINC