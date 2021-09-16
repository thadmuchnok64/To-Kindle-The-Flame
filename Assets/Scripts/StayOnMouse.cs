using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnMouse : MonoBehaviour
{
    void GoToMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        GoToMouse();
    }
    private void LateUpdate()
    {
        GoToMouse();
    }
    
}
