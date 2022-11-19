using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] float bigRadius = 5f;
    [SerializeField] float shortRadius = 2f;
    [Range(1, 360)][SerializeField] float angle = 45f;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] LayerMask obstructionLayer;

    public GameObject playerRef;

    public bool CanSeePlayer { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // only check every 0.2 seconds (more is not necessary)
        while (true)
        {
            yield return wait;
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, shortRadius, targetLayer);
        if (rangeCheck.Length > 0)
        {
            CanSeePlayer = true;
        }
        else
        {
            rangeCheck = Physics2D.OverlapCircleAll(transform.position, bigRadius, targetLayer);
            if (rangeCheck.Length > 0)
            {
                Transform target = rangeCheck[0].transform;
                Vector2 directionToTarget = (target.position - transform.position).normalized;
                if (Vector2.Angle(GetComponent<Enemy1>().getDirection(), directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, target.position);
                    if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))   // true if ray does not hit obstruction
                    {
                        CanSeePlayer = true;
                    }
                    else
                    {
                        CanSeePlayer = false;
                    }
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            // temporary:
            else if (CanSeePlayer)
                CanSeePlayer = false;
        }
    }
}
