using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : MonoBehaviour
{

    [SerializeField] GameObject harpoon;
    [SerializeField] GameObject bodyHarpoon;
    [SerializeField] LineRenderer LR;
    [SerializeField] SpringJoint2D SJ2D;
    [SerializeField] int harpoonForce;
    [SerializeField] int playerForce;
    private HarpoonStick harp;
    [SerializeField] Transform lineTran;
    private bool distanceCalculated;
    [HideInInspector] public bool anchored;
    [HideInInspector] public float potentialHeight;
    private Movement movement;
    [SerializeField] float minDistance;
    [SerializeField] float reelSpeed;

    // Start is called before the first frame update
    void Start()
    {
        distanceCalculated = false;
        anchored = false;
        potentialHeight = 0;
        movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (harp != null)
            {
                Destroy(harp.gameObject);
            }
            else
            {
                harp = Instantiate(harpoon, bodyHarpoon.transform.position, bodyHarpoon.transform.rotation).GetComponent<HarpoonStick>();
                bodyHarpoon.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                harp.GetComponent<Rigidbody2D>().AddForce(harpoonForce * bodyHarpoon.transform.right);
                GetComponent<Rigidbody2D>().AddForce(playerForce * -1 * bodyHarpoon.transform.right);
            }
        }

        if(Input.GetMouseButton(1))
        {

            if (SJ2D.distance > minDistance)
            {

                SJ2D.distance -= reelSpeed;

            }

        }

        if (harp != null)
        {
            LR.enabled = true;
            LR.SetPosition(0, lineTran.position);
            LR.SetPosition(1, harp.transform.position);
            SJ2D.connectedAnchor = harp.transform.position;
            if (harp.stuck)
            {
                SJ2D.enabled = true;
                if (movement.onGround||distanceCalculated == false)
                {
                    GetDistance();
                }
                distanceCalculated = true;
            }
            else
            {
                distanceCalculated = false;
                SJ2D.enabled = false;
                anchored = false;
                
            }
        }
        else
        {
            distanceCalculated = false;
            LR.enabled = false;
            SJ2D.enabled = false;
            anchored = false;
        }
    }

    void GetDistance()
    {
        float d = Vector2.Distance(transform.position, harp.transform.position) / 1.35f;
        SJ2D.distance = d;
        potentialHeight = (transform.position.y) - (harp.transform.position.y - d);
        anchored = true;
    }


}
