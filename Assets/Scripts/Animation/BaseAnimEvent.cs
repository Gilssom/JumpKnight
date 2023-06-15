using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimEvent : MonoBehaviour
{
    [Header("���� ��Ʈ �ڽ�")]
    [SerializeField, Tooltip("��Ʈ �ڽ� �ݶ��̴�")]
    protected BoxCollider2D[] m_AttackArea;
    [SerializeField, Tooltip("��Ʈ �ڽ��� �ٽ� ���� �ð�")]
    protected float m_HitCheckTime;

    protected WaitForSeconds m_CheckTime;
    protected WaitForSeconds m_BladeCheckTime;

    private void Start()
    {
        Init();
    }

    protected abstract void Init();
    protected abstract IEnumerator AttackArea(int number);
    protected abstract void NormalAttack(int number);
}
