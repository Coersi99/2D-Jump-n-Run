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
    public Material invincibilityShader;
    Material material;

    public float vulnerabilityTime = 1f;
    public GameObject[] hearts;
    private int life;
    private bool dead;
    private bool fellToDeath;
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
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (fellToDeath)
        {
            print("reached if");
            StartCoroutine(waitShortlyThenRestart());
        }
        else if(dead)
        {
            animator.SetTrigger("Death");
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<CharacterController2D>().enabled = false;
            GetComponent<Player>().enabled = false;
            StartCoroutine(waitShortlyThenRestart());
        }

        secondsCount += Time.deltaTime;

        if(secondsCount >= vulnerabilityTime){
            isVulnerable = true;
            //GetComponent<Player>().isDamaged = false;
        }

    }

    private IEnumerator waitShortlyThenRestart()
    {
        print("waiting");
        yield return new WaitForSeconds(0.5f);
        print("restarting");
        Restart();
    }

    private void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Triggered once the player takes damage
    public void TakeDamage(int damage, float enemyKnockback, float direction)
    {
        if(life >= 1 && isVulnerable)
        {
            //GetComponent<Player>().isDamaged = true;
            //life -= damage;      
            //Destroy(hearts[life].gameObject);
            while (life >= 1 && damage > 0)
            {
                life--;
                Destroy(hearts[life].gameObject);
                damage--;
            }

            if(life < 1)
            {
             dead = true;
            }else{
                
                if(direction < 0)
                {   
                    rb.velocity = new Vector2(-enemyKnockback, 15);
                }else{
                    rb.velocity = new Vector2(enemyKnockback, 15);
                }

                animator.SetTrigger("Hit");
                GetComponent<SimpleFlash>().Flash();
            }

            isVulnerable = false;
            secondsCount = 0f;
            
        }
    }

    public void fallToDeath()
    {
        print("falling to death");
        isVulnerable = true;
        fellToDeath = true;
        TakeDamage(life, 0, 0);
    }
}
