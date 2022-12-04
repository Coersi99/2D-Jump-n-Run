using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartSystem : MonoBehaviour
{
    // not required in the singleton pattern, but helps us tracking which object survived
    [SerializeField] int id;

    //Player's rigidbody
    private Rigidbody2D rb;
    public Animator animator;

    public float vulnerabilityTime = 1f;
    public GameObject[] hearts;
    private int life;
    private bool dead;
    private bool isVulnerable = true;
    private float secondsCount = 0f;

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
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(dead)
        {
            animator.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<CharacterController2D>().enabled = false;
            GetComponent<Player>().enabled = false;
        }

        secondsCount += Time.deltaTime;

        if(secondsCount >= vulnerabilityTime){
            isVulnerable = true;
        }
    }

    private void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Triggered once the player takes damage
    public void TakeDamage(int damage, float enemyKnockback, float direction)
    {
        if(life >= 1 && isVulnerable)
        {
            life -= damage;      
            Destroy(hearts[life].gameObject);
            if(life < 1)
            {
             dead = true;
            }

            if(direction < 0)
            {
                rb.velocity = new Vector2(-enemyKnockback, rb.velocity.y);
            }else{
                rb.velocity = new Vector2(enemyKnockback, rb.velocity.y);
            }
            isVulnerable = false;
            secondsCount = 0f;
        }

    }
}
