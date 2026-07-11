using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image loadingImage;

    [Header("Loading Images")]
    [SerializeField] private List<Sprite> loadingSprites = new List<Sprite>();

    [Header("Loading Settings")]
    [SerializeField] private float totalLoadingTime = 10f;
    [SerializeField] private float imageChangeInterval = 2f;
    [SerializeField] private float fadeDuration = 0.5f;

    [Header("Scene")]
    [SerializeField] private int nextSceneIndex = 2;

    private HashSet<int> usedIndexes = new HashSet<int>();

    private void Start()
    {
        if (loadingSprites.Count == 0)
        {
            Debug.LogError("No loading sprites assigned!");
            return;
        }

        SetRandomImage();

        StartCoroutine(LoadingRoutine());
    }

    IEnumerator LoadingRoutine()
    {
        float timer = 0f;

        while (timer < totalLoadingTime)
        {
            yield return new WaitForSeconds(imageChangeInterval - fadeDuration);

            yield return Fade(1f, 0f);

            SetRandomImage();

            yield return Fade(0f, 1f);

            timer += imageChangeInterval;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void SetRandomImage()
    {
        if (loadingSprites.Count == 1)
        {
            loadingImage.sprite = loadingSprites[0];
            return;
        }

        if (usedIndexes.Count >= loadingSprites.Count)
            usedIndexes.Clear();

        int index;

        do
        {
            index = Random.Range(0, loadingSprites.Count);
        }
        while (usedIndexes.Contains(index));

        usedIndexes.Add(index);

        loadingImage.sprite = loadingSprites[index];
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        Color color = loadingImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);

            loadingImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        loadingImage.color = color;
    }
}