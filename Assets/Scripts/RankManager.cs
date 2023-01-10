using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RankManager : MonoBehaviour
{

    public int bronze_count;
    public int silver_count;
    public int gold_count;

    public int bronze_value;
    public int silver_value;
    public int gold_value;

    public double max_score;

    [SerializeField] GameObject CoinManager;
    public int score;

    [SerializeField] double s_boundary = 0.8;
    [SerializeField] double a_boundary = 0.6;
    [SerializeField] double b_boundary = 0.4;
    [SerializeField] double c_boundary = 0.2;

    public string rank;

    private GameMaster gm;
    double deaths;
    double deathfactor;

    // Start is called before the first frame update
    void Start()
    {
        bronze_count = GameObject.FindGameObjectsWithTag("Bronze Coin").Length;
        silver_count = GameObject.FindGameObjectsWithTag("Silver Coin").Length;
        gold_count = GameObject.FindGameObjectsWithTag("Gold Coin").Length;

        bronze_value = 10;
        silver_value = 50;
        gold_value = 100;

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        deaths = gm.deaths;
        deathfactor = System.Math.Pow(1.2, deaths);

        max_score = deathfactor * (bronze_count * bronze_value + silver_count * silver_value + gold_count * gold_value);

        
    }

    // Update is called once per frame
    void Update()
    {
        score = CoinManager.GetComponent<CoinManager>().getScore();

        if(score >= max_score * s_boundary)
        {
            this.rank = "S";
        } 
        else if (score >= max_score * a_boundary)
        {
            this.rank = "A";
        }
        else if (score >= max_score * b_boundary)
        {
            this.rank = "B";
        }
        else if (score >= max_score * c_boundary)
        {
            this.rank = "C";
        }
        else
        {
            this.rank = "D";
        }
    }


    public string getRank()
    {
        return this.rank;
    }

    public double getDeaths()
    {
        return Mathf.RoundToInt((float)this.deaths); ;
    }
}
