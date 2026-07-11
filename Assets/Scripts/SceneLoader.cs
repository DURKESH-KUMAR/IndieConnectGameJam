using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private int sceneIndex = 1;

    /// <summary>
    /// Loads the scene using the specified build index.
    /// </summary>
    public void LoadScene()
    {
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