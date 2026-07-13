using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Image fadeImage;

    public string nextScene = "MainMenu";

    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    void Start()
    {
        videoPlayer.Pause(); // Wait until fade-in is complete
        videoPlayer.loopPointReached += VideoFinished;

        StartCoroutine(BeginIntro());
    }

    IEnumerator BeginIntro()
    {
        Color color = fadeImage.color;
        float time = 0;

        while (time < fadeInDuration)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(1, 0, time / fadeInDuration);
            fadeImage.color = color;

            yield return null;
        }

        videoPlayer.Play();
    }

    void VideoFinished(VideoPlayer vp)
    {
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        Color color = fadeImage.color;
        float time = 0;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(0, 1, time / fadeOutDuration);
            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene(nextScene);
    }
}