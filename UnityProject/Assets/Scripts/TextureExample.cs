using UnityEngine;
using System.Collections;

public class TextureExample : MonoBehaviour
{
    public ComputeShader shader, shaderCopy;

    RenderTexture tex, texCopy;

    void Start()
    {
        tex = new RenderTexture(64, 64, 0);
        tex.enableRandomWrite = true;
        tex.Create();

        texCopy = new RenderTexture(64, 64, 0);
        texCopy.enableRandomWrite = true;
        texCopy.Create();

        shader.SetTexture(0, "tex", tex);
        shader.Dispatch(0, tex.width / 8, tex.height / 8, 1);

        shaderCopy.SetTexture(0, "tex", tex);
        shaderCopy.SetTexture(0, "texCopy", texCopy);
        shaderCopy.Dispatch(0, texCopy.width / 8, texCopy.height / 8, 1);
    }

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), texCopy);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Swap(ref tex, ref texCopy);
            shaderCopy.SetTexture(0, "tex", tex);
            shaderCopy.SetTexture(0, "texCopy", texCopy);
            shaderCopy.Dispatch(0, texCopy.width / 8, texCopy.height / 8, 1);
        }
    }

    void Swap(ref RenderTexture a, ref RenderTexture b)
    {
        RenderTexture tmp = a;
        a = b;
        b = tmp;
    }

    void OnDestroy()
    {
        tex.Release();
        texCopy.Release();
    }
}