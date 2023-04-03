using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a single instance of a GameObject to be used throughout a Unity Project.
/// </summary>
/// <typeparam name="T">
/// Component
/// </typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    // static instance
    private static T instance;

    public static T Instance 
    {
        get 
        {
            // if instance is null
            if (instance == null) 
            {
                // find an instance of T
                instance = FindObjectOfType<T>();

                // if the instance is still null
                if (instance == null )
                {
                    // create a game object with T component
                    // set the name to the type of T's name
                    GameObject obj = new(){ name = typeof(T).Name };
                    // add the T component
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // if instance is null
        if (instance == null)
        {
            // set the instance to this
            instance = this as T;
            // don't destroy game object on load
            DontDestroyOnLoad(gameObject);
        }
        // else
        else
        { 
            // destory this game object
            Destroy(gameObject);
        }
    }
}
