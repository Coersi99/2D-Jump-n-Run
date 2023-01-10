using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pauseMenuSound;

    [SerializeField] string MainMenu;

    private GameObject Goal;

    private GameMaster gm;

    private void Start()
    {
        GameIsPaused = false;
        Goal = GameObject.FindGameObjectWithTag("Goal");
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Goal.GetComponent<Goal>().isFinished() && Input.GetKeyDown(KeyCode.Escape) && !HeartSystem.Instance.playerDead)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseMenuSound.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseMenuSound.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenu);

        gm.score = 0;
        gm.deaths = 0;
        gm.lastCheckPointPos = gm.firstCheckPointPos;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
