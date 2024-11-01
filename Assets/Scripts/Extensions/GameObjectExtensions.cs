using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// Destroys all children of the game object
    /// </summary>
    /// <param name="gameObject">GameObject whose children are to be destroyed.</param>
    public static void DestroyAllChildren(this GameObject gameObject)
    {
        gameObject.transform.DestroyAllChildren();
    }

    /// <summary>
    /// Immediately destroys all children of the given GameObject.
    /// </summary>
    /// <param name="gameObject">GameObject whose children are to be destroyed.</param>
    public static void DestroyAllChildrenImmediate(this GameObject gameObject)
    {
        gameObject.transform.DestroyAllChildrenImmediate();
    }
}
