using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    GameManager m_rGM = default;

    Rigidbody rb;
    SingularityTarget st;
    [SerializeField] float m_fSpeed = 15.0f;
    [SerializeField] float m_fJumpSpeed = 8.0f;
    [SerializeField] float m_fAngularSpeed = 3.0f;
    [SerializeField] float m_fCurrentRelativeSlopeAngle = 0f;


    [SerializeField] float m_fCameraRotation = 0.0f;


    [SerializeField] bool m_bIsThrowEnable = false;
    [SerializeField] float m_fThrowForce = 10f;
    [SerializeField] GameObject m_rBallSpawn = default;
    [SerializeField] GameObject m_rBallPrefab = default;

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
            velocity *= m_fSpeed;

            velocity.y = velocityY;

            if (Input.GetButton("Jump") /*&& isGrounded()*/)
            {
                velocity.y = m_fJumpSpeed;
            }

            rb.velocity = velocity;

            if (m_rGM != null && !m_rGM.IsDemoComplete)
                rb.angularVelocity = Vector3.up * Input.GetAxis("Mouse X") * m_fAngularSpeed;
        }


        if (m_rGM != null && !m_rGM.IsDemoComplete)
        {
            m_fCameraRotation += Input.GetAxis("Mouse Y") * m_fAngularSpeed;
            m_fCameraRotation = Mathf.Clamp(m_fCameraRotation, -80.0f, 80.0f);
            Camera.main.transform.localEulerAngles = Vector3.left * m_fCameraRotation;
        }

        if (m_bIsThrowEnable && Input.GetButtonDown("Fire1"))
        {
            ThrowABall();
        }
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

    void ThrowABall()
    {
        if (m_rBallPrefab != null)
        {
            GameObject go = Instantiate<GameObject>(m_rBallPrefab);
            go.transform.position = m_rBallSpawn.transform.position;
            go.transform.rotation = m_rBallSpawn.transform.rotation;

            Vector3 force = m_rBallSpawn.transform.forward * m_fThrowForce;

            go.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
