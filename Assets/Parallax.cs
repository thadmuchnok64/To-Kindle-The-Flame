using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float ParallaxModifier;
    Vector2 CameraStartPos;
    Vector2 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        CameraStartPos = Camera.main.transform.position;
        StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 CameraCurrentPos = Camera.main.transform.position;
        transform.position = (StartPos+((CameraCurrentPos-CameraStartPos)*ParallaxModifier))*(new Vector3(1,1,0));
    }
}
