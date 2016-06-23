using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Interface for an object generator.
/// </summary>
public interface IObjectGenerator
{
    /// <summary>
    /// Generates game objects.
    /// </summary>
    /// <param name="parent">The parent object for these generated game objects.</param>
    /// <param name="count">The number of game objects to generate.</param>
    /// <param name="prefix">Custom identifier TODO: Should be removed and replaced with an implementation.</param>
    /// <returns>The list of generated game objects. The caller should handle their destruction.</returns>
    List<GameObject> Generate(GameObject parent, int count, string prefix);
}
