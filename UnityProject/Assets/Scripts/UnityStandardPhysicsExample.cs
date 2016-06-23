using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnityStandardPhysicsExample : MonoBehaviour
{
    public int ballCount = 5000;

    // Use this for initialization
    void Start()
    {
        IObjectGenerator bg = new BallGenerator();
        bg.Generate(gameObject, ballCount); 
    }
}
