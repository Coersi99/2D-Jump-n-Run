using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public GameObject playerRef;
    [SerializeField] float speed = 20f;
    [SerializeField] float offset_y = 2.5f;
    public int cameraSwitchXLower;
    public int cameraSwitchXUpper;
    private bool following = true;
    private bool adjusted_after_load = false;
    private bool enableOffset = true;


    public void stopFollowing()
    {
        following = false;
    }

    Vector3 SetZ(Vector3 vector, float z)
    {
        vector.z = z;
        return vector;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }




    // Update is called once per frame
    void Update()
    {
        if (!adjusted_after_load)
        {
            this.transform.position = SetZ(playerRef.transform.position, -10);
            adjusted_after_load = true;
        }

        //disable Offset when certain location reached (i.e. camera moves down a bit)
        if(playerRef.transform.position.x >= cameraSwitchXLower && playerRef.transform.position.x <= cameraSwitchXUpper) enableOffset = false;
        else enableOffset = true;

        if (following)
        {
            Vector3 cameraPosition = this.transform.position;
            Vector3 playerPos = playerRef.transform.position;
            if(enableOffset)
            {
                playerPos.y += offset_y;
            }
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
