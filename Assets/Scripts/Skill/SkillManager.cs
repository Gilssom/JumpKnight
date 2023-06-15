using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillManager : SingletomManager<SkillManager>
{
    public PowerJump m_PowerJump;
    public DevilCrying m_DevilCrying;
    public FigthTeams m_FightTeams;
    public TimeWarp m_TimeWarp;

    public List<Skill> AllSkills = new List<Skill>();
    public List<NMActiveSkill> NMActiveSkills = new List<NMActiveSkill>();

    public void Setting()
    {
        AllSkills.Add(m_PowerJump);
        AllSkills.Add(m_DevilCrying);
        AllSkills.Add(m_FightTeams);
        AllSkills.Add(m_TimeWarp);

        foreach (Skill sk in AllSkills)
        {
            if (sk is NMActiveSkill)
            {
                NMActiveSkill thisSkill = sk as NMActiveSkill;
                thisSkill.m_WhoPlayer = Characters.NightMare;
                NMActiveSkills.Add(thisSkill);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            ChangeSkillKey(i, NMActiveSkills[i]);
        }
    }

    void ChangeSkillKey(int where, NMActiveSkill skill)
    {
        SkillKeyMap.Instance.m_ButtonList[where].GetComponent<Image>().sprite = skill.m_SkillIcon;
        SkillKeyMap.Instance.SetKeyFunc(where, skill.SkillExecute, skill.m_CoolTime, skill.m_UseCost);
    }
}
