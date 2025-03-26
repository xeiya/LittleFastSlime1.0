using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float timer;

    public static GameManager gm;

    public Text timerText;

    public bool isPaused;
    
    public GameObject pauseMenu;
    public GameObject playerUI;

    [SerializeField]

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

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = (Mathf.Round(timer*100) / 100).ToString();
    }

    public void TogglePause() 
    {
        playerUI.SetActive(!playerUI.activeInHierarchy);
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;

        Cursor.lockState = pauseMenu.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
