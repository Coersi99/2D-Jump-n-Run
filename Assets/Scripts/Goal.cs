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
    [SerializeField] TextMeshProUGUI text;

    private void Start()
    {
        LevelIsFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("triggered");
        if (!LevelIsFinished)
        {
            Time.timeScale = 0f;
            LevelIsFinished = true;
            text.text = "Score: " + CoinManager.GetComponent<CoinManager>().getScore().ToString();
            GoalMenuUI.SetActive(true);
            GameCanvas.SetActive(false);
        }
    }

    public bool isFinished()
    {
        return LevelIsFinished;
    }
}
