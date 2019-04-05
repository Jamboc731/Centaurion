using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{

    protected override void Init ()
    {
        target = GameObject.Find ("Player").transform;

        base.Init ();
    }

}
