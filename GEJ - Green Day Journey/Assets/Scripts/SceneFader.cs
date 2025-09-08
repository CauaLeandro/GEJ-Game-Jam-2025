using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        Color color = fadeImage.color;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}