using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : SingletomManager<BaseStat>
{
    protected BaseStat() { }
    public static PlayerStat m_PlayerStat;

    #region #°ø¿ë ½ºÅÈ
    [Header("°ø¿ë ½ºÅÈ")]
    [SerializeField]
    protected int m_Level;
    [SerializeField]
    protected int m_Hp;
    [SerializeField]
    protected int m_MaxHp;
    [SerializeField]
    protected int m_Attack;

    public int Level { get { return m_Level; } set { m_Level = value; } }
    public int Hp { get { return m_Hp; } set { m_Hp = value; } }
    public int MaxHp { get { return m_MaxHp; } set { m_MaxHp = value; } }
    public int Attack { get { return m_Attack; } set { m_Attack = value; } }
    #endregion

    private void Awake()
    {
        Init();
    }

    protected virtual void Init() { }
    protected virtual void SetStat(int number) { }
}
