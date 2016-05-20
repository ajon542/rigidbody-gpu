using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityStandardPhysicsExample : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        BallGenerator bg = new BallGenerator();
        List<GameObject> balls = bg.Generate(gameObject, 1000);
    }
}
