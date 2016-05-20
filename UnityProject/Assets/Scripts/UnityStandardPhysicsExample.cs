using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityStandardPhysicsExample : MonoBehaviour
{
    public int ballCount = 5000;
    public string prefix = "Custom";

    // Use this for initialization
    void Start()
    {
        BallGenerator bg = new BallGenerator();
        bg.Generate(gameObject, ballCount, prefix); 
    }
}
