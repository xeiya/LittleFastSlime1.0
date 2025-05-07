using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeSave : MonoBehaviour
{

    [SerializeField] private Text bestTimeText;
    [SerializeField] private Text yourTimeText;

    private float yourTime;
    private float bestTime;
    public float elapsedTime;

    public GameObject finishMenuUI;
    public GameObject playerUI;

    private void Start()
    {
        //Gets the best time
        bestTime = PlayerPrefs.GetFloat("BestTime" + SceneManager.GetActiveScene().buildIndex, Mathf.Infinity);
        UpdateBestTimeText();

        elapsedTime = 0;
    }

    //Checks the for the object that has been coillided with it. if it has been collided with then run this code
    private void OnTriggerEnter(Collider other)
    {
        //Displays the Finish UI if the player makes contact witht his object
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            FinishMenu();
            playerUI.SetActive(false);

            //Saves the time in playerPrefs based on the level, then displays it based on if the previous time is larger than the current
            if (GameManager.gm.elapsedTime < PlayerPrefs.GetFloat("BestTime" + SceneManager.GetActiveScene().buildIndex, 3600f))
            {
                //Creates a "best time" for the current level
                PlayerPrefs.SetFloat("BestTime" + SceneManager.GetActiveScene().buildIndex, GameManager.gm.elapsedTime);
                UpdateBestTimeText();
            }

            //Displays the your the time you did
            yourTimeText.text = TimeString(GameManager.gm.elapsedTime);
        }
    }

    //Updates the best time found in the PlayerPrefs based on the current scene
    private void UpdateBestTimeText() 
    {
         bestTimeText.text = TimeString(PlayerPrefs.GetFloat("BestTime" + SceneManager.GetActiveScene().buildIndex, 3600));
    }

    public void FinishMenu()
    {
        finishMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    //Formats the time into minutes, seconds and milliseconds
    string TimeString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt(time * 1000 % 1000);
        return (string.Format("{0:00}'{01:00}''{2:000}", minutes, seconds, milliseconds));
    }
}
