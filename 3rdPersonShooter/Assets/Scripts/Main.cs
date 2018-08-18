using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static int CURRENT_LEVEL = 1; //every 1 minute of survival increases the Level
    public static bool GAME_OVER = false;
    public static float GAME_TIMER = 0;
    public static int GAME_SCORE;
    private static Main _instance;

    public PlayerManager currentPlayer;
    public SpawnManager spawnManager;
    public HUDManager hud;

    private void Awake()
    {
        _instance = this;
    }
   
    public static Main GetInstance()
    {
        return _instance;
    }

    private void Update()
    {
        GAME_TIMER += Time.deltaTime;
        CURRENT_LEVEL = (int)Mathf.Floor(GAME_TIMER / 60f) + 1;

        hud.UpdateLevelText();
        hud.UpdateTimerField();
    }

    public void RestartGame()
    {
        Debug.Log("RESTARTING GAME");
    }
}
