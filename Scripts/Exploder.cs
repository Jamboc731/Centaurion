using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : Enemy
{

    [SerializeField] float radius = 5;
    [SerializeField] float force = 5;
    [SerializeField] GameObject patricles;
    ExplosionData data;

    protected override void Init ()
    {
        base.Init ();
        data = new ExplosionData ();
    }

    protected override void Move ()
    {
        base.Move ();
        if(Vector3.Distance(transform.position, player.transform.position) < radius / 2)
        {
            HitPlayer (damage);
        }
    }

    protected override void HitPlayer (float _damage)
    {
        Instantiate (patricles, transform.position, Quaternion.identity);
        data.force = force;
        data.position = transform.position;
        data.radius = radius;
        pControl.AddForce (data);
        Destroy (gameObject);
    }

}

public struct ExplosionData
{
    public float force;
    public Vector3 position;
    public float radius;
}