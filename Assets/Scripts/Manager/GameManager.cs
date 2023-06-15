using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletomManager<GameManager>
{
    public Player m_Player;

    public GameObject m_BaseCamera;
    public GameObject m_JumpEffectCamera;

    public UI_PowerGage m_PowerGage { get; private set; }
    public UI_Player_GUI m_PlayerGUI { get; private set; }
    public ButtonManager m_ButtonManager { get; private set; }
    public SkillKeyMap m_SkillKey { get; private set; }

    public float m_BlockDrag;
    public int m_Score;
    public int m_CurStage;

    private void Awake()
    {
        m_PowerGage = UIManager.Instance.ShowSceneUI<UI_PowerGage>();
        m_PlayerGUI = UIManager.Instance.ShowSceneUI<UI_Player_GUI>();
        m_SkillKey = UIManager.Instance.ShowSceneUI<SkillKeyMap>();
        m_ButtonManager = UIManager.Instance.ShowSceneUI<ButtonManager>();

        m_BlockDrag = 0.75f;
        m_CurStage = 1;

        SkillManager.Instance.Setting();

        SoundManager.Instance.Play("Bgm", Defines.Sound.Bgm);
    }

    private void FixedUpdate()
    {
        m_PlayerGUI.SetScore(m_Score);
    }
}
