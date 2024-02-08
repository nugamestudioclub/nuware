using System;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager instance;

    private GameObject m_root;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            m_root = gameObject;
        }
    }

    /// <summary>
    /// Gets the root of the persistent hierarchy. Most likely, you shouldn't
    /// need to use this.
    /// </summary>
    /// <returns></returns>
    public Transform GetRoot() => m_root.transform;

    /// <summary>
    /// Search the persistant hierarchy for a gameobject matching the given predicate.
    /// Expensive operation; be sure to cache the result.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns>The GameObject if found, null otherwise.</returns>
    public GameObject FindInHierarchy(Predicate<Transform> predicate)
    {
        Queue<Transform> roots = new();
        roots.Enqueue(transform);

        while (roots.Count > 0)
        {
            Transform root = roots.Dequeue();

            if (predicate.Invoke(root))
                return root.gameObject;
            

            foreach (Transform child in root)
                roots.Enqueue(child);
        }

        return null;
    }

    /// <summary>
    /// Returns a predicate that matches transforms if they are completely identical
    /// to the input name.
    /// </summary>
    /// <param name="name_comp"></param>
    /// <returns></returns>
    public Predicate<Transform> GetNamePredicate(string name_comp) 
        => (Transform t) => t.gameObject.name.Equals(name_comp);
    
    /// <summary>
    /// Returns a predicate that matches transforms if they are have a given component.
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public Predicate<Transform> GetComponentPredicate<Type>() where Type : MonoBehaviour 
        => (Transform t) => t.GetComponent<Type>() == null;
}
