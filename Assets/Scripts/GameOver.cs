using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int HomeSceneIndex;
    [SerializeField] Image BlackScreen;
    [SerializeField] float fadeoutTime;
    [SerializeField] Text scoreText;
    bool load;
    void Start()
    {
        StartCoroutine(Home());
        scoreText.text = Mathf.Ceil(PlayerPrefs.GetFloat("Score")).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(BlackScreen.color.a>0&&!load)
            BlackScreen.color = new Color(0, 0, 0, BlackScreen.color.a - fadeoutTime * Time.deltaTime);
        if (load)
        {
            BlackScreen.color = new Color(0, 0, 0, BlackScreen.color.a + fadeoutTime * Time.deltaTime);
        }

    }

    public IEnumerator Home()
    {
        yield return new WaitForSeconds(5);
        load = true;
        yield return new WaitForSeconds(fadeoutTime);
        SceneManager.LoadScene(HomeSceneIndex);
    }

}
