using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  각 속성을 가진 객체가 사용할 Animation Evnet 도 각자 다르기 때문에
 *  BaseAnimEvnet 를 부모로 갖는 자식 클래스를 설계함.
 *  공통적인 부분은 모두 부모 클래스에서 지정해줌
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
