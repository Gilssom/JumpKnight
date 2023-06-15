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

// �ش� ��ų �Ҹ� �ڿ�
public enum NMSkillResources
{
    Health, SkillStone
}

// ��ų ī�װ� : ������, �����, ��ƿ��
public enum NMSkillCategory
{
    Attacker, Defenser, Utilliter
}

[System.Serializable]
public abstract class NMPassiveSkill : PassiveSkill
{
    public NMSkillCategory m_Category;
}

// �ݰ� : ������ �Ŀ� ����
[System.Serializable]
public class PowerJump : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.PowerJump(m_UseCost);
    }
}

// ���� ũ���� : ������ ���� �ٴ� ��Ʈ ����
[System.Serializable]
public class DevilCrying : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.DevilCrying(m_UseCost);
    }
}

// �����⵿�� : ������ �Ʊ� ���� ��ȯ
[System.Serializable]
public class FigthTeams : NMActiveSkill
{
    public override void SkillExecute()
    {
        base.SkillExecute();
        GameManager.Instance.m_Player.FigthTeams(m_UseCost);
    }
}

// �ð� �ְ�� : ����� �߷°� ����
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
