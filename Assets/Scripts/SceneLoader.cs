using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private int sceneIndex = 1;
    [SerializeField] int HomeSceneIndex, GameSceneIndex;
    [SerializeField] Image BlackScreen;
    [SerializeField] float fadeoutTime;
    bool load;

    /// <summary>
    /// Loads the scene using the specified build index.
    /// </summary>
    /// 
    void Update()
    {
        if (BlackScreen.color.a > 0 && !load)
            BlackScreen.color = new Color(0, 0, 0, BlackScreen.color.a - fadeoutTime * Time.deltaTime);
        if (load)
        {
            BlackScreen.color = new Color(0, 0, 0, BlackScreen.color.a + fadeoutTime * Time.deltaTime);
        }

    }
    public void LoadScene()
    {
        StartCoroutine(FadeToScene(sceneIndex));
    }
    public void Home()
    {
        StartCoroutine(FadeToScene(HomeSceneIndex));
    }
    public void Restart()
    {
        StartCoroutine(FadeToScene(GameSceneIndex));
    }

    public IEnumerator FadeToScene(int sceneIndex)
    {
        load = true;
        yield return new WaitForSeconds(fadeoutTime);
        SceneManager.LoadScene(sceneIndex);
    }
    /// <summary>
    /// Loads the next scene in the Build Settings.
    /// </summary>
    public void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("No more scenes available in Build Settings.");
        }
    }

    /// <summary>
    /// Loads a scene by its name.
    /// </summary>
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}