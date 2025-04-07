using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float elapsedTime;

    public static GameManager gm;

    public Text timerText;

    public bool isPaused;
    
    public GameObject pauseMenu;
    public GameObject playerUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        elapsedTime = 0;

        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }

        //Takes the elapsed time and divides it into minutes, seconds and milliseconds
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt(elapsedTime * 1000 % 1000);
        timerText.text = string.Format("{0:00}'{01:00}''{2:000}" , minutes, seconds, milliseconds);
    }

    public void TogglePause() 
    {
        playerUI.SetActive(!playerUI.activeInHierarchy);
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;

        Cursor.lockState = pauseMenu.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Retry() 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
