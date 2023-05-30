using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnforcer : MonoBehaviour
{
    [SerializeField] LoadSceneMode sceneMode;
    [SerializeField] string sceneName;

    private void Awake()
    {
        SceneLoader.Load(sceneName, sceneMode);   
    }
}
