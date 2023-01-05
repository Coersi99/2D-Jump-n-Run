using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private bool collectable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectable)
        {
            HeartSystem.Instance.heal();
            collectable = false;
        }
        Destroy(this.gameObject);
    }
}
