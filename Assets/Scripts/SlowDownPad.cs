using UnityEngine;

public class SlowDownPad : MonoBehaviour
{
    [SerializeField] Rigidbody player;
    [SerializeField] private float slowSpeed;
    private void OnTriggerEnter(Collider other)
    {
        player.linearVelocity = player.linearVelocity.normalized * slowSpeed;
    }
}
