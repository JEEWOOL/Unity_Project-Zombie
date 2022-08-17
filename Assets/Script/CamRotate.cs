using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    //public Transform playerCamera;
    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;

    void FixedUpdate()
    {
        if (GameManager.gm.gmState != GameManager.GameState.Go)
        {
            return;
        }

        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90, 90f);

        //Vector3 dir = new Vector3(-mouse_Y, mouse_X, 0);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
