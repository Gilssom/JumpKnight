using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  �� �Ӽ��� ���� ��ü�� ����� Animation Evnet �� ���� �ٸ��� ������
 *  BaseAnimEvnet �� �θ�� ���� �ڽ� Ŭ������ ������.
 *  �������� �κ��� ��� �θ� Ŭ�������� ��������
 *  
 */ 
public class AnimationEvent : BaseAnimEvent
{
    public Player m_Player { get; private set; }

    protected override void Init()
    {
        m_CheckTime = new WaitForSeconds(m_HitCheckTime);
        m_Player = GetComponent<Player>();
    }

    protected override IEnumerator AttackArea(int number)
    {
        m_AttackArea[number].enabled = true;
        Debug.Log($"{m_AttackArea[number].name} + {number}");
        yield return m_CheckTime;
        m_AttackArea[number].enabled = false;
    }

    protected override void NormalAttack(int number)
    {
        m_Player.AttackEffect(number);
        GameObject CurshEffect = ObjectPoolManager.Instance.m_ObjectPoolList[1].Dequeue();
        CurshEffect.transform.position = new Vector2(0, m_Player.transform.position.y + 3f);
        CurshEffect.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.1f, 1, CurshEffect));
    }
}
