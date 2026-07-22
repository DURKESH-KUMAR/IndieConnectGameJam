using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] Text scoreText;
    void Start()
    {
        
        scoreText.text = Mathf.Ceil(PlayerPrefs.GetFloat("Score")).ToString();
    }

    // Update is called once per frame



}
