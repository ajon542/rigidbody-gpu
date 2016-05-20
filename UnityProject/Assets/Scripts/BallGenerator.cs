using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BallGenerator : MonoBehaviour
{
    public int ballCount = 5000;
    private List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        GameObject go;
        for (int i = 0; i < ballCount; ++i)
        {
            // Random position.
            Vector3 randomPosition = new Vector3(Random.value * 20, Random.value * 20, Random.value * 20);

            // Random ball prefab.
            StringBuilder sb = new StringBuilder();
            sb.Append("Ball");
            sb.Append(Random.Range(1, 16));

            // Instantiate the prefab.
            go = (GameObject)Instantiate(Resources.Load(sb.ToString(), typeof(GameObject)), randomPosition, Quaternion.identity);

            // Set the parent so the hierarchy window in Unity editor is clean.
            go.transform.parent = gameObject.transform;
            balls.Add(go);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            print("All " + Resources.FindObjectsOfTypeAll<UnityEngine.Object>().Length);
            print("Textures " + Resources.FindObjectsOfTypeAll<Texture>().Length);
            print("AudioClips " + Resources.FindObjectsOfTypeAll<AudioClip>().Length);
            print("Meshes " + Resources.FindObjectsOfTypeAll<Mesh>().Length);
            print("Materials " + Resources.FindObjectsOfTypeAll<Material>().Length);
            print("GameObjects " + Resources.FindObjectsOfTypeAll<GameObject>().Length);
            print("Components " + Resources.FindObjectsOfTypeAll<Component>().Length);
        }
    }
}
