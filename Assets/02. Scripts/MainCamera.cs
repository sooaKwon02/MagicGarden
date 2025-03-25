using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Follow Settings")]
    public float delayTime = 10.0f;   //따라가는 속도

    [Header("Mouse Rotation")]
    public float rotSpeed = 200f;     //마우스 회전 속도
    [Range(-80, 80)]
    public float minXAngle = -45f;    //X축 최소 각도
    [Range(-80, 80)]
    public float maxXAngle = 45f;     
    private GameObject cameraTarget;  
    private Vector3 relativePosition; 
    private float angleX;           
    private float angleY;            

    void Start()
    {
        cameraTarget = GameObject.FindWithTag("Player");

        if (cameraTarget)
        {
            relativePosition = transform.position - cameraTarget.transform.position;
        }

       
        Vector3 currentEuler = transform.eulerAngles;
        angleX = -currentEuler.x; 
        angleY = currentEuler.y;
    }

    void Update()
    {
        if (!cameraTarget) return;

        Vector3 targetPos = cameraTarget.transform.position + relativePosition;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * delayTime);

        float mx = Input.GetAxis("Mouse X");  // 좌우 이동
        float my = Input.GetAxis("Mouse Y");  // 상하 이동

        angleX += my * rotSpeed * Time.deltaTime; 
        angleY += mx * rotSpeed * Time.deltaTime;   
        angleX = Mathf.Clamp(angleX, minXAngle, maxXAngle);

        Quaternion mouseRotation = Quaternion.Euler(-angleX, angleY, 0);

        transform.rotation = mouseRotation;
    }
}
