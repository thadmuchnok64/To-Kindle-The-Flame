using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonStick : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D RB;
    public bool stuck;
    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        stuck = false;
    }

    private void FixedUpdate()
    {
        if (stuck == false)
        {
            float deg = Mathf.Rad2Deg * (Mathf.Atan2(RB.velocity.y, RB.velocity.x));
            transform.eulerAngles = new Vector3(0, 0, deg);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            stuck = true;
            RB.isKinematic = true;
            RB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}
