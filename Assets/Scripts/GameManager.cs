using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float timer;

    public static GameManager gm;

    public Text timerText;

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
}
