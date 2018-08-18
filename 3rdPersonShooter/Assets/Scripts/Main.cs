using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public static int CURRENT_STAGE = 1; //every 1 minute of survival increases the Level
    public static bool GAME_OVER = false;
    public static float GAME_TIMER = 0;
    public static int GAME_SCORE;
    private static Main _instance;

    [Tooltip ("Every interval, we increase the Stage difficulty")]
    public float StageIncrement_Interval = 30;

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
        CURRENT_STAGE = (int)Mathf.Floor(GAME_TIMER / StageIncrement_Interval) + 1; 

        hud.UpdateLevelText();
        hud.UpdateTimerField();

        //decrease the enemies spawn interval with increase to stage level
        spawnManager.SPAWN_ENEMIES_INTERVAL = spawnManager.MAX_SPAWN_INTERVAL - CURRENT_STAGE;
        spawnManager.SPAWN_ENEMIES_INTERVAL = Mathf.Clamp(spawnManager.SPAWN_ENEMIES_INTERVAL, 0, spawnManager.MAX_SPAWN_INTERVAL);
    }

    public void RestartGame()
    {
        Debug.Log("RESTARTING GAME");
        CURRENT_STAGE = 1;
        GAME_OVER = false;
        GAME_TIMER = 0;
        GAME_SCORE = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
