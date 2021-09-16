using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private Rigidbody2D RB;
    private Animator ANIM;
    public bool onGround;
    [SerializeField] int acceleration;
    [SerializeField] int airAcceleration;
    [SerializeField] int speed;
    [SerializeField] int maxSpeed;
    private bool isFlipped;
    private Harpoon harpoon;
    private bool initiateJump;

    // Start is called before the first frame update
    void Start()
    {
        initiateJump = false;
        isFlipped = false;
        harpoon = GetComponent<Harpoon>();
        onGround = false;
        ANIM = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
    }

    // Fixed Update
    // Put your physics stuff here.
    void FixedUpdate()
    {
        if (!harpoon.anchored && !onGround)
        {
            RB.gravityScale = 7;
            RB.AddForce(new Vector2(Input.GetAxis("Horizontal") * airAcceleration, 0));
        }
        else if(onGround)
        {
            RB.AddForce(new Vector2(Input.GetAxis("Horizontal") * acceleration, 0));
            if (RB.velocity.x > maxSpeed)
            {
                RB.velocity = new Vector2(maxSpeed, RB.velocity.y);
            } else if (RB.velocity.x < -maxSpeed)
            {
                RB.velocity = new Vector2(-maxSpeed, RB.velocity.y);

            }
            RB.gravityScale = 5;
        }
        else
        {
            RB.AddForce(new Vector2(Input.GetAxis("Horizontal") * airAcceleration, 0));
            RB.gravityScale = 7f;
        }

        if (onGround)
        {

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > .005f)
            {
                if (Input.GetAxisRaw("Horizontal") > .005f)
                {
                    ANIM.SetBool("MovingLeft", false);
                }
                else
                {
                    ANIM.SetBool("MovingLeft", true);
                }
                ANIM.SetBool("Moving", true);
            }
            else
            {
                RB.velocity = RB.velocity / new Vector2(1.1f, 1);
                ANIM.SetBool("Moving", false);

            }

            if (RB.velocity.x > speed)
            {
                RB.velocity = new Vector2(speed, RB.velocity.y);
            }
            if (RB.velocity.x < -speed)
            {
                RB.velocity = new Vector2(-speed, RB.velocity.y);
            }
        }
        else
        {
            if (RB.velocity.x > 0)
            {
                if(!isFlipped)
                    ANIM.SetBool("MovingLeft", false);
                else
                    ANIM.SetBool("MovingLeft", true);

            }
            else
            {
                if(!isFlipped)
                    ANIM.SetBool("MovingLeft", true);
                else
                    ANIM.SetBool("MovingLeft", false);

            }
        }
        //change this so that it only happens when touching ground;
    }


    // Update called once per frame
    private void Update()
    {
        if(transform.position.x> Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFlipped = true;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFlipped = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            ANIM.SetBool("InAir", false);
            onGround = true;
            initiateJump = false;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ANIM.SetBool("InAir", false);
            onGround = true;
            initiateJump = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ANIM.SetBool("InAir", true);
            initiateJump = true;
            StartCoroutine(Jump());
        }
    }

IEnumerator Jump()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (initiateJump)
        onGround = false;
    }

}
