using UnityEngine;

public class SpeedLinesActivate : MonoBehaviour
{
    [SerializeField] ParticleSystem speedLines;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            CreateSpeedLines();
        }
    }

    void CreateSpeedLines() 
    {
        speedLines.Play();
    }
}
