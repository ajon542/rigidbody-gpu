Shader "Samples/JuliaSet/Julia Set" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Bounds ("Bounds", Float) = 2
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "JuliaSet.cginc"
            ENDCG
        }
    }
}
