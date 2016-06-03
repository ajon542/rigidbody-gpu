Shader "Samples/MeshExplode/Mesh Explode" {

    Properties {
        _SpriteTex ("Base (RGB)", 2D) = "white" {}
        _Size ("Size", Range(0, 3)) = 0.5
    }

    SubShader {
        Pass {
            Tags { "RenderType"="Opaque" }
            LOD 200

            CGPROGRAM
                #pragma target 5.0
                #pragma vertex VS_Main
                #pragma fragment FS_Main
                #pragma geometry GS_Main
                #include "UnityCG.cginc"
                #include "MeshExplode.cginc"
            ENDCG
        }
    } 
}
