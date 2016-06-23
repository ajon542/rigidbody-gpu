using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class BallGenerator : IObjectGenerator
{
    /// <summary>
    /// Generates a list of ball game objects.
    /// </summary>
    /// <remarks>
    /// It is up to the caller to destroy the game objects.
    /// </remarks>
    /// <param name="parent">The parent game object for each ball.</param>
    /// <param name="ballCount">The number of balls to generate.</param>
    /// <param name="prefix">Quick and dirty was to load the type of ball from resources.</param>
    /// <returns>The list of generated ball objects.</returns>
    public List<GameObject> Generate(GameObject parent, int ballCount, string prefix)
    {
        List<GameObject> balls = new List<GameObject>();
        GameObject go;
        for (int i = 0; i < ballCount; ++i)
        {
            // Random position.
            Vector3 randomPosition = new Vector3(Random.value * 100, Random.value * 100, Random.value * 100);

            // Random ball prefab.
            StringBuilder sb = new StringBuilder();
            sb.Append(prefix);
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
