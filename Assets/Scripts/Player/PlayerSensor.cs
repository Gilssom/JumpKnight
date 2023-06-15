using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    [SerializeField]
    private Transform m_Sensor;

    [SerializeField]
    private LayerMask m_GroundLayerMask;

    [SerializeField]
    private float m_GroundCheckDis = 0.1f;

    public bool IsGrounded;

    void Update()
    {
        IsGrounded = CheckGrounded();
    }

    public bool CheckGrounded()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(m_Sensor.position, m_Sensor.forward, m_GroundCheckDis, m_GroundLayerMask);

        if(hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
