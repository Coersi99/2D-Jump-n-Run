using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinValue = 10;
    private bool collectable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectable)
        {
            CoinManager.instance.AddToScore(coinValue);
            AudioManager.Instance.playCoinEffect();
            collectable = false;
        }
        Destroy(this.gameObject);
    }
}
