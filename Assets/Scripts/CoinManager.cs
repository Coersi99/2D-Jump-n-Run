using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public TextMeshProUGUI text;
    private int score;

    private GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        score = gm.score / 2;
        text.text = score.ToString();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void AddToScore(int Coinvalue)
    {
        score += Coinvalue;
        text.text = score.ToString();

        gm.score = score;
    }

    public int getScore()
    {
        return score;
    }
}
