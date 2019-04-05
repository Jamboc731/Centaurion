using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseDrone : Enemy 
{
    [SerializeField] float recoil = 15;
    [SerializeField] float recoilTime = 1;
    bool normalMove = true;

    protected override void Move ()
    {
        if(normalMove)
            base.Move ();
    }

    private void OnCollisionEnter (Collision collision)
    {

        if (collision.collider.CompareTag ("Player"))
        {
            StartCoroutine (Recoil ());
            HitPlayer (damage);
            moveDirection = Vector3.zero;
            moveDirection -= transform.forward * recoil;
        }

    }

    IEnumerator Recoil ()
    {
        normalMove = false;
        yield return new WaitForSeconds (recoil);
        normalMove = true;
    }



}
