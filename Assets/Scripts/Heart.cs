using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private bool collectable = true;
    [SerializeField] public float despawnTime;

    void Start()
    {
        StartCoroutine(despawn());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collectable)
        {
            HeartSystem.Instance.heal();
            AudioManager.Instance.playHeartEffect();
            collectable = false;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
