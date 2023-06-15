using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : UI_Base
{
    public enum SkillType
    {
        Gage,
        CoolTime,
    }

    public enum GageType
    {
        None,
        AttackCombo,
        JumpCombo,
        SkillGage,
    }

    enum Images
    {
        ButtonCoolTime,
    }

    public SkillType m_HowSkill;
    public GageType m_GageSkillType;
    private bool m_isCoolTime;
    private Button m_Button;

    [SerializeField]
    private int m_UseNeedCost;
    [SerializeField]
    private float m_CurCoolTime;

    public bool isCoolTime { get { return m_isCoolTime; } set { m_isCoolTime = value; } }

    public override void Init()
    {
        Bind<Image>(typeof(Images));

        m_Button = GetComponent<Button>();
    }

    void Update()
    {
        if(m_HowSkill == SkillType.Gage)
        {
            switch (m_GageSkillType)
            {
                case GageType.None:
                    break;
                case GageType.AttackCombo:
                    if(m_UseNeedCost > GameManager.Instance.m_Player.m_AttackComboSkill)
                        m_Button.interactable = false;
                    else
                        m_Button.interactable = true;
                    break;
                case GageType.JumpCombo:
                    if (m_UseNeedCost > GameManager.Instance.m_Player.m_JumpComboSkill)
                        m_Button.interactable = false;
                    else
                        m_Button.interactable = true;
                    break;
                case GageType.SkillGage:
                    if (m_UseNeedCost > GameManager.Instance.m_Player.m_SkillGage)
                        m_Button.interactable = false;
                    else
                        m_Button.interactable = true;
                    break;
            }
        }
    }

    public void GameSetting(SkillType type, float coolTime = 0, int useCost = 0)
    {
        if(type == SkillType.Gage)
            m_UseNeedCost = useCost;
    }

    public void ResetCoolTime(float coolTime)
    {
        m_CurCoolTime = coolTime;
        m_isCoolTime = true;

        StartCoroutine(TestCoolTime(m_CurCoolTime));
    }

    IEnumerator TestCoolTime(float coolTime)
    {
        while (m_isCoolTime)
        {
            m_CurCoolTime -= Time.deltaTime;

            GetImage((int)Images.ButtonCoolTime).fillAmount = m_CurCoolTime / coolTime;

            if (m_CurCoolTime <= 0)
            {
                m_isCoolTime = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
