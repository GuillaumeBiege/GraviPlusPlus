using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool IsPressed = false;

    private Rigidbody rb = default;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPressed)
        {
            DragBall();
        }
    }

    void DragBall()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.position = mousePos;
    }

    private void OnMouseDown()
    {
        IsPressed = true;
        rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        IsPressed = false;
        rb.isKinematic = false;
    }
}
