using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Menus
    [SerializeField]
    private bool menuOpened = false;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject controlMenu;
    public GameObject background;
    #endregion

    #region Options
    public string gameSceneName;
    public AudioMixer audioMixer;
    private Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Dropdown presetQualityDropdown;
    //public Slider pixelLightCountSlider;
    //public Text pixelLightCountText;
    public Dropdown textureQualityDropdown;
    public Dropdown anisotropicTexturesDropdown;
    //public Dropdown antiAliasingDropdown;
    //public Toggle softParticlesToogle;
    public Toggle realtimeReflectionProbesToggle;
    public Toggle textureStreamingToggle;
    public Slider volumeSlider;
    public Text volumeText;
    private float currentVolume;
    //Secret
    public Toggle secretToggle;
    public GameObject secretObject;
    private bool secretBool = false;
    #endregion

    #region UI
    public Slider healthSlider;
    public Slider moonlightSlider;
    public Slider eclipseSlider;
    public Slider newMoonCooldownSlider;
    public Slider crescentMoonCooldownSlider;
    public Slider halfMoonCooldownSlider;
    public Slider fullMoonCooldownSlider;
    public Text healthText;
    public Text moonlightText;
    public Text eclipseText;
    public GameObject nextPhase;
    public GameObject previousPhase;
    public GameObject eclipse;
    public GameObject newMoon;
    public GameObject crescentMoon;
    public GameObject halfMoon;
    public GameObject fullMoon;
    #endregion

    #region Menu Functions
    private void Update()
    {
        Pause();
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        LoadScene(gameSceneName);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        LoadScene("MainMenu");
    }

    private void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Pause()
    {
        if (Input.GetAxisRaw("Pause") > 0 && !menuOpened)
        {
            Time.timeScale = 0;
            menuOpened = true;
            OpenMain();
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        menuOpened = false;
        CloseMenu();
    }

    public void OpenMain()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        background.SetActive(true);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        controlMenu.SetActive(false);
        background.SetActive(true);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenControl()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(true);
        background.SetActive(true);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        background.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region Option Functions
    private void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
        secretToggle.SetIsOnWithoutNotify(false);
        secretObject.SetActive(false);
    }

    public void SetDropdownHelper(int dropdownIndex, Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(7);
        presetQualityDropdown.value = 7;
        dropdown.value = dropdownIndex;
    }

    public void SetToggleHelper(bool toggleBool, Toggle toggle)
    {
        QualitySettings.SetQualityLevel(7);
        presetQualityDropdown.value = 7;
        toggle.SetIsOnWithoutNotify(toggleBool);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        resolutionDropdown.value = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        fullScreenToggle.SetIsOnWithoutNotify(isFullScreen);
        Screen.fullScreen = isFullScreen;
    }

    public void SetPresetQuality(int qualityIndex)
    {
        if (qualityIndex < 7)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            //pixelLightCountSlider.value = QualitySettings.pixelLightCount;
            //pixelLightCountText.text = $"{QualitySettings.pixelLightCount}";
            textureQualityDropdown.value = QualitySettings.masterTextureLimit;
            anisotropicTexturesDropdown.value = Convert.ToInt32(QualitySettings.anisotropicFiltering);
            //antiAliasingDropdown.value = QualitySettings.antiAliasing;
            //softParticlesToogle.SetIsOnWithoutNotify(QualitySettings.softParticles);
            realtimeReflectionProbesToggle.SetIsOnWithoutNotify(QualitySettings.realtimeReflectionProbes);
            textureStreamingToggle.SetIsOnWithoutNotify(QualitySettings.streamingMipmapsActive);
        }

        presetQualityDropdown.value = qualityIndex;
    }

    /*public void SetPixelLightCount(float plcIndex)
    {
        QualitySettings.SetQualityLevel(6);
        presetQualityDropdown.value = 6;
        pixelLightCountSlider.value = plcIndex;
        pixelLightCountText.text = $"{plcIndex}";
        QualitySettings.pixelLightCount = (int)plcIndex;
    }*/

    public void SetTextureQuality(int textureIndex)
    {
        SetDropdownHelper(textureIndex, textureQualityDropdown);
        QualitySettings.masterTextureLimit = textureIndex;
    }

    public void SetAnisotropicTextures(int atIndex)
    {
        if (atIndex < 3)
        {
            SetDropdownHelper(atIndex, anisotropicTexturesDropdown);

            if (atIndex == 0)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            }
            else if (atIndex == 1)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
            }
            else if (atIndex == 2)
            {
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
            }
        }
    }

    /*public void SetAntiAliasing(int aaIndex)
    {
        SetDropdownHelper(aaIndex, antiAliasingDropdown);
        QualitySettings.antiAliasing = aaIndex;
    }*/

    /*public void SetSoftParticles(bool isSoftParticles)
    {
        QualitySettings.SetQualityLevel(6);
        presetQualityDropdown.value = 6;
        softParticlesToogle.SetIsOnWithoutNotify(isSoftParticles);
        QualitySettings.softParticles = isSoftParticles;
    }*/

    public void SetRealtimeReflectionProbes(bool isRealtimeReflectionProbes)
    {
        SetToggleHelper(isRealtimeReflectionProbes, realtimeReflectionProbesToggle);
        QualitySettings.realtimeReflectionProbes = isRealtimeReflectionProbes;
    }

    public void SetTextureStreaming(bool isTextureStreaming)
    {
        SetToggleHelper(isTextureStreaming, textureStreamingToggle);
        QualitySettings.streamingMipmapsActive = isTextureStreaming;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
        volumeText.text = $"{currentVolume + 100}";
    }

    public void SetSecret(bool isSecret)
    {
        secretBool = isSecret;
        secretToggle.SetIsOnWithoutNotify(isSecret);
        secretObject.SetActive(isSecret);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullScreen", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetInt("PresetQuality", presetQualityDropdown.value);
        //PlayerPrefs.SetFloat("PixelLightCount", pixelLightCountSlider.value);
        PlayerPrefs.SetInt("TextureQuality", textureQualityDropdown.value);
        PlayerPrefs.SetInt("AnisotropicTextures", anisotropicTexturesDropdown.value);
        //PlayerPrefs.SetInt("AntiAliasing", antiAliasingDropdown.value);
        //PlayerPrefs.SetInt("SoftParticles", Convert.ToInt32(QualitySettings.softParticles));
        PlayerPrefs.SetInt("RealtimeReflectionProbes", Convert.ToInt32(QualitySettings.realtimeReflectionProbes));
        PlayerPrefs.SetInt("TextureStreaming", Convert.ToInt32(QualitySettings.streamingMipmapsActive));
        PlayerPrefs.SetFloat("Volume", currentVolume);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("Resolution")) 
        { resolutionDropdown.value = PlayerPrefs.GetInt("Resolution"); }
        else 
        { resolutionDropdown.value = currentResolutionIndex; }
        if (PlayerPrefs.HasKey("FullScreen")) 
        { fullScreenToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen"))); }
        else 
        { fullScreenToggle.SetIsOnWithoutNotify(Screen.fullScreen); }
        if (PlayerPrefs.HasKey("PresetQuality"))
        { presetQualityDropdown.value = PlayerPrefs.GetInt("PresetQuality"); }
        else
        { presetQualityDropdown.value = 3; }
        /*if (PlayerPrefs.HasKey("PixelLightCount"))
        {
            pixelLightCountSlider.value = PlayerPrefs.GetFloat("PixelLightCount");
        }
        else
        {
            pixelLightCountSlider.value = QualitySettings.pixelLightCount;
        }*/
        if (PlayerPrefs.HasKey("TextureQuality"))
        { textureQualityDropdown.value = PlayerPrefs.GetInt("TextureQuality"); }
        else
        { textureQualityDropdown.value = 0; }
        if (PlayerPrefs.HasKey("AnisotropicTextures"))
        { anisotropicTexturesDropdown.value = PlayerPrefs.GetInt("AnisotropicTextures"); }
        else
        { anisotropicTexturesDropdown.value = 0; }
        /*if (PlayerPrefs.HasKey("AntiAliasing"))
        { antiAliasingDropdown.value = PlayerPrefs.GetInt("AntiAliasing"); }
        else
        { antiAliasingDropdown.value = QualitySettings.antiAliasing; }*/
        /*if (PlayerPrefs.HasKey("SoftParticles"))
        {
            softParticlesToogle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt("SoftParticles")));
        }
        else
        {
            softParticlesToogle.SetIsOnWithoutNotify(QualitySettings.softParticles);
        }*/
        if (PlayerPrefs.HasKey("RealtimeReflectionProbes"))
        { realtimeReflectionProbesToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt("RealtimeReflectionProbes"))); }
        else
        { realtimeReflectionProbesToggle.SetIsOnWithoutNotify(QualitySettings.realtimeReflectionProbes); }
        if (PlayerPrefs.HasKey("TextureStreaming"))
        { textureStreamingToggle.SetIsOnWithoutNotify(Convert.ToBoolean(PlayerPrefs.GetInt("TextureStreaming"))); }
        else
        { textureStreamingToggle.SetIsOnWithoutNotify(QualitySettings.streamingMipmapsActive); }
        if (PlayerPrefs.HasKey("Volume"))
        { volumeSlider.value = PlayerPrefs.GetFloat("Volume"); }
        else
        { volumeSlider.value = currentVolume; }
    }
    #endregion

    #region UI Functions
    
    #endregion
}
