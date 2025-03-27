using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    public void levelSelect()
    {
        SceneManager.LoadScene(levelName);
    }
}
