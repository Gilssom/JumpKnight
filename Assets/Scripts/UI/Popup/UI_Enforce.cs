using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Enforce : UI_Popup
{
    enum Buttons
    {
        ADEnforceButton,
        ASEnforceButton
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        Time.timeScale = 0.1f;
    }

    public void ADEnforce()
    {
        Time.timeScale = 1;
        BaseStat.m_PlayerStat.ADEnforce();
        ClosePopupUI();
    }

    public void ASEnforce()
    {
        Time.timeScale = 1;
        GameManager.Instance.m_Player.ASEnforce();
        ClosePopupUI();
    }
}
