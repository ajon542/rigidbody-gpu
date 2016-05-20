﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BallGenerator
{
    /// <summary>
    /// Generates a list of ball game objects.
    /// </summary>
    /// <remarks>
    /// It is up to the called to destroy the game objects.
    /// </remarks>
    /// <param name="parent">The parent game object for each ball.</param>
    /// <param name="ballCount">THe number of balls to generate.</param>
    /// <returns>THe list of generated ball objects.</returns>
    public List<GameObject> Generate(GameObject parent, int ballCount)
    {
        List<GameObject> balls = new List<GameObject>();
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
            go = (GameObject)GameObject.Instantiate(Resources.Load(sb.ToString(), typeof(GameObject)), randomPosition, Quaternion.identity);

            // Set the parent so the hierarchy window in Unity editor is clean.
            go.transform.parent = parent.transform;
            balls.Add(go);
        }
        return balls;
    }
}
