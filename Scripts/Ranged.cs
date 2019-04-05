using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : Enemy
{

    [SerializeField] float distanceToShootAt = 10;
    [Tooltip("The random variation from the distance to shoot that the enemy will have just for a bit of variety ya know")] 
    [SerializeField] float randomRangeModifier = 3;
    [SerializeField] float shotsPerSecond = 1;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform firePoint;

    float fireDelay;
    float wait = 0;
    bool canShoot = false;
    bool shoot;

    protected override void Move ()
    {
        fireDelay = t * shotsPerSecond;
        if(Vector3.Distance(transform.position, player.transform.position) > distanceToShootAt + Random.Range(-randomRangeModifier, randomRangeModifier))
        {
            canShoot = false;
            base.Move ();
        }
        else
        {
            moveDirection = Vector3.zero;
            canShoot = true;
            //Debug.Log ("at shooting distance");
        }
        Shoot ();
    }
    
    private void Shoot ()
    {
        if (canShoot)
        {
            if(wait == 0)
            {
                Destroy(Instantiate (projectile, firePoint.position, Quaternion.identity), 5);
                //Debug.Log ("shot");
            }
        }
        if(wait == 0)
        {
            if(canShoot)
                wait += fireDelay;
        }
        else
        {
            wait += fireDelay;
        }
        if(wait >= 1)
        {
            wait = 0;
        }
    }

}
