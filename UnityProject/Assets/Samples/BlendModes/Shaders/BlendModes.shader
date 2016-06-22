
Shader "Samples/BlendModes/Blend Modes" {

    Properties {
        _TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Texture", 2D) = "white" {}
        [Enum(BlendModes)] _Blend ("Blend mode", int) = 0
    }

    SubShader {

        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always

        GrabPass { "_GrabTexture" }

        Pass {

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "BlendModes.cginc"

            ENDCG
        }
    }
}
