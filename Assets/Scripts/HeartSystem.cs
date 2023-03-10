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
    Player player;

    [SerializeField] int maxLifeLimit;
    public float vulnerabilityTime = 1f;
    public List<GameObject> hearts = new List<GameObject>();
    public GameObject canvas;
    private float heartOffset;
    private int life;
    public bool playerDead = false;
    private bool triggerDeathScreen;
    public bool isVulnerable = true;
    private float secondsCount = 0f;

    //Audio stuff
    [SerializeField] private AudioSource damageSoundEffect;
    [SerializeField] private AudioSource deathSoundEffect;


    private GameMaster gm;

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
        life = hearts.Count;
        rb = GetComponent<Rigidbody2D>();
        material = GetComponent<SpriteRenderer>().material;
        player = GetComponent<Player>();

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        heartOffset = hearts[life-1].gameObject.transform.position.x - hearts[life-2].gameObject.transform.position.x;
    }

    void Update()
    {
        if(triggerDeathScreen)
        {
            animator.SetTrigger("Death");
            deathSoundEffect.Play();
            rb.bodyType = RigidbodyType2D.Static;
            GetComponent<CharacterController2D>().enabled = false;
            GetComponent<Player>().enabled = false;
            StartCoroutine(waitShortlyThenRestart());
            triggerDeathScreen = false;

            gm.deaths++;
        }

        secondsCount += Time.deltaTime;

        if(secondsCount >= vulnerabilityTime){
            isVulnerable = true;
        }

    }

    private IEnumerator waitShortlyThenRestart()
    {
        yield return new WaitForSeconds(0.5f);
        DeathScreen.Instance.enableDeathScreen();
        
    }

    // Triggered once the player takes damage
    public void TakeDamage(int damage, float direction)
    {
        if(life >= 1 && isVulnerable)
        {
            while (life >= 1 && damage > 0)
            {
                life--;
                Destroy(hearts[life].gameObject);
                hearts.RemoveAt(life);
                damage--;
            }

            if(life < 1)
            {
                triggerDeathScreen = true;
                playerDead = true;
            }else{

                player.knockbackCounter = player.knockBackTotalTime;    //Set counter to activate knockback and disallow movement
                player.canShoot = false;    //Player can't shoot during knockback animation
                Invoke("enableShoot", player.knockBackTotalTime);

                if(direction >= 0)      //Set knockback direction given the direction of the danger source
                {
                    player.knockFromRight = true;
                }else{
                    player.knockFromRight = false;
                }
                
                animator.SetTrigger("Hit");
                GetComponent<SimpleFlash>().Flash();    //Activate Flash effect (i.e. invulnerability blinking)
                damageSoundEffect.Play();
            }

            isVulnerable = false;
            secondsCount = 0f;
            
        }
    }

    private void enableShoot()
    {
        player.canShoot = true;
    }

    public void fallToDeath()
    {
        isVulnerable = true;
        TakeDamage(life, 0);
    }

    public void heal()
    {
        if(life < maxLifeLimit)    
        {
            GameObject rightMostHeart = hearts[life-1].gameObject;
            GameObject heartClone = Instantiate(rightMostHeart ,new Vector3(rightMostHeart.transform.position.x + heartOffset, rightMostHeart.transform.position.y, 1), rightMostHeart.transform.rotation);
            heartClone.transform.SetParent(canvas.transform);
            heartClone.transform.localScale = new Vector3(rightMostHeart.transform.localScale.x, rightMostHeart.transform.localScale.y, 1);
            hearts.Add(heartClone);
            life++;
        }
    }
}
