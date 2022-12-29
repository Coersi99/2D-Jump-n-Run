using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public GameObject DeathScreenUI;

    // not required in the singleton pattern, but helps us tracking which object survived
    [SerializeField] int id;

    // public getter for receiving the current Instance of our singleton
    // the setter is private, such that external classes cannot change the reference
    public static DeathScreen Instance { get; private set; }

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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void enableDeathScreen()
    {
        DeathScreenUI.SetActive(true);
        AudioManager.Instance.playDeathMelody();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        DeathScreenUI.SetActive(false);
    }
}
