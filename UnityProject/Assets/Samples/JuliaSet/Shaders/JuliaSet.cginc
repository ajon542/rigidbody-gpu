#ifndef JULIASET_GCINC
#define JULIASET_GCINC

// ----------------------------------------------------------------------------
// Variables
// ----------------------------------------------------------------------------
sampler2D _MainTex;
float4 _MainTex_ST;

// ----------------------------------------------------------------------------
// Data Structures
// ----------------------------------------------------------------------------
struct appdata {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f {
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

struct Complex {
    float real;
    float imag;
};

// ----------------------------------------------------------------------------
// Functions
// ----------------------------------------------------------------------------
Complex ComplexAdd (Complex a, Complex b) {
    Complex result;
    result.real = a.real + b.real;
    return result;
}

Complex ComplexMul (Complex a, Complex b) {
    Complex result;
    result.real = (a.real * b.real) - (a.imag * b.imag);
    result.imag = (2 * a.imag * b.imag);
    return result;
}

Complex QuadraticIteration (Complex z, Complex c) {
    float tmpReal = (z.real * z.real) - (z.imag * z.imag) + c.real;
    float tmpImag = (2 * z.real * z.imag) + c.imag;

    Complex result;
    result.real = tmpReal;
    result.imag = tmpImag;

    return result;
}

// ----------------------------------------------------------------------------
// Vertex Shader
// ----------------------------------------------------------------------------
v2f vert (appdata v) {
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    return o;
}

// ----------------------------------------------------------------------------
// Pixel Shader
// ----------------------------------------------------------------------------
fixed4 frag (v2f i) : SV_Target {
    // sample the texture
    fixed4 col = tex2D(_MainTex, i.uv);
    return col;
}

#endif // JULIASET_GCINC
