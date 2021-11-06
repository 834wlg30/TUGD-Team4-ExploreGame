using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMoveScript : MonoBehaviour
{
    public CharacterController charController;

    public Transform cam;
    public float speed;
    public Transform lookTarget;
    public float lookTargetDistance;

    public float turnSmoothTime;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

        if(dir.magnitude >= 0.1)
        {
            float targetAngle = Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * dir; //Vector3.forward;
            charController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //RaycastHit hit;
        //Ray forwardRay = new Ray(transform.position, Vector3.forward);

        //if (Physics.Raycast(forwardRay, out hit, sightDistance))
    }

    private void FixedUpdate()
    {
        lookTarget.transform.position = transform.position + Camera.main.transform.forward * lookTargetDistance; //Camera.main.transform.position + Camera.main.transform.forward * 5;
    }
}
