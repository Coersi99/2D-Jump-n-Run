using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 10;
    private GameObject CoinManagerRef;
    private bool collectable = true;

    // Start is called before the first frame update
    void Start()
    {
        CoinManagerRef = GameObject.FindGameObjectWithTag("CoinManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectable)
        {
            CoinManagerRef.GetComponent<CoinManager>().AddToScore(coinValue);
            collectable = false;
        }
        Destroy(this.gameObject);
    }
}
