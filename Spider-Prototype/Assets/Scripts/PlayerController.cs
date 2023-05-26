using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    public float connectionForce = 500f;
    [SerializeField] float movementSpeed;    
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = 9.81f;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;

    [SerializeField] GameObject web;
    [SerializeField] Animator anim;
    [SerializeField] bool canJump = true;
    public bool swing, oneForce;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;  // Titreme sorununu azaltmak için interpolasyonu etkinleþtir
        

        swing = web.GetComponent<WebController>().paste;
    }

    private void Update()
    {
        swing = web.GetComponent<WebController>().paste;

        AnimaControl();
    }
    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Hareket yonunu hesapla
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement = Camera.main.transform.TransformDirection(movement).normalized;
        movement.y = 0f;
        movement.Normalize();
        movement *= movementSpeed;
        

        
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

       
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Plane"))
        {
            canJump = true;
        }
    }


    public void AnimaControl()
    {
        if (rb.velocity == Vector3.zero)
        {
            anim.SetTrigger("idle");
        }

        if (verticalInput > 0)
        {


            if (transform.position.y <= 3f)
            {
                if (swing == false)
                {
                    anim.SetBool("fall", false);
                    anim.ResetTrigger("swing");
                    anim.SetTrigger("move");
                }
            }
            else if (swing == true)
            {

                anim.SetBool("fall", false);
                anim.ResetTrigger("swing");
                anim.ResetTrigger("move");


            }

            else if (swing == false)
            {
                if (transform.position.y >= 3f)
                {
                    anim.SetBool("fall", true);
                    anim.ResetTrigger("swing");
                    anim.ResetTrigger("move");
                }
            }


        }

        
        if (canJump && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            anim.SetTrigger("jump");
        }

        if (swing == true)
        {
            anim.SetTrigger("swing");
        }
        else if (swing == false)
        {
            anim.ResetTrigger("swing");
        }
    }
}
