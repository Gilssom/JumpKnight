using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockController m_BlockController;
    private Rigidbody2D m_Rigid2D;
    private Material m_Material;

    [SerializeField]
    private int TestHp;
    [SerializeField]
    private float m_TestPower;
    [SerializeField]
    private BlockSensor m_BlockSensor; 

    private void Start()
    {
        m_Rigid2D = GetComponent<Rigidbody2D>();
        m_Material = GetComponent<SpriteRenderer>().material;
        m_BlockSensor = GetComponentInChildren<BlockSensor>();
        m_BlockController = GetComponentInParent<BlockController>();

        // Temp / Stage Setting
        int stage = GameManager.Instance.m_CurStage;
        TestHp = stage * (stage + 3);
    }

    private void FixedUpdate()
    {
        m_Rigid2D.drag = GameManager.Instance.m_BlockDrag;
    }

    
    #region #Trigger Event
    /* 블럭 < - > 방패 :: 위로 튕기기
     *        - > 공격::피격 이벤트
     *        - > 지면::플레이어 피격 이벤트
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            if (m_BlockSensor.CheckGrounded())
                m_BlockController.ShieldCrush(1.5f);
            else
                m_BlockController.ShieldCrush(1);
        }

        if (collision.gameObject.tag == "Attack")
            OnHitEvent(BaseStat.m_PlayerStat);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            BaseStat.m_PlayerStat.TakeDamage(1);
    }
    #endregion

    public void Shield(float power) { m_Rigid2D.velocity = new Vector2(0, 3f) * m_TestPower * power; }

    #region #Block Hit Event
    /* 1. 플레이어 크리티컬 확인 -> 데미지 추가 피해
     * 2. 플레이어 콤보 상승 및 스킬 게이지 상승
     * 3. Material Color , Damage Text 표시
     * 4. 파괴 시 일정 비율 만큼 경험치 증가 및 스코어, 골드 증가
    */
    void OnHitEvent(PlayerStat player)
    {
        if(TestHp > 0)
        {
            bool isCritical = false;
            int critical = Random.Range(0, 100);
            int damage = Random.Range(player.Attack - 5, player.Attack + 1);

            if (critical < player.CriticalChance)
            {
                damage = Mathf.RoundToInt(damage * (player.CriticalDamage / 100));
                isCritical = true;
            }

            GameManager.Instance.m_Player.m_ComboScore++;
            SoundManager.Instance.Play("Block Broken");

            if (GameManager.Instance.m_Player.m_AttackComboSkill < 15)
                GameManager.Instance.m_Player.m_AttackComboSkill++;

            DamageText(damage, isCritical);
            TestHp -= damage;
            StartCoroutine(OnDamageEvent());

            if (TestHp <= 0)
            {
                player.Exp += Mathf.Abs(TestHp / 4);
                GameManager.Instance.m_Score += (int)Random.Range(1000, 3000);
                player.Gold += (int)Random.Range(500, 2500);
                m_BlockController.Broken();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator OnDamageEvent()
    {
        m_Material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        m_Material.color = new Color(1, 1, 1);
    }

    void DamageText(int damage, bool critical)
    {
        UI_Damage damageObj = UIManager.Instance.MakeWorldSpaceUI<UI_Damage>();
        damageObj.m_Pos = this.transform;
        damageObj.m_Damage = damage;
        damageObj.m_Critical = critical;
    }
    #endregion
}
