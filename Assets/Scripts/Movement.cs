using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    Vector3 movement;

    private Vector3 start;
    [Header("Player Properties")]
    [SerializeField] private float speed;
    [SerializeField] private float topSpeed;
    
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    bool readyToJump = true;

    bool grounded = false;

    private bool dashing = true;
    [SerializeField]private float dashingPower = 30f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 2f;

    [Header("Visual Properties")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] ParticleSystem speedLines;
    [SerializeField] private Volume volume;

    [SerializeField] private AnimationCurve lensDistortionAnimationCurve;

    private float lensIntensityLastTime;

    private LensDistortion lensDistortion;

    [Header("Camera Properties")]
    [SerializeField] Transform camTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        volume.profile.TryGet(out lensDistortion);
    }
    void Start()
    {
        Vector3 start = rb.transform.position;

        tr.emitting = false;
    }

    void Update()
    {
        myInput();

        //When pressing shift, it calls the dash couritine
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashing)
        {
            StartCoroutine(Dash());

            speedLines.Play();

            //Takes the lens intensity from last time
            lensIntensityLastTime = Time.realtimeSinceStartup;
        }

        //Creates a float and evaluates the lens intensity last time since startup
        float lensIntensity = lensDistortionAnimationCurve.Evaluate(Time.realtimeSinceStartup - lensIntensityLastTime);
        //takes the lensDistotion intensity value and applies it to the lensIntensity float
        lensDistortion.intensity.value = lensIntensity;

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GameManager.gm.TogglePause();
        }

        movement = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * movement;
    }

    private void FixedUpdate()
    {

        rb.AddForce(movement * speed);

        float y = rb.linearVelocity.y;

        Vector3 tempVec = rb.linearVelocity;
        tempVec.y = 0;

        if (tempVec.magnitude > topSpeed) 
        {
            tempVec = tempVec.normalized * topSpeed;
        }

        tempVec.y = y;

        rb.linearVelocity = tempVec;

        GroundedCheck();

        playerReset();
    }

    private void Jump() 
    { 
        Vector3 jumpMovement = rb.linearVelocity;

        jumpMovement.y = jumpForce;

        rb.linearVelocity = jumpMovement;
    }

    private void resetJump() 
    {
        readyToJump = true;
    }

    private void playerReset() 
    {
        //If the player is below -2 on the Y axis, reset the player to the starting position
        if (rb.transform.position.y < -2)
        {
            rb.transform.position = start;
        }
    }

    private void myInput() 
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
    }

    void GroundedCheck() 
    {
        //Checks the distance between the ground and the player
        //If it detects the ground grounded is true, else, false
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            grounded = true;
        }
        else 
        {
            grounded = false;
        }
    }

    private IEnumerator Dash()
    {
        dashing = false;
        tr.emitting = true;

        //Takes the direction of where we are facing and dash in that direction
        Vector3 dashDir;
        dashDir = Camera.main.transform.forward;
        dashDir.y = 0;
        dashDir.Normalize();
        rb.linearVelocity = dashDir * speed;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        dashing = true;
    }
}
