using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingRedonCentaurionController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 10;
    [SerializeField] float drag;
    [SerializeField] float gravity;
    [SerializeField] float jumpForce;
    [SerializeField] GameObject cam;
    [SerializeField] LayerMask mask;
    bool addGravity = true;
    float startGrav;
    float xIn;
    float zIn;
    float camSensitivity = 5;
    float t;
    Vector3 moveDirection;
    Vector3 temp;
    Vector3 camRot;
    Vector3 playerRot;
    Rigidbody rb;

    private void Start ()
    {
        rb = GetComponent<Rigidbody> ();
        startGrav = gravity;
    }

    private void Update ()
    {
        t = Time.deltaTime;
        xIn = Input.GetAxisRaw ("Horizontal");
        zIn = Input.GetAxisRaw ("Vertical");

        moveDirection = new Vector3 (xIn, moveDirection.y, zIn);
        moveDirection = moveDirection.normalized * moveSpeed;

        playerRot.y += Input.GetAxisRaw ("Mouse X") * camSensitivity;
        camRot.x -= Input.GetAxisRaw ("Mouse Y") * camSensitivity;

        camRot.x = Mathf.Clamp (camRot.x, -90, 90);

        addGravity = !Physics.Raycast (transform.position, Vector3.down, 1.1f,mask);
        //Debug.Log (addGravity);
        if (Input.GetButtonDown ("Jump"))
        {
            if (!addGravity)
            {
                rb.AddForce (Vector3.up * jumpForce * 10, ForceMode.Impulse);
            }
        }
        else
        {
            moveDirection.y = 0;
        }

    }

    private void FixedUpdate ()
    {
        rb.AddForce (transform.TransformDirection(moveDirection), ForceMode.Impulse);

        temp = rb.velocity;
        temp.x *= 1 - drag;
        temp.z *= 1 - drag;
        rb.velocity = temp;

        cam.transform.localEulerAngles = camRot;
        transform.localEulerAngles = playerRot;

        if (addGravity)
        {
            rb.AddForce (Vector3.down * gravity);
        }

    }

}
