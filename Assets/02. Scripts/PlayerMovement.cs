using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.3f;
    public Animator animator;

    void Start()
    {
        // Animator 할당
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        if (Input.GetKey(KeyCode.W)) vertical += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;

        // 이동 방향 벡터
        Vector3 inputDir = new Vector3(-horizontal, 0, -vertical).normalized;

        bool sprint = Input.GetKey(KeyCode.LeftShift);

        float currentSpeed = sprint ? speed * sprintMultiplier : speed;

        if (inputDir != Vector3.zero)
        {
            // 실제 이동
            transform.position += inputDir * currentSpeed * Time.deltaTime;

            // 바라보는 방향 회전
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.25f);

            //애니메이션 파라미터 업데이트
            if (animator)
            {
                animator.SetFloat("Speed", currentSpeed);   // float 파라미터
                animator.SetBool("isSprint", sprint);       // bool 파라미터
            }
        }
        else
        {
            //입력이 없을 때
            if (animator)
            {
                animator.SetFloat("Speed", 0f);
                animator.SetBool("isSprint", false);
            }
        }
    }
}