Shader "Custom/GrabPass Distortion" {

    Properties {
        _DistortionTexture ("DistortionTexture", 2D) = "black" {}
        _DistortionValue ("Distortion Value", Range(0, 0.15)) = 0.07692308
    }

    SubShader {

        Tags {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        Fog { Mode Off }
        ZWrite Off
        ZTest Always
        Cull Off
        
        // See http://docs.unity3d.com/Manual/SL-GrabPass.html
        // Will grab screen contents into a texture, but will only do that once per frame for
        // the first object that uses the given texture name.
        GrabPass { "_GrabTexture" }

        Pass {
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "GrabPassDistortion.cginc"
            ENDCG
        }
    }
}
