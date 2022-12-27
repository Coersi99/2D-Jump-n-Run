using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private Vector3 startPos;
    public GameObject cam;
    public float parallaxEffect;
    public bool enableVerticalMovement;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x * (1-parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        if(enableVerticalMovement){
            transform.position = new Vector3(startPos.x + dist, cam.transform.position.y, transform.position.z);
        }else
        {
            transform.position = new Vector3(startPos.x + dist, startPos.y, transform.position.z);
        }  

        if(temp > startPos.x + length) startPos.x += length;
        else if (temp < startPos.x - length) startPos.x -= length;
    }
}
