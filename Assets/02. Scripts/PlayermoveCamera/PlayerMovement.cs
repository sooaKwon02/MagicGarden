using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float sprintMultiplier = 1.3f;
    public Animator animator;
    public Transform cameraTransform;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;
        if (Input.GetKey(KeyCode.W)) vertical += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        // 만약 현재 방향이 반대로 움직인다면, 앞에 -1을 곱해서 반전
        Vector3 inputDir = (camForward * vertical + camRight * horizontal).normalized;

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = sprint ? speed * sprintMultiplier : speed;

        if (inputDir != Vector3.zero)
        {
            transform.position += inputDir * currentSpeed * Time.deltaTime;

            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.25f);

            if (animator)
            {
                animator.SetFloat("Speed", currentSpeed);
                //animator.SetBool("isSprint", sprint);
            }
        }
        else
        {
            if (animator)
            {
                animator.SetFloat("Speed", 0f);
                //animator.SetBool("isSprint", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 1f);
            foreach (var hit in hits)
            {
                FieldItems fieldItem = hit.GetComponent<FieldItems>();
                if (fieldItem != null)
                {
                    animator.SetTrigger("Dig");
                    fieldItem.Dig();
                }
            }
        }
    }
}
