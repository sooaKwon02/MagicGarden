using UnityEngine;

public class MyCamera : MonoBehaviour
{
    // Inspector에서 설정할 수 있도록 public으로 선언
    public float Yaxis;
    public float Xaxis;

    public Transform target; // Player

    public float rotSensitive = 3f;
    public float dis = 15f;
    public float RotationMin = -10f;
    public float RotationMax = 50f;
    public float smoothTime = 0.12f;

    private Vector3 targetRotation;
    private Vector3 currentVel;

    void Start()
    {
        // 만약 Inspector에서 값이 기본값(0)인 경우에만 초기화하도록 한다.
        // 예를 들어, Xaxis와 Yaxis가 0이면 초기화하고, 그렇지 않으면 Inspector 값을 유지.
        if (Mathf.Approximately(Xaxis, 0f) && Mathf.Approximately(Yaxis, 0f))
        {
            Vector3 initRot = transform.eulerAngles;
            Xaxis = initRot.x;
            Yaxis = initRot.y;
        }
        targetRotation = new Vector3(Xaxis, Yaxis, 0f);
    }

    void LateUpdate()
    {
        // 마우스 입력에 따라 회전값 변경
        Yaxis += Input.GetAxis("Mouse X") * rotSensitive;
        Xaxis -= Input.GetAxis("Mouse Y") * rotSensitive;

        Xaxis = Mathf.Clamp(Xaxis, RotationMin, RotationMax);

        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(Xaxis, Yaxis, 0f), ref currentVel, smoothTime);
        transform.eulerAngles = targetRotation;

        transform.position = target.position - transform.forward * dis;
    }
}
