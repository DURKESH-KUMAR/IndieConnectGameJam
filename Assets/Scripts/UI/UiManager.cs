using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] int loadingSceneIndex;
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] AudioSource Bgm;
    [SerializeField] AudioMixer SfxMixer;
    [SerializeField] Slider sfxSlider,bgmSlider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance!=null&&instance != this)
        {
            Destroy(Bgm.gameObject);
            Destroy(instance.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(Bgm.gameObject);
        DontDestroyOnLoad(SettingsPanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(SettingsPanel!=null)
                Settings();
        }
    }
    private void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SfxVolume", 20);
        bgmSlider.value = PlayerPrefs.GetFloat("BgmVolume", 1);

    }
    public void Play()
    {
        SceneManager.LoadScene(loadingSceneIndex);
    }

    public void Settings()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeInHierarchy);
    }

    public void SfxVolume()
    {
        SfxMixer.SetFloat("SfxVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("SfxVolume",sfxSlider.value);
    }
    public void BgmVolume()
    {
        Bgm.volume=bgmSlider.value;
        PlayerPrefs.SetFloat("BgmVolume", bgmSlider.value);

    }


}
