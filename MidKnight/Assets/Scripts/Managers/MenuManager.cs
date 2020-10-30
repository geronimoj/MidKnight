using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private bool menuOpened = false;
    public string gameSceneName;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject controlMenu;
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    //public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
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
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        Pause();
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        LoadScene(gameSceneName);
    }

    public void LoadGame()
    {

    }

    public void LoadScene(string scene)
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
        Debug.Log("Open Menu");
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        Debug.Log("Open Options");
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void OpenControl()
    {
        Debug.Log("Open Controls");
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log($"Resolution: {Screen.width} x {Screen.height}");
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log($"Quality: {QualitySettings.GetQualityLevel()}");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
