using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private bool IsPressed = false;

    private Rigidbody rb = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DragBall();
    }

    void DragBall()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RayCast();

            if (hit.collider != null)
            {
                Ball ball = hit.collider.gameObject.GetComponent<Ball>();

                if (ball != null)
                {
                    IsPressed = true;
                    rb.isKinematic = true;
                   
                    Debug.Log("Raycast succesfull");
                }
                else
                {
                    IsPressed = false;
                    //rb.isKinematic = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            
            IsPressed = false;
            rb.isKinematic = false;

        }

        if (IsPressed)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            rb.position = new Vector3(worldPosition.x, 0f, worldPosition.y);
        }
        
    }
    
    //Raycast function for the drag mechanic based on mouse position
    private RaycastHit RayCast()
    {
        Vector3 ScreenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 ScreenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 WorldMousePosFar = Camera.main.ScreenToWorldPoint(ScreenMousePosFar);
        Vector3 WorldMousePosNear = Camera.main.ScreenToWorldPoint(ScreenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(WorldMousePosNear, WorldMousePosFar - WorldMousePosNear, out hit);

        return hit;
    }

    //private void OnMouseDown()
    //{
    //    IsPressed = true;
    //    rb.isKinematic = true;
    //}

    //private void OnMouseUp()
    //{
    //    IsPressed = false;
    //    rb.isKinematic = false;
    //}
}
