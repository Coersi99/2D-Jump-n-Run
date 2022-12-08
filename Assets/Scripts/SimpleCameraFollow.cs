using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public GameObject playerRef;
    [SerializeField] float speed = 20f;
    private bool following = true;

    public void stopFollowing()
    {
        following = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (following)
        {
            Vector3 cameraPosition = this.transform.position;
            Vector3 playerPos = playerRef.transform.position;
            playerPos.z = cameraPosition.z;
            Vector3 directionToTarget = (playerPos - cameraPosition).normalized;
            float currentDistance = Vector3.Distance(playerPos, cameraPosition);
            if (currentDistance > 0)
            {
                Vector3 newCameraPosition = cameraPosition + speed * Time.deltaTime * directionToTarget * currentDistance;
                float futureDistance = Vector3.Distance(playerPos, newCameraPosition);
                if (futureDistance > currentDistance)
                {
                    //Overshot the Target
                    newCameraPosition = playerPos;
                }
                this.transform.position = newCameraPosition;
            }
        }
        
    }
}
