using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClasses : MonoBehaviour
{
    
}

public interface Skill
{

}

[System.Serializable]
public abstract class SkillBase : Skill
{
    public string m_Name;
    public Sprite m_SkillIcon;
    public Characters m_WhoPlayer;
    public string m_Description;

    public virtual void SkillExecute() { }
}

public enum Characters
{
    NightMare, Demonic,
}

[System.Serializable]
public abstract class ActiveSkill : SkillBase
{
    public int m_UseCost;
    public float m_CoolTime;
}

[System.Serializable]
public abstract class PassiveSkill : SkillBase
{
    public int m_UsedLevel;
    public string m_FlavorText;
}

[System.Serializable]
public abstract class NMActiveSkill : ActiveSkill
{
    public NMSkillCategory m_Category;
    public NMSkillResources m_RequireResource;
}

// 해당 스킬 소모 자원
public enum NMSkillResources
{
    Health, SkillStone
}

// 스킬 카테고리 : 공격형, 방어형, 유틸형
public enum NMSkillCategory
{
    Attacker, Defenser, Utilliter
}

[System.Serializable]
public abstract class NMPassiveSkill : PassiveSkill
{
    public NMSkillCategory m_Category;
}

// 반격 : 공격형 파워 점프
[System.Serializable]
public class PowerJump : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.PowerJump(m_UseCost);
    }
}

// 데빌 크라이 : 공격형 광역 다단 히트 공격
[System.Serializable]
public class DevilCrying : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.DevilCrying(m_UseCost);
    }
}

// 전투기동대 : 공격형 아군 동료 소환
[System.Serializable]
public class FigthTeams : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.FigthTeams(m_UseCost);
    }
}

// 시간 왜곡기 : 방어형 중력값 감소
[System.Serializable]
public class TimeWarp : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.TimeWarp();
        SkillKeyMap.Instance.ResetCoolTime(m_CoolTime);
    }
}
