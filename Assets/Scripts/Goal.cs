using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Goal : MonoBehaviour
{
    public static bool LevelIsFinished = false;

    public GameObject GoalMenuUI;
    public GameObject GameCanvas;

    [SerializeField] GameObject CoinManager;
    [SerializeField] GameObject RankManager;
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        LevelIsFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (!LevelIsFinished)
        {
            Time.timeScale = 0f;
            LevelIsFinished = true;
            text.text = "SCORE: " + CoinManager.GetComponent<CoinManager>().getScore().ToString() + "\n Deaths: " + RankManager.GetComponent<RankManager>().getDeaths().ToString() + "\nRANK: " + RankManager.GetComponent<RankManager>().getRank();
            
            GoalMenuUI.SetActive(true);
            GameCanvas.SetActive(false);
        }
    }

    public bool isFinished()
    {
        return LevelIsFinished;
    }
}
