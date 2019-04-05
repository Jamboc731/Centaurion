using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

    [SerializeField] protected float moveSpeed = 600;
    [SerializeField] string targetTag = "noTag";
    protected Rigidbody rb;
    protected Transform target;

    private void Start ()
    {
        Init ();
    }

    protected virtual void Init ()
    {
        if(targetTag == "")
        {
            targetTag = "noTag";
        }
        rb = GetComponent<Rigidbody> ();
        rb.drag = 0;
        rb.mass = 0;
        transform.LookAt (target);
        Move ();
    }

    protected virtual void Move ()
    {
        //Debug.Log (moveSpeed);
        rb.velocity = transform.forward * moveSpeed;
    }

    protected void OnCollisionEnter (Collision collision)
    {
        if (targetTag != null)
        {
            if (collision.collider.CompareTag (targetTag))
            {
                Debug.Log ("hit target tag");
                DamageEnemy (collision.gameObject);
            }
        }
    }

    protected virtual void DamageEnemy (GameObject hit)
    {

    }

}
