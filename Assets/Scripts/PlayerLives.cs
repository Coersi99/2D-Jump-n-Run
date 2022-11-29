using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    // not required in the singleton pattern, but helps us tracking which object survived
    [SerializeField] int id;

    public GameObject[] hearts;
    private int life;
    private bool dead;

    Transform player;

    // public getter for receiving the current Instance of our singleton
    // the setter is private, such that external classes cannot change the reference
    public static HeartSystem Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself
        // Note, we don't delete the game object, since the component might be attached next to other components
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // store the reference to ourself in the Instance variable if no other instance has been set yet
            Instance = this;
        }
    }

    private void Start()
    {
        life = hearts.Length;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(dead)
        {
            Debug.Log("Jetzt tot, big uff");
        }
    }

    // Triggered once the player takes damage
    public void TakeDamage(int damage)
    {
        if(life >= 1)
        {
            life -= damage;      
            Destroy(hearts[life].gameObject);
            player.position = player.position + new Vector3(-1, 0, 0);
            if(life < 1)
            {
             dead = true;
            }
        }

    }
}
