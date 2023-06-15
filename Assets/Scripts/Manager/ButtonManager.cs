using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : UI_Scene
{
    enum Buttons
    {
        AttackButton,
        JumpButton,
        ShieldButton,
    }

    enum Images
    {
        CoolTimeImage,
    }

    private Player m_Player;

    public Button m_ShieldButton;
    public Image m_CoolTimeImage;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        m_ShieldButton = GetButton((int)Buttons.ShieldButton);
        m_CoolTimeImage = GetImage((int)Images.CoolTimeImage);
        m_Player = GameManager.Instance.m_Player;
    }

    public void AttackButtonClick()
    {
        m_Player.m_AttackInput = true;
    }

    public void ShieldButtonDown()
    {
        if(!m_Player.m_UsedShield)
            m_Player.m_ShieldInput = true;
    }

    public void ShieldButtonUp()
    {
        m_Player.m_ShieldInput = false;
        m_Player.ShieldOff();
    }

    public void JumpButtonDown()
    {
        m_Player.m_JumpInput = true;
    }

    public void JumpButtonUp()
    {
        m_Player.m_JumpInput = false;
    }
}
