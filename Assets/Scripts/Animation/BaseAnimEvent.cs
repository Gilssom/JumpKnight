using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAnimEvent : MonoBehaviour
{
    [Header("공격 히트 박스")]
    [SerializeField, Tooltip("히트 박스 콜라이더")]
    protected BoxCollider2D[] m_AttackArea;
    [SerializeField, Tooltip("히트 박스를 다시 끄는 시간")]
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
