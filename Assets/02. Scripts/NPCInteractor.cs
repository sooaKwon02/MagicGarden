using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NPCInteractor : MonoBehaviour
{
    public Transform[] points; // 이동 경로 지점
    private NavMeshAgent agent;
    private int currPoint = 0;

    public NPCTalk talk; 
    public GameObject talkPanel;
    public TMP_Text talkText;

    private bool playerRange = false;
    private Coroutine coroutine;

    public Transform player;

    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (points.Length > 0)
        {
            agent.SetDestination(points[0].position);
        }
    }

    void Update()
    {
        float speed = agent.velocity.magnitude;
        Debug.Log($"Agent Speed: {speed}");
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currPoint = (currPoint + 1) % points.Length;
            agent.SetDestination(points[currPoint].position);
        }

        if (playerRange && Input.GetKeyDown(KeyCode.Space))
        {
            RandomTalk();
        }

        if (agent.isStopped && player != null)
        {
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
        }
    }

    private void RandomTalk()
    {

        if (talk != null && talk.dialogueLines.Length > 0)
        {
            int randomIndex = Random.Range(0, talk.dialogueLines.Length);
            string randomLine = talk.dialogueLines[randomIndex];

            talkPanel.SetActive(true);
            talkText.text = randomLine;

            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);

            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(OffPanel(3f));
        }
    }

    private IEnumerator OffPanel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        talkPanel.SetActive(false);

        agent.isStopped = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRange = false;
            talkPanel.SetActive(false);
        }
    }
}
