using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] Transform start, end;
    [SerializeField] int speed;
    private Rigidbody2D RB;
    bool toggled = false;
    bool stopped = true;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void CollisionCheck(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (stopped == true)
            {
                stopped = false;
                if (toggled)
                {
                    toggled = false;
                    StartCoroutine(Move(start.position));
                }
                else
                {
                    toggled = true;
                    StartCoroutine(Move(end.position));
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionCheck(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CollisionCheck(collision);
    }

    private IEnumerator Move(Vector3 endpoint)
    {
        yield return new WaitForSeconds(.5f);
        float dis = Vector2.Distance(transform.position, endpoint);
        while (dis > .1f)
        {
            RB.velocity = Vector3.Normalize(endpoint-transform.position)*speed;
            yield return new WaitForFixedUpdate();
            dis = Vector2.Distance(transform.position, endpoint);
        }
        RB.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(.5f);
        stopped = true;
    }
}
