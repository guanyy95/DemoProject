using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMove : MonoBehaviour
{
    private float h;
    private float v;
    public float speed = 6;
    public float turnSpeed = 15;
    public Transform camTrans;
    private Vector3 movement;
    private Vector3 camForward;

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        
        transform.Translate(camTrans.right * h * speed * Time.deltaTime + camForward * v * speed * Time.deltaTime, Space.World);
        if (h != 0 || v != 0)
        {
            Rotating(h, v);
        }
    }
    
    
    private void Rotating(float hh, float vv)
    {
        camForward = Vector3.Cross(camTrans.right, Vector3.up);
        Vector3 targetDir = camTrans.right * hh + camForward * vv;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }
}
