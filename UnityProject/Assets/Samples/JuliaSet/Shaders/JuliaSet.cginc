#ifndef JULIASET_GCINC
#define JULIASET_GCINC

// ----------------------------------------------------------------------------
// Variables
// ----------------------------------------------------------------------------
sampler2D _MainTex;
float4 _MainTex_ST;
float _Bounds;

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

// Get the result of adding two complex numbers.
Complex ComplexAdd (Complex a, Complex b) {
    Complex result;
    result.real = a.real + b.real;
    result.imag = a.imag + b.imag;
    return result;
}

// Gets the result of multiplying two complex numbers.
Complex ComplexMul (Complex a, Complex b) {
    Complex result;
    result.real = (a.real * b.real) - (a.imag * b.imag);
    result.imag = (a.real * b.imag) + (a.imag * b.real);
    return result;
}

// Gets the absolute value squared (or magnitude squared) of a complex number.
float ComplexAbs(Complex a) {
    return (a.real * a.real) + (a.imag * a.imag);
}

// Perform a quadratic iteration to generate the Julia set.
int QuadraticIteration (Complex z, Complex c) {
    int iterations = 0;
    while (iterations < 1024 && ComplexAbs(z) < 20) {
        Complex tmp = ComplexMul(z, z);
        z = ComplexAdd(tmp, c);
        iterations++;
    }
    return iterations;
}

// Convert a color in range [0, 255] to range [0, 1].
float4 ConvertColor (int r, int g, int b) {
    return float4(r/255.0, g/255.0, b/255.0, 1);
}

// Generate a color for the Julia set.
float4 GetColor(int iterations) {
    float value = 3.5 * iterations;
    int component = fmod(value, 255);

    if (value > 700) {
        return ConvertColor(255, 255, component);
    }
    else if (value > 255) {
        return ConvertColor(255, component, 0);
    }
    else {
        return ConvertColor(component, 0, 0);
    }
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

    float xMin = -_Bounds;
    float xMax = _Bounds;
    float yMin = -_Bounds;
    float yMax = _Bounds; 
    float xDelta = xMax - xMin;
    float yDelta = yMax - yMin;

    // Initialize the dynamic system;
    Complex z;
    z.real = -_Bounds + i.uv.x * xDelta;
    z.imag = -_Bounds + i.uv.y * yDelta;

    Complex c;
    c.real = -0.8;
    c.imag = 0.156;

    // Perform the iterations.
    int iterations = QuadraticIteration(z, c);

    // Obtain the color.
    fixed4 col = GetColor(iterations);
    return col;
}

#endif // JULIASET_GCINC
