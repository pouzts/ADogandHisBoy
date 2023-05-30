using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void Load(string sceneName)
    {
        if (!IsSceneLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
    }

    public static void Load(string sceneName, LoadSceneMode sceneMode)
    {
        if (!IsSceneLoaded(sceneName))
            SceneManager.LoadScene(sceneName, sceneMode);
    }

    public static bool IsSceneLoaded(string sceneName)
    {
        // loop through every scene in the project
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            // set scene to the scene index
            Scene scene = SceneManager.GetSceneAt(i);

            // if scene.name is equal to sceneName
            if (scene.name == sceneName)
                return true;
        }

        return false;
    }
}
