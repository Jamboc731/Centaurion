using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holo : MonoBehaviour
{

    [SerializeField] Material [] mats;

    private void Update ()
    {
        foreach (var m in mats)
        {
            //m.mainTextureOffset = Time.time;
        }
    }

}
