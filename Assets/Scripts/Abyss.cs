using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private GameObject cameraRef;
    private GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        cameraRef = GameObject.FindGameObjectWithTag("MainCamera");
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cameraRef.GetComponent<SimpleCameraFollow>().stopFollowing();
        playerRef.GetComponent<HeartSystem>().fallToDeath();
    }
}
