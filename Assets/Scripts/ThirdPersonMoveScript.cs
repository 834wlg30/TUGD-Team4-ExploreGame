using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMoveScript : MonoBehaviour
{
    public CharacterController charController;

    public Transform cam;
    public float speed;
    public float sprintFactor;
    public Vector3 currentMovement;

    public float verticalVelocity;
    public float initialJumpVelocity;
    public float maxJumpHeight;
    public float maxJumpTime;
    public float gravity;

    public Transform lookTarget;
    public Transform shoulderTarget;
    public float camSpeed;
    public Vector3 camTurn = new Vector3(0, 0, 0);
    public float lookTargetDistance;

    public Transform testGun;
    public GameObject projectile;
    public Transform spawnBulletPoint;
    public GameObject gunEffects;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    [SerializeField] LayerMask aimColliderMask;
    public Transform aimTargetTest;

    MeshSockets sockets;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        sockets = GetComponent<MeshSockets>();
        //sockets.Attach(testGun.transform, MeshSockets.SocketId.RightHand);

    }

    private void Awake()
    {
        setupJumpVariables();
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if(dir.magnitude >= 0.1)
        {
            float targetAngle = Camera.main.transform.eulerAngles.y;
            //targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //This is one of two things that makes the character keep rotated toward where he is walking rather than where he is looking
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * dir; // Vector3.forward; //Using Vector3.forward instead is one of two things that makes the character keep rotated toward where he is walking rather than where he is looking
            //currentMovement.x = moveDir.x;
            //currentMovement.z = moveDir.z;

            float sprint;
            if (Input.GetKey(KeyCode.LeftShift)) sprint = sprintFactor;
            else sprint = 1;
            charController.Move(moveDir.normalized * speed * sprint * Time.deltaTime);
        }
        //charController.Move(currentMovement * Time.deltaTime);

        //RaycastHit hit;
        //Ray forwardRay = new Ray(transform.position, Vector3.forward);

        //if (Physics.Raycast(forwardRay, out hit, sightDistance))

        if (Input.GetMouseButtonDown(0)) Shoot();
        HandleGravity();
        Jump();
    }

    private void FixedUpdate()
    {
        camTurn += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * camSpeed;
        camTurn.x = Mathf.Clamp(camTurn.x, -85, 85);

        lookTarget.transform.position = transform.position + Camera.main.transform.forward * lookTargetDistance; //Camera.main.transform.position + Camera.main.transform.forward * 5;

        shoulderTarget.rotation = Quaternion.Euler(camTurn);

        //shoulderTarget.Rotate(new Vector3(Input.GetAxis("Mouse Y"), 0, 0f) * Time.deltaTime * camSpeed);
        //shoulderTarget.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * camSpeed);

        //Vector3 angle = shoulderTarget.rotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f) * Time.deltaTime * camSpeed;
        //angle = new Vector3(angle.x, angle.y, 0);
        //shoulderTarget.rotation = Quaternion.Euler(angle);
        //shoulderTarget.LookAt(lookTarget);

        ThirdPersonController();
    }

    public void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    public void Jump()
    {
        //if (!charController.isGrounded)
        //{
            //verticalVelocity -= gravity * Time.deltaTime;
        //}
        //else
        //{
            //verticalVelocity = -1f;
        //}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (charController.isGrounded)
            {
                Debug.Log("Jump!!!");
                //transform.GetComponent<Rigidbody>().velocity = Vector3.up * jumpForce;
                verticalVelocity = initialJumpVelocity;
            }
            else
            {
                Debug.Log("NO GROUNDED!! NO JUMP U BAD!!");
            }
        }

        //charController.Move(new Vector3 (0,verticalVelocity,0) * Time.deltaTime);
    }

    public void HandleGravity()
    {
        if (!charController.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -1;
        }

        //currentMovement.y = verticalVelocity;
        charController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    public void ThirdPersonController()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 4000f, aimColliderMask))
        {
            aimTargetTest.position = hit.point;
            testGun.LookAt(hit.point);
        }
        else
        {
            aimTargetTest.position = lookTarget.position;
            testGun.LookAt(lookTarget);
        }
    }

    public void Shoot()
    {
        Vector3 aimDir = (aimTargetTest.position - testGun.position).normalized;
        GameObject bulletClone = Instantiate(projectile, spawnBulletPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        GameObject gunEffectsClone = Instantiate(gunEffects, spawnBulletPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
        Destroy(gunEffectsClone, 5f);
    }
}
