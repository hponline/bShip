using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public GameObject gameOverPanel;

    [Header("Variables")]
    public bool isGameOver;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void Resume()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
    }
}
