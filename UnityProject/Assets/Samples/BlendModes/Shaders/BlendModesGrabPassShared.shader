
Shader "Samples/BlendModes/GrabPass Blend Modes" {

    Properties {
        _TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Texture", 2D) = "white" {}
        [Enum(BlendModes)] _Blend ("Blend mode", int) = 0
    }

    Category {
        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Always

        SubShader {

            GrabPass { "_GrabTexture" }

            Pass {

                Blend SrcAlpha OneMinusSrcAlpha

                CGPROGRAM

                #pragma vertex vert
                #pragma fragment frag

                #include "BlendModesGrabPass.cginc"

                ENDCG
            }
        }
    }
}
