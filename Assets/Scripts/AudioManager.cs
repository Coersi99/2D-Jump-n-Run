using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // not required in the singleton pattern, but helps us tracking which object survived
    [SerializeField] int id;

    // public getter for receiving the current Instance of our singleton
    // the setter is private, such that external classes cannot change the reference
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource bgm1;
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private AudioSource victoryMusic;
    [SerializeField] private AudioSource explosionEffect;
    [SerializeField] private AudioSource deathMelody;
    [SerializeField] private AudioSource enemyHitEffect;
    [SerializeField] private AudioSource ghostScreamEffect;
    [SerializeField] private AudioSource gawdEffect;
    [SerializeField] private AudioSource bossSecondPhaseEffect;
    [SerializeField] private AudioSource bossDeathEffect;
    [SerializeField] private AudioSource enemyDeath1Effect;
    [SerializeField] private AudioSource enemyDeath2Effect;
    [SerializeField] private AudioSource coinEffect;
    

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

    public void playExplosionEffect()
    {
        explosionEffect.Play();
    }

    public void playDeathMelody()
    {
        bgm1.Stop();
        bossMusic.Stop();
        victoryMusic.Stop();
        deathMelody.Play();
    }

    public void playBossMusic()
    {
        bgm1.Stop();
        bossMusic.Play();
    }

    public void playVictoryMusic()
    {
        bgm1.Stop();
        bossMusic.Stop();
        victoryMusic.Play();
    }

    public void playEnemyHitEffect()
    {
        enemyHitEffect.Play();
    }

    public void playGhostScreamEffect()
    {
        ghostScreamEffect.Play();
    }

    public void playGawdEffect()
    {
        gawdEffect.Play();
    }

    public void playBossSecondPhaseEffect()
    {
        bossSecondPhaseEffect.Play();
    }

    public void playBossDeathEffect()
    {
        bossDeathEffect.Play();
    }

    public void playEnemyDeath1Effect()
    {
        enemyDeath1Effect.Play();
    }

    public void playEnemyDeath2Effect()
    {
        enemyDeath2Effect.Play();
    }

    public void playCoinEffect()
    {
        coinEffect.Play();
    }
}
