using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image image = null;
    [SerializeField] private Color fadeInColor;
    [SerializeField] private Color fadeOutColor;

    [SerializeField] private float duration = 2f;
    [SerializeField] private bool startAtStart = false;

    private bool isDone = false;

    private void Start()
    {
        if (startAtStart)
            FadeIn();
    }

    public void TransitionScene(string sceneName)
    {
        FadeIn();
        
        if (isDone)
            SceneLoader.Load(sceneName);
        
        FadeOut();
    }

    public void FadeIn() 
    {
        StartCoroutine(FadeScene(fadeOutColor, fadeInColor, duration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeScene(fadeInColor, fadeOutColor, duration));
    }

    private IEnumerator FadeScene(Color startColor, Color endColor, float duration)
    {
        isDone = false;
        
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (image != null)
                image.color = Color.Lerp(startColor, endColor, timer/duration);

            yield return null;
        }

        isDone = true;
    }
}
