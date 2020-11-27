using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region Menus
    [SerializeField]
    private bool menuOpened = false;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject controlMenu;
    public GameObject mapObject;
    public GameObject UIObject;
    private SavingManager SM;
    private GameManager GM;
    #endregion

    #region Options
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
    public Slider masterVolumeSlider;
    public Text masterVolumeText;
    private float currentMasterVolume;
    public Slider musicVolumeSlider;
    public Text musicVolumeText;
    private float currentMusicVolume;
    public Slider soundFXVolumeSlider;
    public Text soundFXVolumeText;
    private float currentSoundFXVolume;
    //Secret
    public Toggle secretToggle;
    public GameObject secretObject;
    private bool secretBool = false;
    #endregion

    #region UI
    private List<GameObject> TempNewMoons = new List<GameObject>();
    private List<GameObject> TempHalfMoons = new List<GameObject>();
    private List<GameObject> TempCrescentMoons = new List<GameObject>();
    private List<GameObject> TempFullMoons = new List<GameObject>();
    private GameObject player;
    private GameObject playerGraphics;
    public Image healthFillImage;
    public Image[] healthBaubles = new Image[3];
    public Image moonlightFillImage;
    public Image eclipseFillImage;
    public Text eclipseText;
    public GameObject dashObject;
    public Image dashFillImage;
    private float dashCooldown = 0;
    public GameObject eclipseFlourish;
    public Transform currentSelectedMoonTransform;
    public Transform nextMoonTransform;
    public Transform previousMoonTransform;
    public Transform currentMoonTransform;
    public GameObject newMoon;
    public GameObject crescentMoon;
    public GameObject halfMoon;
    public GameObject fullMoon;
    public Text phaseSpawingCooldownText;
    private int swapIndex = -1;
    private bool firstSwap = false;
    #endregion

    private void Start()
    {
        #region Option Functions
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
        #endregion

        #region UI Functions
        player = GameObject.FindGameObjectWithTag("Player");
        playerGraphics = GameObject.FindGameObjectWithTag("PlayerGraphics");
        playerGraphics.SetActive(false);
        eclipseFillImage.gameObject.SetActive(false);
        eclipseText.gameObject.SetActive(false);
        newMoon.SetActive(false);
        crescentMoon.SetActive(false);
        halfMoon.SetActive(false);
        fullMoon.SetActive(false);
        #endregion

        #region Menu Functions
        Time.timeScale = 0;
        SM = FindObjectOfType<SavingManager>();
        GM = FindObjectOfType<GameManager>();
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        secretObject.SetActive(false);
        UIObject.SetActive(false);
        menuOpened = true;
        //player.SetActive(false);

        if (GM.room != null)
        {
            Destroy(GM.room.gameObject);
        }
        #endregion
    }

    private void Update()
    {
        #region Menu Functions
        Pause();

        if (playerGraphics.activeSelf == false)
        {
            player.GetComponent<PlayerController>().Move(new Vector3(0.007f, 0, 0));
        }
        #endregion

        #region UI Functions
        if (!menuOpened)
        {
            HealthUI();
            MoonlightUI();
            DashUI();
            EclipseUI();
            PhasesUI();
        }
        #endregion
    }

    #region Menu Functions
    public void NewGame()
    {
        Time.timeScale = 1;
        StartCoroutine(NewGameEnumerator());
    }

    private IEnumerator NewGameEnumerator()
    {
        ScreenFade.ScreenFader.FadeIn();
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;

        StartGame();
        bool LoadSucceed;
        Debug.Log("Load Binary Default: " + (SM.Load(true, true, "default.bin") ? LoadSucceed = true : LoadSucceed = false));
        if (!LoadSucceed) { Debug.Log("Load Text Default: " + (SM.Load(false, true, "default.bin") ? LoadSucceed = true : LoadSucceed = false)); }
        if (!LoadSucceed) { Debug.Log("Load Failed: Returning to menu."); MainMenu(); Time.timeScale = 1; }

        ScreenFade.ScreenFader.FadeOut();
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;

        if(!LoadSucceed)
        { Time.timeScale = 0; }
    }

    public void Continue()
    {
        Time.timeScale = 1;
        StartCoroutine(ContinueIEnumerator());
    }

    private IEnumerator ContinueIEnumerator()
    {
        ScreenFade.ScreenFader.FadeIn();
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;

        StartGame();
        bool LoadSucceed;
        Debug.Log("Load Binary: " + (SM.Load(true, true) ? LoadSucceed = true : LoadSucceed = false));
        if (!LoadSucceed) { Debug.Log("Load Text: " + (SM.Load(false, true) ? LoadSucceed = true : LoadSucceed = false)); }
        if (!LoadSucceed) { Debug.Log("Load Failed: Returning to menu."); MainMenu(); Time.timeScale = 1; }

        ScreenFade.ScreenFader.FadeOut();
        while (!ScreenFade.ScreenFader.FadeFinished())
            yield return null;

        if (!LoadSucceed)
        { Time.timeScale = 0; }
    }

    public void Restart()
    {
        UnPause();
        player.GetComponent<PlayerController>().TakeDamage(player.GetComponent<PlayerController>().MaxHealth);
    }

    public void StartGame()
    {
        menuOpened = false;
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        secretObject.SetActive(false);
        UIObject.SetActive(true);
        playerGraphics.SetActive(true);

        if (GM.room != null)
        {
            Destroy(GM.room.gameObject);
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 0;
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        secretObject.SetActive(false);
        UIObject.SetActive(false);
        playerGraphics.SetActive(false);

        if (GM.room != null)
        {
            Destroy(GM.room.gameObject);
        }
    }

    public void Pause()
    {
        if (Input.GetAxisRaw("Pause") > 0 && !menuOpened)
        {
            Time.timeScale = 0;
            menuOpened = true;
            OpenPause();
        }
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        menuOpened = false;
        ClosePause();
    }

    public void OpenStart()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenPause()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public  void OpenOptions()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void CloseToMainPause()
    {
        if (!menuOpened)
        {
            MainMenu();
        }
        else
        {
            OpenPause();
        }
    }

    public void OpenControl()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(true);
        mapObject.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenMap()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(true);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    private void ClosePause()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
        mapObject.SetActive(false);
        UIObject.SetActive(true);

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

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        currentMasterVolume = volume;
        masterVolumeText.text = $"{currentMasterVolume + 100}";
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        currentMusicVolume = volume;
        musicVolumeText.text = $"{currentMusicVolume + 100}";
    }

    public void SetSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffectsVolume", volume);
        currentSoundFXVolume = volume;
        soundFXVolumeText.text = $"{currentSoundFXVolume + 100}";
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
        PlayerPrefs.SetFloat("MasterVolume", currentMasterVolume);
        PlayerPrefs.SetFloat("MusicVolume", currentMusicVolume);
        PlayerPrefs.SetFloat("SoundEffectsVolume", currentSoundFXVolume);
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
        if (PlayerPrefs.HasKey("MasterVolume"))
        { masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume"); }
        else
        { masterVolumeSlider.value = currentMasterVolume; }
        if (PlayerPrefs.HasKey("MusicVolume"))
        { musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume"); }
        else
        { musicVolumeSlider.value = currentMusicVolume; }
        if (PlayerPrefs.HasKey("SoundEffectsVolume"))
        { soundFXVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume"); }
        else
        { soundFXVolumeSlider.value = currentSoundFXVolume; }
    }
    #endregion

    #region UI Functions
    public void HealthUI()
    {
        float health = player.GetComponent<PlayerController>().Health;
        int i = 0;

        for (int e = 0; e < healthBaubles.Length; e++)
        {
            healthBaubles[e].color = Color.black;
        }

        while (health > 4)
        {
            health--;
            healthBaubles[i].color = Color.white;
            i++;
        }

        healthFillImage.fillAmount = health / 4;
    }

    public void MoonlightUI()
    {
        moonlightFillImage.fillAmount = player.GetComponent<PlayerController>().MoonLight / 100;
        eclipseText.text = player.GetComponent<PlayerController>().MoonLight.ToString();
    }

    public void DashUI()
    {
        if (player.GetComponent<UnlockTracker>().GetKeyValue("dash"))
        {
            dashObject.SetActive(true);
            dashCooldown += Time.deltaTime;

            if (dashCooldown >= player.GetComponent<PlayerController>().DashCooldown)
            {
                dashCooldown = player.GetComponent<PlayerController>().DashCooldown;
            }
            if (player.GetComponent<PlayerController>().DashTimer == player.GetComponent<PlayerController>().DashCooldown)
            {
                dashCooldown = 0;
            }

            dashFillImage.fillAmount = dashCooldown / player.GetComponent<PlayerController>().DashCooldown;
        }
        else
        {
            dashObject.SetActive(false);
        }
    }

    public void EclipseUI()
    {
        if (player.GetComponent<UnlockTracker>().GetKeyValue("eclipse"))
        {
            eclipseFillImage.gameObject.SetActive(true);
            eclipseText.gameObject.SetActive(true);
            eclipseFillImage.fillAmount = player.GetComponent<PhaseManager>().swapsTillEclipse / 10;
        }
        if (eclipseFillImage.fillAmount >= 1)
        {
            eclipseFlourish.SetActive(true);
        }
        else
        {
            eclipseFlourish.SetActive(false);
        }

        eclipseText.text = $"{100 * eclipseFillImage.fillAmount}%";
    }

    public void PhasesUI()
    {
        if (!firstSwap && swapIndex != player.GetComponent<PhaseManager>().swapToIndex)
        {
            swapIndex = player.GetComponent<PhaseManager>().swapToIndex;
            firstSwap = true;
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
        {
            newMoon.SetActive(true);
            firstSwap = true;
        }
        else
        {
            newMoon.SetActive(false);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
        {
            crescentMoon.SetActive(true);
            firstSwap = true;
        }
        else
        {
            crescentMoon.SetActive(false);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
        {
            halfMoon.SetActive(true);
            firstSwap = true;
        }
        else
        {
            halfMoon.SetActive(false);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
        {
            fullMoon.SetActive(true);
            firstSwap = true;
        }
        else
        {
            fullMoon.SetActive(false);
        }
        if (player.GetComponent<PhaseManager>().everyMoonPhase[0].CooldownTimer >= 0)
        {
            newMoon.GetComponent<Image>().fillAmount = 1 - (player.GetComponent<PhaseManager>().everyMoonPhase[0].CooldownTimer / player.GetComponent<PhaseManager>().everyMoonPhase[0].phaseCooldown);

            foreach (GameObject obj in TempNewMoons)
            {
                obj.GetComponent<Image>().fillAmount = newMoon.GetComponent<Image>().fillAmount;
            }
        }
        if (player.GetComponent<PhaseManager>().everyMoonPhase[1].CooldownTimer >= 0)
        {
            halfMoon.GetComponent<Image>().fillAmount = 1 - (player.GetComponent<PhaseManager>().everyMoonPhase[1].CooldownTimer / player.GetComponent<PhaseManager>().everyMoonPhase[1].phaseCooldown);

            foreach (GameObject obj in TempHalfMoons)
            {
                obj.GetComponent<Image>().fillAmount = halfMoon.GetComponent<Image>().fillAmount;
            }
        }
        if (player.GetComponent<PhaseManager>().everyMoonPhase[2].CooldownTimer >= 0)
        {
            crescentMoon.GetComponent<Image>().fillAmount = 1 - (player.GetComponent<PhaseManager>().everyMoonPhase[2].CooldownTimer / player.GetComponent<PhaseManager>().everyMoonPhase[2].phaseCooldown);

            foreach (GameObject obj in TempCrescentMoons)
            {
                obj.GetComponent<Image>().fillAmount = crescentMoon.GetComponent<Image>().fillAmount;
            }
        }
        if (player.GetComponent<PhaseManager>().everyMoonPhase[3].CooldownTimer >= 0)
        {
            fullMoon.GetComponent<Image>().fillAmount = 1 - (player.GetComponent<PhaseManager>().everyMoonPhase[3].CooldownTimer / player.GetComponent<PhaseManager>().everyMoonPhase[3].phaseCooldown);

            foreach (GameObject obj in TempFullMoons)
            {
                obj.GetComponent<Image>().fillAmount = fullMoon.GetComponent<Image>().fillAmount;
            }
        }

        float phaseCooldown = player.GetComponent<PhaseManager>().CooldownTimer < 0 ? 0 : player.GetComponent<PhaseManager>().CooldownTimer;
        phaseSpawingCooldownText.text = $"Phase Switch: {phaseCooldown}s";

        if (player.GetComponent<PhaseManager>().CooldownTimer < 0 && Input.GetAxis("SelectPhase") > 0)
        {
            if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[0])
            {
                if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 1;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 2;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 3;
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[1])
            {
                if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 2;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 3;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 0;
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[2])
            {
                if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 3;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 0;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 1;
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[3])
            {
                if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 0;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 1;
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    player.GetComponent<PhaseManager>().swapToIndex = 2;
                }
            }
        }
        if (firstSwap)
        {
            foreach (GameObject obj in TempNewMoons)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in TempHalfMoons)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in TempCrescentMoons)
            {
                Destroy(obj);
            }
            foreach (GameObject obj in TempFullMoons)
            {
                Destroy(obj);
            }

            TempNewMoons.Clear();
            TempHalfMoons.Clear();
            TempCrescentMoons.Clear();
            TempFullMoons.Clear();

            if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[0] && player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
            {
                newMoon.transform.position = currentMoonTransform.position;

                if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    GameObject tempCrescentMoon = Instantiate(crescentMoon, previousMoonTransform);
                                    tempCrescentMoon.transform.position = previousMoonTransform.position;
                                    TempCrescentMoons.Add(tempCrescentMoon);
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    GameObject tempHalfMoon = Instantiate(halfMoon, nextMoonTransform);
                                    tempHalfMoon.transform.position = nextMoonTransform.position;
                                    TempHalfMoons.Add(tempHalfMoon);
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 1:
                                halfMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, nextMoonTransform);
                                tempFullMoon.transform.position = nextMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                fullMoon.transform.position = previousMoonTransform.position;
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                halfMoon.transform.position = nextMoonTransform.position;
                                GameObject tempHalfMoon = Instantiate(halfMoon, previousMoonTransform);
                                tempHalfMoon.transform.position = previousMoonTransform.position;
                                TempHalfMoons.Add(tempHalfMoon);
                                break;
                        }
                    }
                    else
                    {
                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempHalfMoon1 = Instantiate(halfMoon, nextMoonTransform);
                        tempHalfMoon1.transform.position = nextMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon1);
                        GameObject tempHalfMoon2 = Instantiate(halfMoon, previousMoonTransform);
                        tempHalfMoon2.transform.position = previousMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 2:
                                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                fullMoon.transform.position = nextMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, previousMoonTransform);
                                tempFullMoon.transform.position = previousMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempCrescentMoon = Instantiate(crescentMoon, nextMoonTransform);
                                tempCrescentMoon.transform.position = nextMoonTransform.position;
                                TempCrescentMoons.Add(tempCrescentMoon);
                                crescentMoon.transform.position = previousMoonTransform.position;
                                break;
                        }
                    }
                    else
                    {
                        crescentMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempCrescentMoon1 = Instantiate(crescentMoon, nextMoonTransform);
                        tempCrescentMoon1.transform.position = nextMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon1);
                        GameObject tempCrescentMoon2 = Instantiate(crescentMoon, previousMoonTransform);
                        tempCrescentMoon2.transform.position = previousMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    GameObject tempFullMoon1 = Instantiate(fullMoon, nextMoonTransform);
                    tempFullMoon1.transform.position = nextMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon1);
                    GameObject tempFullMoon2 = Instantiate(fullMoon, previousMoonTransform);
                    tempFullMoon2.transform.position = previousMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon2);
                }
                else
                {
                    GameObject tempNewMoon1 = Instantiate(newMoon, currentSelectedMoonTransform);
                    tempNewMoon1.transform.position = currentSelectedMoonTransform.position;
                    TempNewMoons.Add(tempNewMoon1);
                    GameObject tempNewMoon2 = Instantiate(newMoon, nextMoonTransform);
                    tempNewMoon2.transform.position = nextMoonTransform.position;
                    TempNewMoons.Add(tempNewMoon2);
                    GameObject tempNewMoon3 = Instantiate(newMoon, previousMoonTransform);
                    tempNewMoon3.transform.position = previousMoonTransform.position;
                    TempNewMoons.Add(tempNewMoon3);
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[1] && player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
            {
                halfMoon.transform.position = currentMoonTransform.position;

                if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    GameObject tempCrescentMoon = Instantiate(crescentMoon, previousMoonTransform);
                                    tempCrescentMoon.transform.position = previousMoonTransform.position;
                                    TempCrescentMoons.Add(tempCrescentMoon);
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    GameObject tempNewMoon = Instantiate(newMoon, nextMoonTransform);
                                    tempNewMoon.transform.position = nextMoonTransform.position;
                                    TempNewMoons.Add(tempNewMoon);
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 0:
                                newMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, nextMoonTransform);
                                tempFullMoon.transform.position = nextMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                fullMoon.transform.position = previousMoonTransform.position;
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                newMoon.transform.position = nextMoonTransform.position;
                                GameObject tempNewMoon = Instantiate(newMoon, previousMoonTransform);
                                tempNewMoon.transform.position = previousMoonTransform.position;
                                TempNewMoons.Add(tempNewMoon);
                                break;
                        }
                    }
                    else
                    {
                        newMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempNewMoon1 = Instantiate(newMoon, nextMoonTransform);
                        tempNewMoon1.transform.position = nextMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon1);
                        GameObject tempNewMoon2 = Instantiate(newMoon, previousMoonTransform);
                        tempNewMoon2.transform.position = previousMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 2:
                                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                fullMoon.transform.position = nextMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon);
                                tempFullMoon.transform.position = previousMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempCrescentMoon = Instantiate(crescentMoon, nextMoonTransform);
                                tempCrescentMoon.transform.position = nextMoonTransform.position;
                                TempCrescentMoons.Add(tempCrescentMoon);
                                crescentMoon.transform.position = previousMoonTransform.position;
                                break;
                        }
                    }
                    else
                    {
                        crescentMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempCrescentMoon1 = Instantiate(crescentMoon, nextMoonTransform);
                        tempCrescentMoon1.transform.position = nextMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon1);
                        GameObject tempCrescentMoon2 = Instantiate(crescentMoon, previousMoonTransform);
                        tempCrescentMoon2.transform.position = previousMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    GameObject tempFullMoon1 = Instantiate(fullMoon, nextMoonTransform);
                    tempFullMoon1.transform.position = nextMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon1);
                    GameObject tempFullMoon2 = Instantiate(fullMoon, previousMoonTransform);
                    tempFullMoon2.transform.position = previousMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon2);
                }
                else
                {
                    GameObject tempHalfMoon1 = Instantiate(halfMoon, currentSelectedMoonTransform);
                    tempHalfMoon1.transform.position = currentSelectedMoonTransform.position;
                    TempHalfMoons.Add(tempHalfMoon1);
                    GameObject tempHalfMoon2 = Instantiate(halfMoon, nextMoonTransform);
                    tempHalfMoon2.transform.position = nextMoonTransform.position;
                    TempHalfMoons.Add(tempHalfMoon2);
                    GameObject tempHalfMoon3 = Instantiate(halfMoon, previousMoonTransform);
                    tempHalfMoon3.transform.position = previousMoonTransform.position;
                    TempHalfMoons.Add(tempHalfMoon3);
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[2] && player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
            {
                crescentMoon.transform.position = currentMoonTransform.position;

                if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    GameObject tempHalfMoon = Instantiate(halfMoon, previousMoonTransform);
                                    tempHalfMoon.transform.position = previousMoonTransform.position;
                                    TempHalfMoons.Add(tempHalfMoon);
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    GameObject tempNewMoon = Instantiate(newMoon, nextMoonTransform);
                                    tempNewMoon.transform.position = nextMoonTransform.position;
                                    TempNewMoons.Add(tempNewMoon);
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 0:
                                newMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, nextMoonTransform);
                                tempFullMoon.transform.position = nextMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                fullMoon.transform.position = previousMoonTransform.position;
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                newMoon.transform.position = nextMoonTransform.position;
                                GameObject tempNewMoon = Instantiate(newMoon, previousMoonTransform);
                                tempNewMoon.transform.position = previousMoonTransform.position;
                                TempNewMoons.Add(tempNewMoon);
                                break;
                        }
                    }
                    else
                    {
                        newMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempNewMoon1 = Instantiate(newMoon, nextMoonTransform);
                        tempNewMoon1.transform.position = nextMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon1);
                        GameObject tempNewMoon2 = Instantiate(newMoon, previousMoonTransform);
                        tempNewMoon2.transform.position = previousMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 1:
                                halfMoon.transform.position = currentSelectedMoonTransform.position;
                                fullMoon.transform.position = nextMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, previousMoonTransform);
                                tempFullMoon.transform.position = previousMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempHalfMoon = Instantiate(halfMoon, nextMoonTransform);
                                tempHalfMoon.transform.position = nextMoonTransform.position;
                                TempHalfMoons.Add(tempHalfMoon);
                                halfMoon.transform.position = previousMoonTransform.position;
                                break;
                        }
                    }
                    else
                    {
                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempHalfMoon1 = Instantiate(halfMoon, nextMoonTransform);
                        tempHalfMoon1.transform.position = nextMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon1);
                        GameObject tempHalfMoon2 = Instantiate(halfMoon, previousMoonTransform);
                        tempHalfMoon2.transform.position = previousMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    GameObject tempFullMoon1 = Instantiate(fullMoon, nextMoonTransform);
                    tempFullMoon1.transform.position = nextMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon1);
                    GameObject tempFullMoon2 = Instantiate(fullMoon, previousMoonTransform);
                    tempFullMoon2.transform.position = previousMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon2);
                }
                else
                {
                    GameObject tempCrescentMoon1 = Instantiate(crescentMoon, currentSelectedMoonTransform);
                    tempCrescentMoon1.transform.position = currentSelectedMoonTransform.position;
                    TempCrescentMoons.Add(tempCrescentMoon1);
                    GameObject tempCrescentMoon2 = Instantiate(crescentMoon, nextMoonTransform);
                    tempCrescentMoon2.transform.position = nextMoonTransform.position;
                    TempCrescentMoons.Add(tempCrescentMoon2);
                    GameObject tempCrescentMoon3 = Instantiate(crescentMoon, previousMoonTransform);
                    tempCrescentMoon3.transform.position = previousMoonTransform.position;
                    TempCrescentMoons.Add(tempCrescentMoon3);
                }
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[3] && player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
            {
                fullMoon.transform.position = currentMoonTransform.position;

                if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    GameObject tempHalfMoon = Instantiate(halfMoon, nextMoonTransform);
                                    tempHalfMoon.transform.position = nextMoonTransform.position;
                                    TempHalfMoons.Add(tempHalfMoon);
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    GameObject tempNewMoon = Instantiate(newMoon, previousMoonTransform);
                                    tempNewMoon.transform.position = previousMoonTransform.position;
                                    TempNewMoons.Add(tempNewMoon);
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 0:
                                newMoon.transform.position = currentSelectedMoonTransform.position;
                                crescentMoon.transform.position = nextMoonTransform.position;
                                GameObject tempCrescentMoon = Instantiate(crescentMoon, previousMoonTransform);
                                tempCrescentMoon.transform.position = previousMoonTransform.position;
                                TempCrescentMoons.Add(tempCrescentMoon);
                                break;
                            case 2:
                                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempNewMoon = Instantiate(newMoon, nextMoonTransform);
                                tempNewMoon.transform.position = nextMoonTransform.position;
                                TempNewMoons.Add(tempNewMoon);
                                newMoon.transform.position = previousMoonTransform.position;
                                break;
                        }
                    }
                    else
                    {
                        newMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempNewMoon1 = Instantiate(newMoon, nextMoonTransform);
                        tempNewMoon1.transform.position = nextMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon1);
                        GameObject tempNewMoon2 = Instantiate(newMoon, previousMoonTransform);
                        tempNewMoon2.transform.position = previousMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 1:
                                halfMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempCrescentMoon = Instantiate(crescentMoon, nextMoonTransform);
                                tempCrescentMoon.transform.position = nextMoonTransform.position;
                                TempCrescentMoons.Add(tempCrescentMoon);
                                crescentMoon.transform.position = previousMoonTransform.position;
                                break;
                            case 2:
                                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                halfMoon.transform.position = nextMoonTransform.position;
                                GameObject tempHalfMoon = Instantiate(halfMoon, previousMoonTransform);
                                tempHalfMoon.transform.position = previousMoonTransform.position;
                                TempHalfMoons.Add(tempHalfMoon);
                                break;
                        }
                    }
                    else
                    {
                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                        GameObject tempHalfMoon1 = Instantiate(halfMoon, nextMoonTransform);
                        tempHalfMoon1.transform.position = nextMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon1);
                        GameObject tempHalfMoon2 = Instantiate(halfMoon, previousMoonTransform);
                        tempHalfMoon2.transform.position = previousMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                    GameObject tempCrescentMoon1 = Instantiate(crescentMoon, nextMoonTransform);
                    tempCrescentMoon1.transform.position = nextMoonTransform.position;
                    TempCrescentMoons.Add(tempCrescentMoon1);
                    GameObject tempCrescentMoon2 = Instantiate(crescentMoon, previousMoonTransform);
                    tempCrescentMoon2.transform.position = previousMoonTransform.position;
                    TempCrescentMoons.Add(tempCrescentMoon2);
                }
                else
                {
                    GameObject tempFullMoon1 = Instantiate(fullMoon, currentSelectedMoonTransform);
                    tempFullMoon1.transform.position = currentSelectedMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon1);
                    GameObject tempFullMoon2 = Instantiate(fullMoon, nextMoonTransform);
                    tempFullMoon2.transform.position = nextMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon2);
                    GameObject tempFullMoon3 = Instantiate(fullMoon, previousMoonTransform);
                    tempFullMoon3.transform.position = previousMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon3);
                }
            }
            else
            {
                if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                        {
                            if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                            {
                                switch (player.GetComponent<PhaseManager>().swapToIndex)
                                {
                                    case 0:
                                        newMoon.transform.position = currentSelectedMoonTransform.position;
                                        halfMoon.transform.position = nextMoonTransform.position;
                                        crescentMoon.transform.position = new Vector3(-100, -100);
                                        fullMoon.transform.position = previousMoonTransform.position;
                                        break;
                                    case 1:
                                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                                        crescentMoon.transform.position = nextMoonTransform.position;
                                        fullMoon.transform.position = new Vector3(-100, -100);
                                        newMoon.transform.position = previousMoonTransform.position;
                                        break;
                                    case 2:
                                        crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                        fullMoon.transform.position = nextMoonTransform.position;
                                        newMoon.transform.position = new Vector3(-100, -100);
                                        halfMoon.transform.position = previousMoonTransform.position;
                                        break;
                                    case 3:
                                        fullMoon.transform.position = currentSelectedMoonTransform.position;
                                        newMoon.transform.position = nextMoonTransform.position;
                                        halfMoon.transform.position = new Vector3(-100, -100);
                                        crescentMoon.transform.position = previousMoonTransform.position;
                                        break;
                                }
                            }
                            else
                            {
                                switch (player.GetComponent<PhaseManager>().swapToIndex)
                                {
                                    case 0:
                                        newMoon.transform.position = currentSelectedMoonTransform.position;
                                        halfMoon.transform.position = nextMoonTransform.position;
                                        crescentMoon.transform.position = previousMoonTransform.position;
                                        fullMoon.transform.position = new Vector3(-100, -100);
                                        break;
                                    case 1:
                                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                                        crescentMoon.transform.position = nextMoonTransform.position;
                                        fullMoon.transform.position = new Vector3(-100, -100);
                                        newMoon.transform.position = previousMoonTransform.position;
                                        break;
                                    case 2:
                                        crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                        fullMoon.transform.position = new Vector3(-100, -100);
                                        newMoon.transform.position = nextMoonTransform.position;
                                        halfMoon.transform.position = previousMoonTransform.position;
                                        break;
                                }
                            }
                        }
                        else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = new Vector3(-100, -100);
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = new Vector3(-100, -100);
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    crescentMoon.transform.position = new Vector3(-100, -100);
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = new Vector3(-100, -100);
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    GameObject tempHalfMoon = Instantiate(halfMoon, previousMoonTransform);
                                    tempHalfMoon.transform.position = previousMoonTransform.position;
                                    TempHalfMoons.Add(tempHalfMoon);
                                    break;
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    GameObject tempNewMoon = Instantiate(newMoon, nextMoonTransform);
                                    tempNewMoon.transform.position = nextMoonTransform.position;
                                    TempNewMoons.Add(tempNewMoon);
                                    crescentMoon.transform.position = new Vector3(-100, -100);
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    newMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = new Vector3(-100, -100);
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = previousMoonTransform.position;
                                    halfMoon.transform.position = new Vector3(-100, -100);
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = new Vector3(-100, -100);
                                    crescentMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 0:
                                    newMoon.transform.position = currentSelectedMoonTransform.position;
                                    halfMoon.transform.position = new Vector3(-100, -100);
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    GameObject tempCrescentMoon = Instantiate(crescentMoon, previousMoonTransform);
                                    tempCrescentMoon.transform.position = previousMoonTransform.position;
                                    TempCrescentMoons.Add(tempCrescentMoon);
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    newMoon.transform.position = nextMoonTransform.position;
                                    halfMoon.transform.position = new Vector3(-100, -100);
                                    GameObject tempNewMoon = Instantiate(newMoon, previousMoonTransform);
                                    tempNewMoon.transform.position = previousMoonTransform.position;
                                    TempNewMoons.Add(tempNewMoon);
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 0:
                                newMoon.transform.position = currentSelectedMoonTransform.position;
                                GameObject tempFullMoon = Instantiate(fullMoon, nextMoonTransform);
                                tempFullMoon.transform.position = nextMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                halfMoon.transform.position = new Vector3(-100, -100);
                                crescentMoon.transform.position = new Vector3(-100, -100);
                                fullMoon.transform.position = previousMoonTransform.position;
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                newMoon.transform.position = nextMoonTransform.position;
                                halfMoon.transform.position = new Vector3(-100, -100);
                                crescentMoon.transform.position = new Vector3(-100, -100);
                                GameObject tempNewMoon = Instantiate(newMoon, previousMoonTransform);
                                tempNewMoon.transform.position = previousMoonTransform.position;
                                TempNewMoons.Add(tempNewMoon);
                                break;
                        }
                    }
                    else
                    {
                        newMoon.transform.position = currentSelectedMoonTransform.position;
                        halfMoon.transform.position = new Vector3(-100, -100);
                        crescentMoon.transform.position = new Vector3(-100, -100);
                        fullMoon.transform.position = new Vector3(-100, -100);
                        GameObject tempNewMoon1 = Instantiate(newMoon, nextMoonTransform);
                        tempNewMoon1.transform.position = nextMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon1);
                        GameObject tempNewMoon2 = Instantiate(newMoon, previousMoonTransform);
                        tempNewMoon2.transform.position = previousMoonTransform.position;
                        TempNewMoons.Add(tempNewMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                    {
                        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = previousMoonTransform.position;
                                    newMoon.transform.position = new Vector3(-100, -100);
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = nextMoonTransform.position;
                                    newMoon.transform.position = new Vector3(-100, -100);
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                                case 3:
                                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                                    newMoon.transform.position = new Vector3(-100, -100);
                                    halfMoon.transform.position = nextMoonTransform.position;
                                    crescentMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                        else
                        {
                            switch (player.GetComponent<PhaseManager>().swapToIndex)
                            {
                                case 1:
                                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                                    crescentMoon.transform.position = nextMoonTransform.position;
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    newMoon.transform.position = new Vector3(-100, -100);
                                    GameObject tempCrescentmoon = Instantiate(crescentMoon, previousMoonTransform);
                                    tempCrescentmoon.transform.position = previousMoonTransform.position;
                                    TempCrescentMoons.Add(tempCrescentmoon);
                                    break;
                                case 2:
                                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                    fullMoon.transform.position = new Vector3(-100, -100);
                                    GameObject tempHalfMoon = Instantiate(halfMoon, nextMoonTransform);
                                    tempHalfMoon.transform.position = nextMoonTransform.position;
                                    TempHalfMoons.Add(tempHalfMoon);
                                    newMoon.transform.position = new Vector3(-100, -100);
                                    halfMoon.transform.position = previousMoonTransform.position;
                                    break;
                            }
                        }
                    }
                    else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 1:
                                halfMoon.transform.position = currentSelectedMoonTransform.position;
                                crescentMoon.transform.position = new Vector3(-100, -100);
                                GameObject tempFullMoon = Instantiate(fullMoon, nextMoonTransform);
                                tempFullMoon.transform.position = nextMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                fullMoon.transform.position = previousMoonTransform.position;
                                newMoon.transform.position = new Vector3(-100, -100);
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                newMoon.transform.position = new Vector3(-100, -100);
                                halfMoon.transform.position = nextMoonTransform.position;
                                crescentMoon.transform.position = new Vector3(-100, -100);
                                GameObject tempHalfMoon = Instantiate(halfMoon, previousMoonTransform);
                                tempHalfMoon.transform.position = previousMoonTransform.position;
                                TempHalfMoons.Add(tempHalfMoon);
                                break;
                        }
                    }
                    else
                    {
                        halfMoon.transform.position = currentSelectedMoonTransform.position;
                        crescentMoon.transform.position = new Vector3(-100, -100);
                        fullMoon.transform.position = new Vector3(-100, -100);
                        newMoon.transform.position = new Vector3(-100, -100);
                        GameObject tempHalfMoon1 = Instantiate(halfMoon, nextMoonTransform);
                        tempHalfMoon1.transform.position = nextMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon1);
                        GameObject tempHalfMoon2 = Instantiate(halfMoon, previousMoonTransform);
                        tempHalfMoon2.transform.position = previousMoonTransform.position;
                        TempHalfMoons.Add(tempHalfMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
                {
                    if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                    {
                        switch (player.GetComponent<PhaseManager>().swapToIndex)
                        {
                            case 2:
                                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                                fullMoon.transform.position = nextMoonTransform.position;
                                newMoon.transform.position = new Vector3(-100, -100);
                                halfMoon.transform.position = new Vector3(-100, -100);
                                GameObject tempFullMoon = Instantiate(fullMoon, previousMoonTransform);
                                tempFullMoon.transform.position = previousMoonTransform.position;
                                TempFullMoons.Add(tempFullMoon);
                                break;
                            case 3:
                                fullMoon.transform.position = currentSelectedMoonTransform.position;
                                newMoon.transform.position = new Vector3(-100, -100);
                                GameObject tempCrescentMoon = Instantiate(crescentMoon, nextMoonTransform);
                                tempCrescentMoon.transform.position = nextMoonTransform.position;
                                TempCrescentMoons.Add(tempCrescentMoon);
                                halfMoon.transform.position = new Vector3(-100, -100);
                                crescentMoon.transform.position = previousMoonTransform.position;
                                break;
                        }
                    }
                    else
                    {
                        crescentMoon.transform.position = currentSelectedMoonTransform.position;
                        fullMoon.transform.position = new Vector3(-100, -100);
                        newMoon.transform.position = new Vector3(-100, -100);
                        halfMoon.transform.position = new Vector3(-100, -100);
                        GameObject tempCrescentMoon1 = Instantiate(crescentMoon, nextMoonTransform);
                        tempCrescentMoon1.transform.position = nextMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon1);
                        GameObject tempCrescentMoon2 = Instantiate(crescentMoon, previousMoonTransform);
                        tempCrescentMoon2.transform.position = previousMoonTransform.position;
                        TempCrescentMoons.Add(tempCrescentMoon2);
                    }
                }
                else if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
                {
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    newMoon.transform.position = new Vector3(-100, -100);
                    halfMoon.transform.position = new Vector3(-100, -100);
                    crescentMoon.transform.position = new Vector3(-100, -100);
                    GameObject tempFullMoon1 = Instantiate(fullMoon, nextMoonTransform);
                    tempFullMoon1.transform.position = nextMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon1);
                    GameObject tempFullMoon2 = Instantiate(fullMoon, previousMoonTransform);
                    tempFullMoon2.transform.position = previousMoonTransform.position;
                    TempFullMoons.Add(tempFullMoon2);
                }
            }

            firstSwap = false;
        }
    }
    #endregion
}
