using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
    Vector3 movement;

    private Vector3 start;

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

    [SerializeField] private TrailRenderer tr;
    [SerializeField] Transform camTransform;

    [SerializeField] ParticleSystem speedLines;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        
        Vector3 start = rb.transform.position;

        tr.emitting = false;
    }

    void Update()
    {
        myInput();

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        movement = Quaternion.AngleAxis(camTransform.rotation.eulerAngles.y, Vector3.up) * movement;
    }

    private void LateUpdate()
    {
        //checks if the speed is over 12, if not don't play effect
        if (rb.linearVelocity.magnitude >= 12)
        {
            speedLines.Play();
        }
        else if (rb.linearVelocity.magnitude <= 11)
        {
            speedLines.Stop();
        }
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
