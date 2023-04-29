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

    public Animator animator;
    public AudioClip Walking_Sound, Running_Sound, Jump_Sound;
    public AudioSource MouseAudio;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool isGrounded;


    private void Start()
    {

    }

    void Update()
    {
        #region Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);//Check Grounded

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //gravity
        }
        #endregion

        #region Input & Movement

        float x = Input.GetAxis("Horizontal");//AD
        float z = Input.GetAxis("Vertical");//WS

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * Gravity);
            animator.SetTrigger("IsJumping");
            MouseAudio.clip = Jump_Sound;
            MouseAudio.Play();
        }

        Vector3 move = transform.right * x + transform.forward * z;
        Controller.Move(move * speed * Time.deltaTime);//Movement

        velocity.y += Gravity * Time.deltaTime;

        Controller.Move(velocity * Time.deltaTime);//Jump

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = spritSpeed;//Sprint
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 3f;//Walk
        }

        #endregion

        #region Animations & Sounds

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            animator.SetBool("IsMoving", true);
            MouseAudio.clip = Walking_Sound;
            MouseAudio.Play();
        }

        else if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetBool("IsRunning", true);
            MouseAudio.clip = Running_Sound;
            MouseAudio.Play();
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsRunning", false);
        }

        #endregion

    }
}