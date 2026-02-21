using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;

    public float acceleration;

    public float jumpStrength;
    public float mouseSensetivity;

    public float bobbingStrength;
    public float bobbingFrequency;

    private bool onSurface;

    private const float BackMovementFactor = 0.625f;
    private const float SidewaysMovementFactor = 0.75f;

    private const float AirFriction = 0.1f;

    private float time;
    private float footstepsTimer;

    private Vector3 prevMovementDir;

    private float materialFriction;

    private float velocity;

    private float movementVelocity;
    private float bobTimer;

    private string tag = "";

    private float rotX;
    private float rotY;

    private float minFov = 85;
    private float maxFov = 100;

    private float lowestFov = 30;
    private float highestFov = 140;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        this.time = 0f;
        this.bobTimer = 0f;

        this.onSurface = true;
        tag = "Floor";

        rotX = 360 - camera.transform.eulerAngles.y;
        rotY = camera.transform.eulerAngles.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float tick = Time.fixedDeltaTime;
        float friction = this.onSurface ? this.materialFriction : AirFriction;
        float controlCoefficient = friction;

        float mass = this.GetComponent<Rigidbody>().mass;
        float g = Physics.gravity.magnitude;
        float weight = mass * g;

        float accelerationWithFriction = acceleration + 1 - Mathf.Exp(this.velocity / 2f) - friction * g;
        this.velocity += tick * accelerationWithFriction * controlCoefficient;

        //transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X"), 0f);
        //camera.transform.rotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") + Mathf.Sin(time * bobbingFrequency) * (bobbingStrength + bobTimer/500f) * movementVelocity * 100 / 5f * 8f, 0f, Mathf.Cos(time * bobbingFrequency * 1.618f) * bobbingStrength * this.velocity);

        //camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, -Input.GetAxis("Horizontal") * 2f);
        camera.transform.eulerAngles = new Vector3(-rotY + Mathf.Sin(time * bobbingFrequency) * (bobbingStrength + bobTimer / 500f) * movementVelocity * 100 / 5f * 8f, 360 - rotX, 0f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 360 - rotX, transform.eulerAngles.z);

        float vAxis = Input.GetAxis("Vertical");
        float hAxis = Input.GetAxis("Horizontal");

        vAxis = ( vAxis > 0f ? vAxis : vAxis * BackMovementFactor ) ;

        if (vAxis == 0f && hAxis == 0f) this.velocity -= tick * friction * g;
        this.velocity = Mathf.Max(0, this.velocity);

        Vector3 movementDir = (transform.forward * vAxis + transform.right * SidewaysMovementFactor * hAxis) * tick;
        movementDir = (movementDir != Vector3.zero && movementDir.magnitude > 1f ? movementDir.normalized : movementDir);

        Vector3 deltaPos = ( Vector3.Lerp ( prevMovementDir, movementDir, controlCoefficient ) * this.velocity ) ;
        transform.position += deltaPos;

        this.movementVelocity = deltaPos.magnitude;

        if (vAxis == 0f && hAxis == 0f && deltaPos.magnitude < 0.02f) this.velocity = 0f;

        this.prevMovementDir = Vector3.Lerp ( prevMovementDir, movementDir, controlCoefficient );
    }

    void Update()
    {
        rotX -= Input.GetAxis("Mouse X") * mouseSensetivity;
        rotY += Input.GetAxis("Mouse Y") * mouseSensetivity;

        rotY = Mathf.Clamp(rotY, -90f, 90f);




        time += Time.deltaTime;
        footstepsTimer += Time.deltaTime;

        if (movementVelocity > 0.02f && footstepsTimer > Mathf.Lerp(0.4f, 0.15f, movementVelocity * 10f) && onSurface)
        {
            //footsteps.Play();
            footstepsTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && this.onSurface)
        {
            //jump_grunt.Play();
            GetComponent<Rigidbody>().AddForce(transform.up * jumpStrength);

            this.onSurface = false;
        }

        if (Input.GetKey(KeyCode.LeftControl)) transform.localScale = new Vector3(0.6f, 0.45f, 0.6f);
        else transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);

        if (!onSurface)
        {
            bobTimer += Time.deltaTime;
        }

        StickToGround();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.materialFriction = collision.collider.material.dynamicFriction;
        this.onSurface = true;

        this.bobTimer = 0f;

        this.tag = collision.collider.tag;

        if (this.tag == "Floor")
        {
            //this.fall_impact.Play();
        }
    }

    void StickToGround()
    {
        if (Physics.Raycast(transform.position, -camera.transform.up, out RaycastHit hit, 10f))
        {
            Vector3 groundNormal = hit.normal;

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}