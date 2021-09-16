using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D RB;
    [SerializeField] Transform player;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        RB.velocity = (player.position - transform.position)*10;
    }
}
