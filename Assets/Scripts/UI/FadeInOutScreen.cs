using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInOutScreen : MonoBehaviour
{
    public float speed = 1;

    private Image fadeOutScreen;
    private Color fadeColor = Color.black;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        fadeOutScreen = this.GetComponentInChildren<Image>();
    }

    public void ShowScreenNoDelay()
    {
        fadeColor.a = 1f;
        fadeOutScreen.color = fadeColor;
        fadeOutScreen.gameObject.SetActive(true);
    }

    public IEnumerator FadeIn()
    {
        float alpha = fadeOutScreen.color.a;

        fadeOutScreen.gameObject.SetActive(true);

        while (alpha < 1f)
        {
            yield return new WaitForSeconds(0.01f);
            alpha += 0.01f * speed;
            fadeColor.a = alpha;
            fadeOutScreen.color = fadeColor;
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = fadeOutScreen.color.a;

        while (alpha > 0)
        {
            yield return new WaitForSeconds(0.01f);
            alpha -= 0.01f * speed;
            fadeColor.a = alpha;
            fadeOutScreen.color = fadeColor;

            if (fadeColor.a <= 0f)
            {
                fadeOutScreen.gameObject.SetActive(false);
            }
        }
    }
}
