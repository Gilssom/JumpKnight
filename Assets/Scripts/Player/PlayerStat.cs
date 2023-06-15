using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  ���� �÷��̾��� �������� ���� Ŭ������ ���谡 �޶����ų�,
 *  ������ Ŭ������ ����� ���ɼ��� �ֱ� ������
 *  BaseStat �� �θ�� �޴� �ڽ� Ŭ������ ����
 */
public class PlayerStat : BaseStat
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region #ĳ���� �ΰ� ����
    [Header("ĳ���� �ΰ� ����")]
    [SerializeField]
    int m_CriticalChance;
    [SerializeField]
    float m_CriticalDamage;
    [SerializeField]
    int m_Exp;
    [SerializeField]
    int m_MaxExp;
    [SerializeField]
    int m_Gold;

    public int CriticalChance { get { return m_CriticalChance; } set { m_CriticalChance = value; } }
    public float CriticalDamage { get { return m_CriticalDamage; } set { m_CriticalDamage = value; } }
    public int Exp
    {
        get { return m_Exp; }
        set
        {
            m_Exp = value;

            int level = Level;

            if (m_Exp < m_MaxExp)
                return;
            else
                level++;

            if (level != Level)
            {
                SoundManager.Instance.Play("Level Up");
                Level = level;
                m_Exp = 0;
                m_MaxExp += 25;
                UIManager.Instance.ShowPopupUI<UI_Enforce>();
                Heal(MaxHp - Hp);
            }
        }
    }
    public int MaxExp { get { return m_MaxExp; } set { m_MaxExp = value; } }
    public int Gold { get { return m_Gold; } set { m_Gold = value; } }
    #endregion

    #region �÷��̾� �ɷ�ġ ��ȭ ��ġ
    [Header("ĳ���� �ɷ�ġ ��ȭ ��ġ")]
    [SerializeField, Tooltip("�ǰ� �� ���� �Ǻ�")]
    public bool m_Hit_Gurad;
    [SerializeField, Tooltip("�ǰ� �� ���� �ð�")]
    public float m_Hit_GuardTime;
    [SerializeField, Tooltip("�ִ� ü�� �߰� �ɷ�ġ")]
    public int m_Plus_MaxHp;
    [SerializeField, Tooltip("ũƼ���� Ȯ�� �߰� �ɷ�ġ")]
    public int m_Plus_CriChance;
    [SerializeField, Tooltip("ũ��Ƽ�� ������ �߰� �ɷ�ġ")]
    public float m_Plus_CriDamage;
    [SerializeField, Tooltip("���ݷ� �߰� �ɷ�ġ")]
    public int m_Plus_Attack;
    #endregion

    private WaitForSeconds m_GuardTime;

    protected override void Init()
    {
        m_GuardTime = new WaitForSeconds(m_Hit_GuardTime);

        m_PlayerStat = this;
        m_Level = 1;
        m_Hp += 3;
        m_MaxHp += 3;
        m_Attack += 15;
        //SetStat(m_Level);
    }

    // Json ���� �������� �÷��̾� Stat ��� �ý��� ����
    // 6�� 15�� ���� ���� X ( ���� ����� ���� �ϼ��� ���� ���� ���� )
    protected override void SetStat(int level)
    {
        //Dictionary<int, Data.Stat> dict = m_DataManager.StatDict;
        //Data.Stat stat = dict[level];
        //Data.Stat expStat = dict[level + 1];

        //m_Hp += stat.maxHp;
        //m_MaxHp += stat.maxHp;
        //m_Attack += stat.attack;
        //m_CriticalChance += stat.criticalchance;
        //m_CriticalDamage += stat.criticaldamage;
        //m_Defense += stat.defense;

        //m_Exp -= stat.totalExp;
        //m_MaxExp = expStat.totalExp;
    }

    #region #Player Health System
    public void Heal(int health)
    {
        this.Hp += health;
        ClampHealth();
    }

    public void TakeDamage(int dmg)
    {
        if(!m_Hit_Gurad)
        {
            StartCoroutine(HitCheck());
            Hp -= dmg;
            ClampHealth();  
        }
    }

    void ClampHealth()
    {
        Hp = Mathf.Clamp(Hp, 0, MaxHp);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
    #endregion

    IEnumerator HitCheck()
    {
        m_Hit_Gurad = true;
        GameManager.Instance.m_Player.m_ComboScore = 0;
        GameManager.Instance.m_PlayerGUI.ComboReset = true;

        yield return m_GuardTime;

        m_Hit_Gurad = false;
        GameManager.Instance.m_PlayerGUI.ComboReset = false;
    }

    public void ADEnforce()
    {
        m_Attack += 5;
    }
}