using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected float moveSpeed = 5;
    [SerializeField] protected float maxHealth = 10;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float maxSpeed = 10;
    [Range(0, 1)]
    [SerializeField] protected float lookSmoothing = 0.65f;
    [SerializeField] protected ParticleSystem exploded;

    protected GameObject player;
    protected Rigidbody rb;
    protected CenturionController pControl;
    protected Vector3 directionToPlayer;
    protected Vector3 moveDirection;
    protected Vector3 temp;
    protected float health;
    protected new string name;
    protected float t;

    private void Start ()
    {
        Init ();   
    }

    private void Update ()
    {
        t = Time.deltaTime;
        LookAtPlayer ();
        Move ();
    }

    protected virtual void Init ()
    {
        player = GameObject.Find ("Player");
        pControl = player.GetComponent<CenturionController> ();
        rb = GetComponent<Rigidbody> ();
        rb.useGravity = false;
        name = gameObject.name;
        health = maxHealth;
    }

    protected void LookAtPlayer ()
    {
        //directionToPlayer = player.transform.position - transform.position;
        //transform.eulerAngles = Vector3.Slerp (transform.eulerAngles, directionToPlayer, lookSmoothing);
        transform.LookAt (player.transform);
        
    }

    protected virtual void Move ()
    {
        //Debug.Log (rb.velocity.magnitude);
        temp = transform.forward.normalized * (moveDirection.magnitude + (moveSpeed * t));
        if (Mathf.Abs (temp.magnitude) < maxSpeed)
        {
            moveDirection = transform.forward.normalized * (moveDirection.magnitude + (moveSpeed * t));

        }
        else
            moveDirection = transform.forward.normalized * (moveDirection.magnitude);

        rb.velocity = moveDirection;
    }

    public virtual void TakeDamage (float _damage)
    {
        health -= _damage;
        if(health <= 0)
        {
            Die ();
        }
        else
            Debug.Log (string.Format ("{0} took {1} points of damage and is now on {2} health", name, _damage, health));
    }

    public virtual void Die ()
    {
        Debug.Log (string.Format ("{0} died", name));
        if (exploded != null)
            exploded.Play ();
        else Debug.Log ("splode is wrong");
        Destroy (gameObject);
    }

    public virtual void Explode ()
    {
        Debug.Log (string.Format ("{0} exploded", name));
    }

    protected virtual void HitPlayer(float _damage)
    {
        pControl.TakeDamage (_damage);
    }

}
