using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Scripts.Enemy

{
    namespace girlHostile
    {
        public class EnemyAi : MonoBehaviour
        {
            [Header("Scripts")]
           

            [Header("Variables")]
            [SerializeField,Range(0f, 50f)] private float wanderRadius;
            [SerializeField,Range(0f, 50f)] private float chase_range,agentstoppingdistance;
            [SerializeField,Range(0f, 50f)] private float attack_distance;          


            [Header("Components")]
            [SerializeField] public GameObject player; 
            [SerializeField] public NavMeshAgent agent;
            [SerializeField] private Animator animator;

            [Header("Booleans")]
            [SerializeField] private bool cooldown;
            [SerializeField] private bool ReadyToHit = true;
            [SerializeField] public bool angry;

            [Header("Markers")]
           
            private Vector3 newPos;
           
            private void Awake()
            {
                agent.stoppingDistance = agentstoppingdistance;
                //Tracker because gameobject being tracked by navmesh should be grounded
                agent.destination = player.transform.position;
                cooldown = false;
            }

            private void OnEnable()
            {
                agent.destination = player.transform.position;
                cooldown = false;
                Animations(0, 0);
                newPos = RandomNavSphere(player.transform.position, wanderRadius, -1);
            }
            private void Update()
            {
             //   Debug.Log(Vector3.Distance(transform.position, player.transform.position));
                //if agent is active
                
                    //if girl is in chase mode
                    if (!angry&&agent.enabled)
                    {
                        //if girl hasnt just attacked
                        if (!cooldown)
                        {
                            //if player is in front of girl
                            if (Vector3.Distance(player.transform.position, transform.position) < chase_range)
                            {
                                    agent.SetDestination(player.transform.position);
								//Trigger Chase Animation
							
                                    Animations(2, 0);
                                    Attack();
                            }

                            //if player is not in front of girl
                            else 
                            {
                                ChangePos();
                            }
                        }
                    }
					else if(angry)
					{
                        //Float Around
                        Animations(3, 0);
					}
                
            }
            void ChangePos()
            {
			
                if (Vector3.Distance(transform.position, newPos) < 3)
                {
					
                        Animations(0, 0);
                     

                    newPos = RandomNavSphere(player.transform.position, wanderRadius, -1);
                }

                else
                {
					
                        Animations(1, 0);
                     
                }
                agent.SetDestination(newPos);
            //    Debug.Log(Vector3.Distance(transform.position, newPos));
            }

            private void Attack()
            {
                //if player is too close
                if (Vector3.Distance(player.transform.position, transform.position) < attack_distance)
                {

                }


            }
         

            public  Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
            {
                Vector3 randDirection = Random.insideUnitSphere * dist;

                randDirection += origin;

                NavMeshHit navHit;

                NavMesh.SamplePosition(randDirection, out navHit, 5, layermask);
               
                return navHit.position;
            }
            /* walk state 0 is idle 
             * walk state 2 is chasing
             * walk state 1 is walking 
             */
            public void Animations(int walk_state, int hit_state)
            {
                animator.SetInteger("Hit_State", hit_state);
                animator.SetInteger("Walk_State", walk_state);
            }

         

            void ReActivate()
            {
              //  agent.Warp(newPos);
                cooldown = false;
                agent.isStopped = false;
                ReadyToHit = true;
            }

           

           public void ChangeGirl()
			{
                angry = true;
              //  BoxVolume.SetActive(false);
                agent.enabled = false;
			}

           
        }
    }
}