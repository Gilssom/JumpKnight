using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlayer : MonoBehaviour
{
    private Animator m_Animator;
    private Rigidbody2D m_Rigid2D;
    private PlayerSensor m_GroundCheck;
    private float m_AttackSpeed;
    private bool m_Grounded = false;
    private int m_currentAttack = 0;
    private float m_AttackTimeSpeed = 0.0f;

    private WaitForSeconds m_AttackDelay;
    private WaitForSeconds m_AttackWait;

    public int m_ID;

    [SerializeField]
    private BoxCollider2D m_AttackArea;

    [SerializeField]
    private float m_otherTime;

    void Start()
    {
        m_GroundCheck = transform.Find("GroundSensor").GetComponent<PlayerSensor>();

        m_Animator = GetComponent<Animator>();
        m_Rigid2D = GetComponent<Rigidbody2D>();

        m_AttackDelay = new WaitForSeconds(0.15f);
        m_AttackWait = new WaitForSeconds(1f);

        m_otherTime = 10f;

        StartCoroutine(SubAttack());

        m_AttackSpeed = GameManager.Instance.m_Player.m_AttackSpeed;
    }

    void Update()
    {
        m_AttackTimeSpeed += Time.deltaTime * m_AttackSpeed;
        m_otherTime -= Time.deltaTime;

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
    }

    IEnumerator SubAttack()
    {
        yield return m_AttackWait;

        int attackCnt = m_ID;
        m_AttackArea.enabled = true;

        while (m_otherTime > 0)
        {
            if (m_AttackTimeSpeed > 0.25f)
                Attack(attackCnt);

            if (attackCnt < m_ID + 2)
                attackCnt++;
            else
                attackCnt = m_ID;

            yield return m_AttackDelay;
        }

        m_AttackArea.enabled = false;
        Jump();
    }

    void Attack(int number)
    {
        m_currentAttack++;

        GameObject SubSlash = ObjectPoolManager.Instance.m_ObjectPoolList[number].Dequeue();
        SubSlash.SetActive(true);
        ObjectPoolManager.Instance.StartCoroutine(ObjectPoolManager.Instance.DestroyObj(0.3f, number, SubSlash));

        if (m_currentAttack > 3)
            m_currentAttack = 1;

        if (m_AttackTimeSpeed > 1.0f)
            m_currentAttack = 1;

        m_Animator.SetTrigger("Attack" + m_currentAttack);

        m_AttackTimeSpeed = 0.0f;
    }

    void Jump()
    {
        m_Animator.SetTrigger("Jump");

        m_Grounded = false;
        m_Animator.SetBool("Grounded", m_Grounded);
        m_Rigid2D.velocity = new Vector2(m_Rigid2D.velocity.x, 80);
    }
}