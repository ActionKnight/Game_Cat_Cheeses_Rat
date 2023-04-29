using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAi : MonoBehaviour
{
    Vector3 randomDirection;
    [Range(0f,50f)]public float WalkRadius;
    [Range(0f, 5f)] public float StoppingDistance;
    [Range(0f, 5f)] public float AttackDistance;
    public NavMeshAgent agent;
    public bool is_Chasing;
    public GameObject Rat;

    public Animator animator;

    private bool move;

    Vector3 Destination;

    public static EnemyAi instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        StartCoroutine("StartOff");
    }
    private void Update()
    {
        if (move)
        {
            if (!is_Chasing)
            {
                if (Vector3.Distance(transform.position, Destination) < StoppingDistance)
                {
                    animator.SetBool("Walk", false);
                }
               
            }
            else
            {
                if (Vector3.Distance(transform.position, Rat.transform.position) < AttackDistance)
                {
                    Death(true);
                   
                }
                agent.SetDestination(Rat.transform.position);
            }
        }
    }

    Vector3 ChooseRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * WalkRadius + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, WalkRadius, 1);
        Vector3 finalPosition = hit.position;
        Instantiate(new GameObject("try"), finalPosition, Quaternion.identity);
        StartCoroutine("ReSearch");
        return finalPosition;
    }

    IEnumerator ReSearch()
    {
        yield return new WaitForSeconds(5);
        Destination = ChooseRandomPoint();
        agent.SetDestination(Destination);
        animator.SetBool("Walk", true);
    }
    IEnumerator StartOff()
    {
        yield return new WaitForSeconds(10);
        Destination = ChooseRandomPoint();
        agent.enabled = true;
        agent.SetDestination(Destination);
        animator.SetBool("Walk", true);
        move = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && move)
        {
            is_Chasing = true;
        }
    }

    public void Death(bool killedOrTimeOut)
    {
       
        if (killedOrTimeOut == true)
        {
            StartCoroutine("OnDeath",true);
        }
        else
        {
            StartCoroutine("OnDeath", false);
        }
    }

    IEnumerator OnDeath(bool death)
    {
        move = false;
        agent.enabled = false;
        PlayerMovement.instance.ImmobilizedOrDead = true;
        if (death)
        {
            animator.SetTrigger("Kill");
            yield return new WaitForSeconds(0.5f);
            PlayerMovement.instance.Die();
            yield return new WaitForSeconds(2.5f);
        }
        else
        {
            yield return new WaitForSeconds(0f);
        }
        GameManager.instance.ResetTimer();
        is_Chasing = false;
        GameManager.instance.LivesLeft--;
        PlayerMovement.instance.Spawn();

        //Reset Cat
        Destination = ChooseRandomPoint();
        agent.enabled = true;
        agent.SetDestination(Destination);
        animator.SetBool("Walk", true);
        move = true;
    }
}
