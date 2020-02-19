using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    
    public Transform CameraTarget;
    public Vector3 dif;
    void Start() {
        GameObject obj = GameObject.Find("Body1");
        CameraTarget = obj.transform;
        dif = CameraTarget.position - transform.position;
        Debug.Log(dif);   
    }
    void Update()
    {
       transform.position = CameraTarget.position - dif;
    }
}
