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
        bestTime = PlayerPrefs.GetFloat("BestTime" + SceneManager.loadedSceneCount, Mathf.Infinity);
        UpdateBestTimeText();

        elapsedTime = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            FinishMenu();
            playerUI.SetActive(false);

            if (GameManager.gm.elapsedTime < PlayerPrefs.GetFloat("BestTime" + SceneManager.loadedSceneCount, 0))
            {
                //Creates a "best time" for the current level
                PlayerPrefs.SetFloat("BestTime" + SceneManager.loadedSceneCount, GameManager.gm.elapsedTime);
                UpdateBestTimeText();
            }

            //Displays the your the time you did
            yourTimeText.text = TimeString(GameManager.gm.elapsedTime);
        }
    }

    //Updates the best time found in the PlayerPrefs
    private void UpdateBestTimeText() 
    {
         bestTimeText.text = TimeString(PlayerPrefs.GetFloat("BestTime" + SceneManager.loadedSceneCount, 0));
    }

    public void FinishMenu()
    {
        finishMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    string TimeString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt(time * 1000 % 1000);
        return (string.Format("{0:00}'{01:00}''{2:000}", minutes, seconds, milliseconds));
    }
}
