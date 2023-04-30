using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;
    private float speed;
    public float spritSpeed = 15f;
    public float normalSpeed = 3f;
    public float Gravity = -19.01f;
    public float jumpheight = 2f;
    public bool is_Running, is_Jumping = false;

    public Animator animator;
    public AudioClip Walking_Sound, Running_Sound, Jump_Sound;
    public AudioSource MouseAudio, JumpSource;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public static PlayerMovement instance;

    Vector3 velocity;
    public bool isGrounded,ImmobilizedOrDead;

    public _PlayerMovement movementScript;

    private void Start()
    {
        if (instance == null) { instance = this; }
        Spawn();
        movementScript = new _PlayerMovement();
        movementScript.Enable();
        movementScript.Move.Movement.started += ctx => { MoveStarted(); };
        movementScript.Move.Movement.canceled += ctx => { MoveEnded(); };
        movementScript.Move.Jump.started += ctx => Jump();

        movementScript.Move.Run.started += ctx => { Toggle_Run(); MoveStarted(); };
        movementScript.Move.Run.canceled += ctx => { MoveEnded(); Toggle_Run(); };

    }
    void FixedUpdate()
    {
       if(!ImmobilizedOrDead) Move(movementScript.Move.Movement.ReadValue<Vector2>());
        #region Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);//Check Grounded
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //gravity
        }
        velocity.y += Gravity * Time.deltaTime;
        if (is_Jumping)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * Gravity);
        }
        Controller.Move(velocity * Time.deltaTime);//Jump

        #endregion
    }

    void Move(Vector2 Input)
    {
        Vector3 move = transform.right * Input.x + transform.forward * Input.y;
        if (is_Running)
        {
            Controller.Move(move * spritSpeed * Time.deltaTime);
        }
        else
        {
            Controller.Move(move * normalSpeed * Time.deltaTime);//Movement
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            animator.SetTrigger("IsJumping");
            JumpSource.Play();
            is_Jumping = true;
            StartCoroutine("EndJump");
        }
    }
    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(0.1f);
        is_Jumping = false;
    }

    void MoveStarted()
    {
        if (!ImmobilizedOrDead)
        {
            if (is_Running)
            {
                animator.SetBool("IsRunning", true);
                MouseAudio.clip = Running_Sound;
                MouseAudio.Play();
            }
            else
            {
                animator.SetBool("IsMoving", true);
                MouseAudio.clip = Walking_Sound;
                MouseAudio.Play();
            }
        }
    }

    void MoveEnded()
    {
        if (!ImmobilizedOrDead)
        {
            if (!is_Running)
            {
                animator.SetBool("IsMoving", false);
            }
            animator.SetBool("IsRunning", false);
            MouseAudio.Stop();
        }
    }

    void Toggle_Run()
    {
        is_Running = !is_Running;
    }

    public void Spawn()
    {
        ImmobilizedOrDead = false;
        GameObject Spawner = GameObject.FindGameObjectWithTag("PlayerSpawner");
        Controller.enabled = false;
        transform.position = Spawner.transform.position;
        Controller.enabled = true;
    }


    public void Eat()
    {
        animator.SetTrigger("IsEating");
    }
    public void Die()
    {
        animator.SetTrigger("IsDed");
        MouseAudio.Stop();
    }

}