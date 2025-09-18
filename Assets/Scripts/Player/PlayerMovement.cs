using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;

    public Animator animator;

    public float dashSpeed;
    public float dashCount;
    private float dashCounter = 0, dashCoolDown = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //Movement
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(inputX * speed, inputY * speed);

        animator.SetFloat("Run", (Mathf.Abs(inputX) + Mathf.Abs(inputY)) * speed);

        //turn the sprite left-right by the moving side
        if (inputX > 0)
        {
            transform.localScale = new Vector2(-1.0f, 1.0f);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector2(1.0f, 1.0f);
        }

        Dash();




    }

    private void Dash()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && dashCoolDown <= 0)
        {
            StartCoroutine(Dashing());
            dashCounter++;
        }

        

        if(dashCounter >= dashCount)
        {
            dashCoolDown = 1.0f;
            dashCounter = 0;
            speed = dashSpeed;
        }

        if(dashCoolDown > 0)
        {
            dashCoolDown -= Time.deltaTime;
        }
    }

    private IEnumerator Dashing()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(0.1f);
        speed = 4f;
    }



    
}
