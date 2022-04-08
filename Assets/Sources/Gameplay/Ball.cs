using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //References 
    private Rigidbody rb = default;
    private SpringJoint[] sjs = default;

    //Variables
    [SerializeField] private bool IsPressed = false;
    [SerializeField] private float releaseDelay = 0.25f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        sjs = GetComponents<SpringJoint>();
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
            Invoke("ReleaseBall", releaseDelay);
        }

        if (IsPressed)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            rb.position = new Vector3(worldPosition.x, 0f, worldPosition.z);
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

    private void ReleaseBall()
    {
        foreach (SpringJoint sj in sjs)
        {
            Destroy(sj);
        }
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
