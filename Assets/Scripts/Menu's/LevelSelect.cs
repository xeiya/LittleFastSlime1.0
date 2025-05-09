using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    //Selects the level based on the name
    public void levelSelect()
    {
        SceneManager.LoadScene(levelName);
    }
}
