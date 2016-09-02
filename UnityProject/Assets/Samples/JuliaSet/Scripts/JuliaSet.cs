using UnityEngine;
using System.Collections;

public class JuliaSet : MonoBehaviour
{
    public ComputeShader juliaSetShader;
    private RenderTexture tex;

    private int textureSize = 1024;
    private int threadGroupCount = 128;

    void Start()
    {
        tex = new RenderTexture(textureSize, textureSize, 0);
        tex.enableRandomWrite = true;
        tex.Create();

        juliaSetShader.SetTexture(0, "tex", tex);
        juliaSetShader.Dispatch(0, threadGroupCount, threadGroupCount, 1);
    }

    private void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), tex);
    }

    private void OnDestroy()
    {
        tex.Release();
    }
}
