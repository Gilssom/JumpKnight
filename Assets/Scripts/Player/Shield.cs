using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Player m_Player;
    private BoxCollider2D m_Collider;
    public bool m_isReady = false;

    void Awake()
    {
        m_Player = GameManager.Instance.m_Player;
        m_Collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        transform.position = new Vector3(0, m_Player.transform.position.y + 1.5f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            m_isReady = true;

            CameraShake.Instance.CameraShaking(3f, 0.2f);

            SoundManager.Instance.Play("Shield");

            GameObject CurshEffect = ObjectPoolManager.Instance.m_ObjectPoolList[0].Dequeue();
            CurshEffect.transform.position = m_Collider.bounds.center + 
                new Vector3(0, m_Collider.bounds.extents.y, 0);
            CurshEffect.SetActive(true);
            ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.75f, 0, CurshEffect));
        }
    }
}
