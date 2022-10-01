using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using IslandEscape.Entities;
using IslandEscape.Map;
using IslandEscape.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public UIManager ui;
    public GameObject terrain;
    public WaterManager waterManager;
    public Player player;

    // When the game was started
    // TODO: actually track elapsed time instead of relying on timeSinceLevelLoad - startTime
    public float startTime;

    // Whether or not the game is paused
    public bool gamePaused = true;

    // At what speed the game is moving
    public float GameSpeed { get { return 1.0f; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    private void InitGame()
    {
        StartGame();
    }

    public void StartGame()
    {
        gamePaused = false;
        startTime = Time.timeSinceLevelLoad;
    }

}

