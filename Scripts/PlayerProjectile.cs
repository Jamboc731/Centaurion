using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{

    CenturionController player;

    protected override void Init ()
    {
        player = GameObject.Find ("Player").GetComponent<CenturionController> ();
        moveSpeed = 60;
        target = GameObject.Find ("Main Camera").transform;
        base.Init ();
    }

    protected override void Move ()
    {
        transform.rotation = target.rotation;
        base.Move ();
    }

    protected override void DamageEnemy (GameObject hit)
    {
        hit.GetComponent<Enemy> ().TakeDamage (player.Damage ());
    }

}
