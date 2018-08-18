
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Image healthBar;
    public Image[] chevrons;
    public Text chevronText;
    public Text LevelText;
    public Text TimerText;
    public Text ScoreText;
    public PlayerManager player;
    public GameObject gameOverPanel;

    private void Awake()
    {
        UpdateLevelText();
        gameOverPanel.SetActive(false);
        UpdateScore(0);
    }

    public void UpdateHealthBar(float currentHealth, float TotalHealth)
    {
        healthBar.fillAmount = currentHealth / TotalHealth;
    }

    public void UpdateLevelText()
    {
        LevelText.text = Main.CURRENT_STAGE.ToString();
    }

    public void UpdateTimerField()
    {
        TimerText.text = Utils.ConvertMilliToTimeString((long)(Main.GAME_TIMER * 1000f));
    }

    public void UpdateScore(int score)
    {
        Main.GAME_SCORE += score;
        ScoreText.text = Main.GAME_SCORE.ToString();
    }

    public void UpdateChevron()
    {
        for(int i=0; i<chevrons.Length; i++)
        {
            if (i < player.speedMult)
            {
                chevrons[i].gameObject.SetActive(true);
            }
            else
            {
                chevrons[i].gameObject.SetActive(false);
            }
        }

        chevronText.text = "Speed X" + player.speedMult;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

}
