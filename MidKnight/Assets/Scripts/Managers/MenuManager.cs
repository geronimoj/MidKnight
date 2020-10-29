﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private bool menuOpened = false;
    public string gameSceneName;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject controlMenu;

    public void Update()
    {
        Pause();
    }

    public void NewGame()
    {
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
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        controlMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void OpenControl()
    {
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

    public void Quit()
    {
        Application.Quit();
    }
}