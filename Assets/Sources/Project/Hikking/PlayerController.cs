using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    GameManager m_rGM = default;

    Rigidbody rb;
    SingularityTarget st;
    [SerializeField] float speed = 15.0f;
    [SerializeField] float jumpSpeed = 8.0f;
    [SerializeField] float angularSpeed = 3.0f;
    [SerializeField] float m_fCurrentRelativeSlopeAngle = 0f;


    [SerializeField] float cameraRotation = 0.0f;
    
    // Use this for initialization
    void Start () 
    {
        m_rGM = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        st = GetComponent<SingularityTarget>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {

        if (IsSlopeWalkable())
        {
            float velocityY = rb.velocity.y;
            Vector3 velocity = transform.forward * Input.GetAxisRaw("Vertical");
            velocity += transform.right * Input.GetAxisRaw("Horizontal");
            velocity.Normalize();
            velocity *= speed;

            velocity.y = velocityY;

            if (Input.GetButton("Jump") /*&& isGrounded()*/)
            {
                velocity.y = jumpSpeed;
            }

            rb.velocity = velocity;

            if (m_rGM != null && !m_rGM.IsDemoComplete)
                rb.angularVelocity = Vector3.up * Input.GetAxis("Mouse X") * angularSpeed;
        }


        if (m_rGM != null && !m_rGM.IsDemoComplete)
        {
            cameraRotation += Input.GetAxis("Mouse Y") * angularSpeed;
            cameraRotation = Mathf.Clamp(cameraRotation, -80.0f, 80.0f);
            Camera.main.transform.localEulerAngles = Vector3.left * cameraRotation;
        }
    }

    bool isGrounded()
    {

       

        Collider[] colliders = Physics.OverlapSphere(transform.position - Vector3.up, 0.3f);
        foreach(Collider c in colliders)
        {
            if(c.CompareTag("Ground"))
            {
                return true;
            }
        }

        return false;
    }

    bool IsSlopeWalkable()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up, 5f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Ground"))
            {
                //Compare the angle between the ground normal and the current direction
                //of the gravity to determine if the player can walk on this slope

                m_fCurrentRelativeSlopeAngle = Vector3.Angle(-hits[i].normal, st.m_vCurrentGravityDir);

                Debug.Log("Angle = " + m_fCurrentRelativeSlopeAngle.ToString());

                if (m_fCurrentRelativeSlopeAngle > 40f)
                {
                    return false;
                }


                return true;
            }
        }

        return false;

    }
}
