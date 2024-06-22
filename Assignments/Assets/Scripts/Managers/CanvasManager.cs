using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static bool isPaused;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button resumeButton;
    public Button returnToMenu;

    [Header("Menu")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Text")]
    public TMP_Text livesText;
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text sfxVolSliderText;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider sfxVolSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));

        if (returnToMenu)
            returnToMenu.onClick.AddListener(() => GameManager.Instance.LoadScene("Title"));

        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.LoadScene("Game"));
       
        if (settingsButton)
            settingsButton.onClick.AddListener(() => SetMenus(settingsMenu, mainMenu));

        if (backButton)
            backButton.onClick.AddListener(() => SetMenus( mainMenu, settingsMenu));

        if (masterVolSlider)
        {
            SetupSliderInfo(masterVolSlider, masterVolSliderText, "MasterVol");
        }
        if (musicVolSlider)
        {
            SetupSliderInfo(musicVolSlider, musicVolSliderText, "MusicVol");
        }
        if (sfxVolSlider)
        {
            SetupSliderInfo(sfxVolSlider, sfxVolSliderText, "SFXVol");
        }


        if (livesText)
        {
            GameManager.Instance.OnLifeValueChange += OnLifeValueChanged;
            livesText.text = $"Lives: {GameManager.Instance.lives}";
        }
            
    }


    void SetupSliderInfo(Slider mySlider, TMP_Text sliderText, string parameterName)
    {
        mySlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, sliderText, parameterName, mySlider));
        float newVal = (mySlider.value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(mySlider.value);
        audioMixer.SetFloat(parameterName, newVal);


        if (sliderText)
            sliderText.text = (newVal == -80.0f) ? "0%" : (int)(mySlider.value * 10) + "%";
    }

    void OnSliderValueChanged(float value, TMP_Text volSliderText, string mixerParameterName, Slider mySlider)
    {

        value = (value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(value);
        if (volSliderText)
            volSliderText.text = (value == -80.0f) ? "0%" : (int)(mySlider.value * 10) + "%";

        audioMixer.SetFloat(mixerParameterName, value);
    }

    void OnLifeValueChanged(int value)
    {
        if (livesText)
            livesText.text = $"Lives: {value}";
    }

    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }
    
    void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyUp(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}
