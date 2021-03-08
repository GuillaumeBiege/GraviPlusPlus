using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SingularityRenderer : MonoBehaviour
{
    public enum TypeSingularity
    {
        PUNCTUAL,
        AXIAL,
        PLANAR
    }

    SingularityComponent m_rSingulatity = default;
    MeshFilter m_rMesh = default;

    [SerializeField] TypeSingularity m_eTypeSingularity = TypeSingularity.PUNCTUAL;
    Vector3 m_vOldScale = Vector3.zero;

    private void Awake()
    {
        m_rSingulatity = GetComponent<SingularityComponent>();
        m_rMesh = GetComponentInChildren<MeshFilter>();

    }

    // Update is called once per frame
    void Update()
    {
        if (m_rSingulatity && m_rMesh)
        {
            switch (m_eTypeSingularity)
            {
                case TypeSingularity.PUNCTUAL:
                    PunctualMeshDisplay();
                    break;
                case TypeSingularity.AXIAL:
                    AxialMeshDisplay();
                    break;
                case TypeSingularity.PLANAR:
                    PlanarMeshDisplay();
                    break;
                default:
                    break;
            }
        }
    }
    

    void PunctualMeshDisplay()
    {
        //Removing previous scale modif
        m_rMesh.transform.localScale -= m_vOldScale;

        //Generating the new size
        Vector3 meshScale = new Vector3(m_rSingulatity.m_fRangeMax, m_rSingulatity.m_fRangeMax, m_rSingulatity.m_fRangeMax);

        //Converting range RADIUS into DIAMETER
        meshScale *= 2f;

        //Applying new scale
        m_rMesh.transform.localScale += meshScale;

        //Saving this scale as the old one to be deleted before next scale modif
        m_vOldScale = meshScale;

    }

    void AxialMeshDisplay()
    {
        SingularityAxialComponent axial = m_rSingulatity as SingularityAxialComponent;
        if (axial)
        {
            //Getting singularity positions
            Vector3 start = axial.transform.position;
            Vector3 end = axial.m_rSourceB.transform.position;

            Vector3 direction = end - start;
            Vector3 middlePos = (direction / 2f) + start;

            m_rMesh.transform.position = middlePos;
            m_rMesh.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            //Removing previous scale modif
            m_rMesh.transform.localScale -= m_vOldScale;

            //Generating the new size
            Vector3 meshScale = new Vector3(m_rSingulatity.m_fRangeMax, direction.magnitude / 2f, m_rSingulatity.m_fRangeMax);

            //Applying new scale
            m_rMesh.transform.localScale += meshScale;

            //Saving this scale as the old one to be deleted before next scale modif
            m_vOldScale = meshScale;
        }
        else
        {
            Debug.LogError("Singularity Renderer -> Cast to SingularityAxialComponent failed !");
        }

    }

    void PlanarMeshDisplay()
    {
        SingularityPlanarComponent planar = m_rSingulatity as SingularityPlanarComponent;
        if (planar)
        {
            Vector3[] vertices = m_rMesh.mesh.vertices;

            Vector3 localPosSourceD = planar.m_rSourceB.transform.localPosition + planar.m_rSourceC.transform.localPosition;

            //Bottom
                //Source A
            vertices[7] = Vector3.zero;
            vertices[14] = Vector3.zero;
            vertices[18] = Vector3.zero;

                //Source B
            vertices[6] = planar.m_rSourceB.transform.localPosition;
            vertices[12] = planar.m_rSourceB.transform.localPosition;
            vertices[20] = planar.m_rSourceB.transform.localPosition;

                //Source D
            vertices[0] = localPosSourceD;
            vertices[13] = localPosSourceD;
            vertices[21] = localPosSourceD;

                //Source C
            vertices[1] = planar.m_rSourceC.transform.localPosition;
            vertices[15] = planar.m_rSourceC.transform.localPosition;
            vertices[19] = planar.m_rSourceC.transform.localPosition;

            //Top
            Vector3 rangeVector = planar.m_vPlanarNormal * planar.m_fRangeMax;

                //Source A
            vertices[5] = rangeVector;
            vertices[11] = rangeVector;
            vertices[17] = rangeVector;

                //Source B
            vertices[4] = planar.m_rSourceB.transform.localPosition + rangeVector;
            vertices[10] = planar.m_rSourceB.transform.localPosition + rangeVector;
            vertices[23] = planar.m_rSourceB.transform.localPosition + rangeVector;

                //Source D
            vertices[2] = localPosSourceD + rangeVector;
            vertices[8] = localPosSourceD + rangeVector;
            vertices[22] = localPosSourceD + rangeVector;

                //Source C
            vertices[3] = planar.m_rSourceC.transform.localPosition + rangeVector;
            vertices[9] = planar.m_rSourceC.transform.localPosition + rangeVector;
            vertices[16] = planar.m_rSourceC.transform.localPosition + rangeVector;

            //Applying the changes
            m_rMesh.mesh.vertices = vertices;
            m_rMesh.mesh.RecalculateBounds();
            m_rMesh.mesh.RecalculateNormals();
        }
    }
}
