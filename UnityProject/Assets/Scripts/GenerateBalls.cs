using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateBalls : MonoBehaviour
{
    public int ballCount = 10;
    public GameObject ball;

    private List<GameObject> balls = new List<GameObject>();


    void Start()
    {
        GameObject go;
        for (int i = 0; i < ballCount; ++i)
        {
            Vector3 randomPosition = new Vector3(Random.value * 20, Random.value * 20, Random.value * 20);
            go = (GameObject)Instantiate(ball, randomPosition, Quaternion.identity);
            go.transform.parent = gameObject.transform;
            balls.Add(go);
        }
    }

    void Update()
    {

    }
}
