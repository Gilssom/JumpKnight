using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] 
    private Player m_Player;
    [SerializeField]
    private float m_CameraSpeed;

    Transform m_PlayerTransform;

    Vector3 m_CameraPos = new Vector3(0, 10, -10);

    private void Awake()
    {
        m_PlayerTransform = m_Player.transform;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, m_PlayerTransform.position + m_CameraPos,
            Time.deltaTime * m_CameraSpeed);
    }
}
