using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    public static GameManager gm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gm == null)
        {
            Destroy(gameObject);
        }
        else 
        {
            gm = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
