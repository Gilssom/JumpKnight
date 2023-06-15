using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStat))]
public class Player : MonoBehaviour
{
    public float m_JumpPower = 7.5f;
    public int m_ComboScore = 0;  
    public int m_AttackComboSkill = 0;
    public int m_JumpComboSkill = 0;
    public float m_SkillGage = 0;

    [SerializeField]
    private BoxCollider2D m_DevilSkillArea;
    [SerializeField]
    private GameObject m_DevilSkill, m_JumpSkill;

    public bool m_UsedShield = false;
    public float m_AttackSpeed = 0f;

    private Animator m_Animator;
    private Rigidbody2D m_Rigid2D;
    private PlayerSensor m_GroundCheck;
    private bool m_Grounded = false;
    private int m_currentAttack = 0;
    private float m_AttackTimeSpeed = 0.0f;
    private float m_IdleDelay = 0.0f;
    private bool m_Death = false;
    private bool m_isGage = false;
    private bool m_noBlood = false;
    private int m_BlockLayer, m_PlayerLayer;

    [SerializeField]
    private float m_ShieldCoolTime;

    public ParticleSystem m_GageEffect;
    public Shield m_Shield;

    #region #모바일 버튼 관련 함수
    public bool m_AttackInput = false;
    public bool m_ShieldInput = false;
    public bool m_JumpInput = false;
    #endregion

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigid2D = GetComponent<Rigidbody2D>();
        m_GroundCheck = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        m_PlayerLayer = LayerMask.NameToLayer("Player");
        m_BlockLayer = LayerMask.NameToLayer("Block");

        m_ShieldCoolTime = 3;
        m_AttackSpeed = 1;
    }

    bool GageEffect(bool isGage) => m_isGage ? true : false;

    #region #Player State
    /*  플레이어는 시간이 지남에 따라 스킬 게이지를 얻는다. ( 시간 왜곡자 사용 )
     *  플레이어가 땅에 닿아 있을 경우 블럭과 충돌하지 않는다. ( 블럭이 땅에 충돌 시 플레이어 피격 )
     *  점프 게이지의 최대 수치는 현재 80.
     */
    void Update()
    {
        m_AttackTimeSpeed += Time.deltaTime * m_AttackSpeed;

        if(m_SkillGage <= 100)
            m_SkillGage += Time.deltaTime * 1.2f;

        if (m_Grounded && !m_Shield.gameObject.activeSelf)
            Physics2D.IgnoreLayerCollision(m_PlayerLayer, m_BlockLayer, true);
        else
            Physics2D.IgnoreLayerCollision(m_PlayerLayer, m_BlockLayer, false);

        if (GageEffect(m_isGage) && !m_GageEffect.isPlaying)
            m_GageEffect.Play();
        else
        {
            if (m_JumpPower > 5 && !GageEffect(m_isGage))
                m_JumpPower -= Time.deltaTime * 50;

            m_GageEffect.Stop();
        }

        if (!m_Grounded && m_GroundCheck.IsGrounded)
        {
            m_Grounded = true;
            m_Animator.SetBool("Grounded", m_Grounded);
        }

        if (m_Grounded && !m_GroundCheck.IsGrounded)
        {
            m_Grounded = false;
            m_Animator.SetBool("Grounded", m_Grounded);
        }

        m_Animator.SetFloat("AirSpeedY", m_Rigid2D.velocity.y);
        m_Animator.SetFloat("AttackSpeed", m_AttackSpeed);

        if (BaseStat.m_PlayerStat.Hp <= 0)
        {
            GameManager.Instance.m_ButtonManager.gameObject.SetActive(false);
            m_Animator.SetBool("noBlood", m_noBlood);
            m_Animator.Play("Death");
            m_Death = true;
            Invoke("GameSet", 3f);
        }

        else if (m_AttackInput && m_AttackTimeSpeed > 0.25f)
        {
            m_currentAttack++;

            SoundManager.Instance.Play("Sword");

            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_AttackTimeSpeed > 1.0f)
                m_currentAttack = 1;

            m_Animator.SetTrigger("Attack" + m_currentAttack);

            m_AttackTimeSpeed = 0.0f;
            m_AttackInput = false;
        }

        else if (m_JumpInput && m_Grounded)
        {
            m_isGage = true;
            float Gage = m_JumpPower < 80 ? 30 : 0;

            m_JumpPower += Time.deltaTime * Gage;
        }

        else if (!m_JumpInput && m_Grounded && m_JumpPower > 5)
        {
            m_Animator.SetTrigger("Jump");
            SoundManager.Instance.Play("Jump");

            if (m_JumpComboSkill < 30)
                m_JumpComboSkill++;

            m_Grounded = false;
            m_isGage = false;
            m_Animator.SetBool("Grounded", m_Grounded);
            m_Rigid2D.velocity = new Vector2(m_Rigid2D.velocity.x, m_JumpPower);
        }

        else
        {
            m_IdleDelay -= Time.deltaTime;
            if (m_IdleDelay < 0)
                m_Animator.SetInteger("AnimState", 0);
        }

        if(!m_Death)
            Shield();
    }
    #endregion

    #region #Player Shield
    void Shield()
    {
        if (m_ShieldInput && !m_UsedShield && !m_Shield.m_isReady)
        {
            m_Shield.gameObject.SetActive(true);
            m_Shield.transform.position = new Vector3(0, transform.position.y + 1.5f, 0);
            m_UsedShield = true;
            m_Animator.SetTrigger("Block");
            m_Animator.SetBool("IdleBlock", true);
        }
    }

    public void ShieldOff()
    {
        if (m_ShieldCoolTime >= 3)
        {
            StartCoroutine(ShieldCoolTime());
            m_Shield.gameObject.SetActive(false);
            m_Animator.SetBool("IdleBlock", false);
        }
    }

    IEnumerator ShieldCoolTime()
    {
        m_Shield.m_isReady = false;
        GameManager.Instance.m_ButtonManager.m_ShieldButton.interactable = false;

        while (m_UsedShield)
        {
            m_ShieldCoolTime -= Time.deltaTime;

            GameManager.Instance.m_ButtonManager.m_CoolTimeImage.fillAmount = m_ShieldCoolTime / 3;

            if (m_ShieldCoolTime <= 0)
            {
                m_UsedShield = false;
                m_ShieldCoolTime = 3;
                GameManager.Instance.m_ButtonManager.m_ShieldButton.interactable = true;
            }

            yield return new WaitForFixedUpdate();
        }
    }
    #endregion

    #region #Player Skill System
    public void PowerJump(int Cost)
    {
        m_JumpComboSkill -= Cost;

        GameManager.Instance.m_JumpEffectCamera.SetActive(true);

        StartCoroutine(PowerJump());
    }

    IEnumerator PowerJump()
    {
        yield return new WaitForSeconds(0.55f);

        SoundManager.Instance.Play("Power Jump");

        GameManager.Instance.m_JumpEffectCamera.SetActive(false);

        m_Animator.SetTrigger("Jump");

        m_Grounded = false;
        m_isGage = false;
        m_Animator.SetBool("Grounded", m_Grounded);
        m_Rigid2D.velocity = new Vector2(m_Rigid2D.velocity.x, 50);

        m_DevilSkillArea.gameObject.SetActive(true);
        m_JumpSkill.SetActive(true);

        yield return new WaitForSeconds(4);

        m_DevilSkillArea.gameObject.SetActive(false);
        m_JumpSkill.SetActive(false);
    }

    public void DevilCrying(int Cost)
    {
        m_AttackComboSkill -= Cost;
        StartCoroutine(DevilSkill());
    }

    IEnumerator DevilSkill()
    {
        m_DevilSkillArea.gameObject.SetActive(true);
        m_DevilSkill.SetActive(true);
        UIManager.Instance.ShowPopupUI<UI_DevilSkill>();

        yield return new WaitForSeconds(5);

        m_DevilSkillArea.gameObject.SetActive(false);
        m_DevilSkill.SetActive(false);
    }

    public void FigthTeams(int Cost)
    {
        m_SkillGage -= Cost;

        StartCoroutine(FightTeams());
    }

    IEnumerator FightTeams()
    {
        GameObject sub1 = ResourcesManager.Instance.Instantiate("SubCharacter/Sub_1");
        GameObject sub2 = ResourcesManager.Instance.Instantiate("SubCharacter/Sub_2");

        yield return new WaitForSeconds(12f);

        Destroy(sub1);
        Destroy(sub2);
    }

    public void TimeWarp()
    {
        StartCoroutine(TimeWarpEffect());
    }

    IEnumerator TimeWarpEffect()
    {
        SoundManager.Instance.Play("TimeWarp");

        GameManager.Instance.m_BlockDrag = 4f;
        UIManager.Instance.ShowPopupUI<UI_TimeWarpInfo>();

        yield return new WaitForSeconds(5);

        GameManager.Instance.m_BlockDrag = 0.75f;
    }
    #endregion

    public void AttackEffect(int number)
    {
        GameObject SlashEffect = ObjectPoolManager.Instance.m_ObjectPoolList[number].Dequeue();
        SlashEffect.transform.position = new Vector2(transform.position.x + 1.5f, transform.position.y + 3f);
        SlashEffect.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.3f, number, SlashEffect));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            m_Rigid2D.velocity = Vector2.zero;
        }
    }

    public void ASEnforce()
    {
        m_AttackSpeed += m_AttackSpeed * 0.1f;
    }

    void GameSet()
    {
        UIManager.Instance.ShowPopupUI<UI_Death>();
    }
}
