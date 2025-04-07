using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SpeedUpPad : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Rigidbody player;
    [SerializeField] private float speedUp;
    [SerializeField] private float maxSpeed;

    [Header("Volume")]
    [SerializeField] private Volume volume;
    [SerializeField] private AnimationCurve lensDistortionAnimationCurve;
    private float lensIntensityLastTime;
    private LensDistortion lensDistortion;

    private void Awake()
    {
        volume.profile.TryGet(out lensDistortion);
    }

    private void OnTriggerEnter(Collider other)
    {
        player.linearVelocity += player.linearVelocity.normalized * speedUp;
        player.linearVelocity = Vector3.ClampMagnitude(player.linearVelocity, maxSpeed);

        lensIntensityLastTime = Time.realtimeSinceStartup;
        float lensIntensity = lensDistortionAnimationCurve.Evaluate(Time.realtimeSinceStartup - lensIntensityLastTime);
        lensDistortion.intensity.value = lensIntensity;
    }
}
