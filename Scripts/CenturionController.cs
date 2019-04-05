using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenturionController : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5;
    [Tooltip ("Must be positive")]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpHeight = 5;
    [SerializeField] float camSensitivity = 5;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float maxHealth = 10;
    [Range (0, 1)]
    [SerializeField] float smoothVal = 0.5f;
    [SerializeField] float shotsPerSecond = 2;
    [SerializeField] float damage = 2;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;
    [SerializeField] ParticleSystem gunShot;
    [SerializeField] Animator anim;
    Vector3 moveDirection;
    Rigidbody rb;
    Vector3 camRot = Vector3.zero;
    Vector3 playerRot = Vector3.zero;
    GameObject cam;
    Camera camCam;
    bool canJump;
    bool hasdashed = false;
    bool dashing = false;
    bool canShoot;
    bool doubleJumped = false;
    bool firstHit = false;
    float t;
    float currentY;
    float startSpeed;
    float health;
    float fovChange = 90;
    float maxSpeed;
    float dashSpeed = 5;
    float baseSpeed;
    float fireWait;
    float startGrav;

    void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        startGrav = gravity;
        baseSpeed = moveSpeed;
        maxSpeed = moveSpeed * dashSpeed;
        rb = GetComponent<Rigidbody> ();
        rb.useGravity = false;
        gravity = gravity * Time.deltaTime;
        startSpeed = moveSpeed;
        cam = transform.GetChild (0).gameObject;
        camCam = cam.GetComponent<Camera> ();
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Escape)) ToggleMoose ();

        #region Movement and Look
        camCam.fieldOfView = Mathf.Lerp (camCam.fieldOfView, 60 + (fovChange * (rb.velocity.magnitude / maxSpeed)), smoothVal);
        t = Time.deltaTime;
        currentY = moveDirection.y;
        moveDirection.y = 0;
        moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), moveDirection.y, Input.GetAxisRaw ("Vertical"));
        moveDirection = moveDirection.normalized * moveSpeed;
        if (Input.GetButtonDown ("Sprint"))
        {
            if (canJump)
            {
                moveSpeed *= 1.5f;
            }
            else
            {
                if (dashing)
                {
                    StartCoroutine (SlowDash ());
                }
                if (!hasdashed)
                {
                    StartCoroutine (Dash ());
                    hasdashed = true;
                }
            }
        }
        if (Input.GetButtonUp ("Sprint"))
        {
            if (!dashing)
            {
                moveSpeed = startSpeed;
            }
        }

        moveDirection.y = currentY;

        if (canJump)
        {
            if (Input.GetButtonDown ("Jump"))
            {
                moveDirection.y = jumpHeight;
                doubleJumped = true;
            }
        }
        if (!doubleJumped)
        {
            if (Input.GetButtonDown ("Jump"))
            {
                doubleJumped = false;
                moveDirection.y = jumpHeight;
            }
        }
        if (!canJump)
        {
            moveDirection.y -= gravity;
        }

        if (moveDirection.y < 0)
        {
            gravity = startGrav * 1.5f * t;
        }
        else
        {
            gravity = startGrav * t;
        }

        rb.velocity = transform.TransformDirection (moveDirection);

        playerRot.y += Input.GetAxisRaw ("Mouse X") * camSensitivity;
        camRot.x -= Input.GetAxisRaw ("Mouse Y") * camSensitivity;

        camRot.x = Mathf.Clamp (camRot.x, -90, 90);

        cam.transform.localEulerAngles = camRot;
        transform.localEulerAngles = playerRot;
        #endregion

        if (Input.GetButton ("Fire"))
        {
            Shoot ();
        }
        else
        {
            if(fireWait != 0)
            {
                FireWait ();
            }
        }
            

    }

    void Shoot ()
    {
        if(projectile != null)
        {
            if(fireWait == 0)
            {
                Destroy(Instantiate (projectile, firePoint.position, cam.transform.rotation), 5);
                gunShot.Play ();
                anim.SetTrigger ("Shoot");
            }
            FireWait ();
        }
    }

    void FireWait ()
    {
        fireWait += t * shotsPerSecond;
        if (fireWait >= 1)
        {
            fireWait = 0;
        }
    }

    public IEnumerator Dash ()
    {
        dashing = true;
        moveSpeed *= dashSpeed;
        yield return new WaitForSeconds (dashTime);
        StartCoroutine (SlowDash ());
    }

    IEnumerator SlowDash ()
    {
        while (dashing)
        {
            moveSpeed -= (moveSpeed / 6);
            yield return new WaitForSeconds ((dashTime / 2));
        }
    }

    public void TakeDamage (float _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Die ();
        }
    }

    private void Die ()
    {
        Debug.Log ("Player died");
    }

    public void AddForce (ExplosionData data)
    {
        rb.AddExplosionForce (data.force, data.position, data.radius);
    }

    public void SetHasDashed (bool val)
    {
        hasdashed = val;
    }

    public void SetCanJump (bool val)
    {
        canJump = val;
    }

    public void SetMoveSpeed (float val)
    {
        moveSpeed = val;
    }

    public void SetDashing (bool val)
    {
        dashing = val;
    }

    public void SetMoveDirY(float val)
    {
        moveDirection.y = val;
    }

    public float BaseSpeed ()
    {
        return baseSpeed;
    }

    private void OnCollisionEnter (Collision collision)
    {
        if(Vector3.Angle(collision.contacts[0].point - transform.position, Vector3.down) < 30)
        {
            if (collision.gameObject.CompareTag ("Walkable"))
            {
                gravity = startGrav;
                hasdashed = false;
                canJump = true;
                moveDirection.y = 0;
                StopCoroutine (Dash ());
                moveSpeed = startSpeed;
                dashing = false;
                doubleJumped = false;
            }

        }
    }

    private void OnCollisionStay (Collision collision)
    {
        if (Vector3.Angle (collision.contacts [0].point - transform.position, Vector3.down) < 30)
        {
            if (collision.gameObject.CompareTag ("Walkable"))
            {
                canJump = true;
            }

        }
    }

    private void OnCollisionExit (Collision collision)
    {
        
        if (collision.gameObject.CompareTag ("Walkable"))
        {
            canJump = false;
        }
        
    }

    public float Damage ()
    {
        return damage;
    }

    void ToggleMoose ()
    {
        Cursor.visible = !Cursor.visible;
        if (Cursor.visible) Cursor.lockState = CursorLockMode.None; else Cursor.lockState = CursorLockMode.Locked;

    }

}
