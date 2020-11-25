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
    private bool menuOpened = true;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject optionsPauseMenu;
    public GameObject optionsStartMenu;
    public GameObject controlPauseMenu;
    public GameObject controlStartMenu;
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
    public Slider volumeSlider;
    public Text volumeText;
    private float currentVolume;
    //Secret
    public Toggle secretToggle;
    public GameObject secretObject;
    private bool secretBool = false;
    #endregion

    #region UI
    private GameObject player;
    private GameObject playerGraphics;
    public Image healthFillImage;
    public Image[] healthBaubles = new Image[3];
    public Image moonlightFillImage;
    public Text moonlightText;
    public Image eclipseFillImage;
    public Text eclipseText;
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
    private float newMoonCooldown = 0;
    private float halfMoonCooldown = 0;
    private float crescentMoonCooldown = 0;
    private float fullMoonCooldown = 0;
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
        optionsPauseMenu.SetActive(false);
        optionsStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
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
            //MoonlightUI();
            EclipseUI();
            PhasesUI();
        }
        #endregion
    }

    #region Menu Functions
    public void NewGame()
    {
        StartGame();
        bool LoadSucceed;
        Debug.Log("Load Binary Default: " + (SM.Load(true, true, "default.bin") ? LoadSucceed = true : LoadSucceed = false));
        if (!LoadSucceed) { Debug.Log("Load Text Default: " + SM.Load(false, true, "default.bin")); }
    }

    public void Continue()
    {
        StartGame();
        bool LoadSucceed;
        Debug.Log("Load Binary: " + (SM.Load(true, true) ? LoadSucceed = true : LoadSucceed = false));
        if (!LoadSucceed) { Debug.Log("Load Text: " + SM.Load(false, true)); }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        menuOpened = false;
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
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
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
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
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
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
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenPauseOptions()
    {
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(true);
        OpenOptions();
    }

    public void OpenStartOptions()
    {
        optionsStartMenu.SetActive(true);
        optionsPauseMenu.SetActive(false);
        OpenOptions();
    }

    private void OpenOptions()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
        UIObject.SetActive(false);

        if (secretBool)
        {
            secretObject.SetActive(true);
        }
    }

    public void OpenPauseControl()
    {
        controlStartMenu.SetActive(true);
        controlPauseMenu.SetActive(false);
        OpenControl();
    }

    public void OpenStartControl()
    {
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(true);
        OpenControl();
    }

    private void OpenControl()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
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
        optionsStartMenu.SetActive(false);
        optionsPauseMenu.SetActive(false);
        controlStartMenu.SetActive(false);
        controlPauseMenu.SetActive(false);
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
        eclipseText.text = $"{player.GetComponent<PlayerController>().MoonLight}";
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
        if (player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
        {
            newMoon.SetActive(true);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
        {
            crescentMoon.SetActive(true);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
        {
            halfMoon.SetActive(true);
        }
        if (player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
        {
            fullMoon.SetActive(true);
        }
        if (newMoonCooldown <= player.GetComponent<PhaseManager>().everyMoonPhase[0].phaseCooldown)
        {
            newMoon.GetComponent<Image>().fillAmount = newMoonCooldown / player.GetComponent<PhaseManager>().everyMoonPhase[0].phaseCooldown;
        }
        if (crescentMoonCooldown <= player.GetComponent<PhaseManager>().everyMoonPhase[1].phaseCooldown)
        {
            crescentMoon.GetComponent<Image>().fillAmount = crescentMoonCooldown / player.GetComponent<PhaseManager>().everyMoonPhase[1].phaseCooldown;
        }
        if (halfMoonCooldown <= player.GetComponent<PhaseManager>().everyMoonPhase[2].phaseCooldown)
        {
            halfMoon.GetComponent<Image>().fillAmount = halfMoonCooldown / player.GetComponent<PhaseManager>().everyMoonPhase[2].phaseCooldown;
        }
        if (fullMoonCooldown <= player.GetComponent<PhaseManager>().everyMoonPhase[3].phaseCooldown)
        {
            fullMoon.GetComponent<Image>().fillAmount = fullMoonCooldown / player.GetComponent<PhaseManager>().everyMoonPhase[3].phaseCooldown;
        }

        newMoonCooldown += Time.deltaTime;
        crescentMoonCooldown += Time.deltaTime;
        halfMoonCooldown += Time.deltaTime;
        fullMoonCooldown += Time.deltaTime;
        float phaseCooldown = player.GetComponent<PhaseManager>().CooldownTimer < 0 ? 0 : player.GetComponent<PhaseManager>().CooldownTimer;
        phaseSpawingCooldownText.text = $"Phase Switch: {phaseCooldown}s";

        if (player.GetComponent<PhaseManager>().CooldownTimer < 0 && Input.GetAxis("SelectPhase") > 0)
        {
            if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[0] && newMoonCooldown > player.GetComponent<PhaseManager>().everyMoonPhase[0].phaseCooldown)
            {
                newMoon.transform.position = currentMoonTransform.position;
                crescentMoon.transform.position = nextMoonTransform.position;
                halfMoon.transform.position = currentSelectedMoonTransform.position;
                fullMoon.transform.position = previousMoonTransform.position;
                newMoonCooldown = 0;
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[1] && crescentMoonCooldown > player.GetComponent<PhaseManager>().everyMoonPhase[1].phaseCooldown)
            {
                crescentMoon.transform.position = currentMoonTransform.position;
                halfMoon.transform.position = nextMoonTransform.position;
                fullMoon.transform.position = currentSelectedMoonTransform.position;
                newMoon.transform.position = previousMoonTransform.position;
                crescentMoonCooldown = 0;
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[2] && halfMoonCooldown > player.GetComponent<PhaseManager>().everyMoonPhase[2].phaseCooldown)
            {
                halfMoon.transform.position = currentMoonTransform.position;
                fullMoon.transform.position = nextMoonTransform.position;
                newMoon.transform.position = currentSelectedMoonTransform.position;
                crescentMoon.transform.position = previousMoonTransform.position;
                halfMoonCooldown = 0;
            }
            else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[3] && fullMoonCooldown > player.GetComponent<PhaseManager>().everyMoonPhase[3].phaseCooldown)
            {
                fullMoon.transform.position = currentMoonTransform.position;
                newMoon.transform.position = nextMoonTransform.position;
                crescentMoon.transform.position = currentSelectedMoonTransform.position;
                halfMoon.transform.position = previousMoonTransform.position;
                fullMoonCooldown = 0;
            }
        }
        if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[0] && player.GetComponent<UnlockTracker>().GetKeyValue("new moon"))
        {
            switch (player.GetComponent<PhaseManager>().swapToIndex)
            {
                case 0:
                    if (player.GetComponent<PhaseManager>().KnownPhases.Count > 2)
                    {
                        player.GetComponent<PhaseManager>().swapToIndex++;
                    }
                    break;
                case 1:
                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                    halfMoon.transform.position = nextMoonTransform.position;
                    fullMoon.transform.position = previousMoonTransform.position;
                    break;
                case 2:
                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                    fullMoon.transform.position = nextMoonTransform.position;
                    crescentMoon.transform.position = previousMoonTransform.position;
                    break;
                case 3:
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    crescentMoon.transform.position = nextMoonTransform.position;
                    halfMoon.transform.position = previousMoonTransform.position;
                    break;
            }
        }
        else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[1] && player.GetComponent<UnlockTracker>().GetKeyValue("crescent"))
        {
            switch (player.GetComponent<PhaseManager>().swapToIndex)
            {
                case 0:
                    newMoon.transform.position = currentSelectedMoonTransform.position;
                    halfMoon.transform.position = nextMoonTransform.position;
                    fullMoon.transform.position = previousMoonTransform.position;
                    break;
                case 1:
                    if (player.GetComponent<PhaseManager>().KnownPhases.Count > 3)
                    {
                        player.GetComponent<PhaseManager>().swapToIndex++;
                    }
                    break;
                case 2:
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
        else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[2] && player.GetComponent<UnlockTracker>().GetKeyValue("half moon"))
        {
            switch (player.GetComponent<PhaseManager>().swapToIndex)
            {
                case 0:
                    newMoon.transform.position = currentSelectedMoonTransform.position;
                    crescentMoon.transform.position = nextMoonTransform.position;
                    fullMoon.transform.position = previousMoonTransform.position;
                    break;
                case 1:
                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                    fullMoon.transform.position = nextMoonTransform.position;
                    newMoon.transform.position = previousMoonTransform.position;
                    break;
                case 2:
                    if (player.GetComponent<PhaseManager>().KnownPhases.Count > 4)
                    {
                        player.GetComponent<PhaseManager>().swapToIndex++;
                    }
                    break;
                case 3:
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    newMoon.transform.position = nextMoonTransform.position;
                    crescentMoon.transform.position = previousMoonTransform.position;
                    break;
            }
        }
        else if (player.GetComponent<PhaseManager>().CurrentPhase == player.GetComponent<PhaseManager>().everyMoonPhase[3] && player.GetComponent<UnlockTracker>().GetKeyValue("full moon"))
        {
            switch (player.GetComponent<PhaseManager>().swapToIndex)
            {
                case 0:
                    newMoon.transform.position = currentSelectedMoonTransform.position;
                    crescentMoon.transform.position = nextMoonTransform.position;
                    halfMoon.transform.position = previousMoonTransform.position;
                    break;
                case 1:
                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                    halfMoon.transform.position = nextMoonTransform.position;
                    newMoon.transform.position = previousMoonTransform.position;
                    break;
                case 2:
                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                    newMoon.transform.position = nextMoonTransform.position;
                    crescentMoon.transform.position = previousMoonTransform.position;
                    break;
                case 3:
                    if (player.GetComponent<PhaseManager>().KnownPhases.Count > 5)
                    {
                        player.GetComponent<PhaseManager>().swapToIndex = 0;
                    }
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
                    halfMoon.transform.position = currentMoonTransform.position;
                    fullMoon.transform.position = previousMoonTransform.position;
                    break;
                case 1:
                    crescentMoon.transform.position = currentSelectedMoonTransform.position;
                    halfMoon.transform.position = nextMoonTransform.position;
                    fullMoon.transform.position = currentMoonTransform.position;
                    newMoon.transform.position = previousMoonTransform.position;
                    break;
                case 2:
                    halfMoon.transform.position = currentSelectedMoonTransform.position;
                    fullMoon.transform.position = nextMoonTransform.position;
                    newMoon.transform.position = currentMoonTransform.position;
                    crescentMoon.transform.position = previousMoonTransform.position;
                    break;
                case 3:
                    fullMoon.transform.position = currentSelectedMoonTransform.position;
                    newMoon.transform.position = nextMoonTransform.position;
                    crescentMoon.transform.position = currentMoonTransform.position;
                    halfMoon.transform.position = previousMoonTransform.position;
                    break;
            }
        }
    }
    #endregion
}
