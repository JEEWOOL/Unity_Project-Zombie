using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        if (target == null)
        {
            transform.forward = Camera.main.transform.forward;
        }
        else
        {
            transform.forward = target.transform.forward;
        }
    }
}
