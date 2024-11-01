using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Destroys all child game objects of the given transform.
    /// </summary>
    /// <param name="transform">The Transform whose child game objects are to be destroyed.</param>
    public static void DestroyAllChildren(this Transform transform)
    {
        transform.ForEveryChild(child => Object.Destroy(child.gameObject));
    }

    /// <summary>
    /// Immediately destroys all child game objects of the given transform.
    /// </summary>
    /// <param name="parent">The Transform whose child game objects are to be immediately destroyed.</param>
    public static void DestroyAllChildrenImmediate(this Transform parent)
    {
        parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
    }

    /// <summary>
    /// Executes a specified action for each child of a given transform.
    /// </summary>
    /// <param name="transform">The parent transform.</param>
    /// <param name="action">The action to be performed on each child.</param>
    /// <remarks>
    /// This method iterates over all child transforms in reverse order and executes a given action on them.
    /// The action is a delegate that takes a Transform as parameter.
    /// </remarks>
    public static void ForEveryChild(this Transform transform, System.Action<Transform> action)
    {
        for(var i = transform.childCount - 1; i >= 0; i--)
        {
            action(transform.GetChild(i));
        }
    }
}
