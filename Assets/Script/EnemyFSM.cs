using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    EnemyState m_State;
    CharacterController cc;
    Vector3 originPos;
    Transform player;
    Animator animator;
    Quaternion originRot;
    NavMeshAgent agent;

    public float attackDistance = 2f;
    public float findDistance = 8f;
    public float moveSpeed = 5f;
    float attackDelay = 2f;
    float currentTime = 0;
    public int attackPower = 3;
    public float moveDistance = 20f;
    public int hp = 20;
    int maxhp = 20;
    public Slider hpSlider;

    private GameObject playerGo;

    Text gameText;
    public GameObject gameLabel;    

    void Start()
    {
        m_State = EnemyState.Idle;        
        player = GameObject.Find("Player").transform;        
        cc = GetComponent<CharacterController>();        
        originPos = transform.position;
        originRot = transform.rotation;
        animator = transform.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerGo = GameObject.FindWithTag("Player");
        //gameText = gameLabel.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:                
                break;
        }

        hpSlider.value = (float)hp / (float)maxhp;        
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
            animator.SetTrigger("IdleToMove");
        }
        
    }

    void Move()
    {
        
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {         
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //Vector3 dir = (player.position - transform.position).normalized;           
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;
            
            agent.stoppingDistance = attackDistance;
            agent.destination = player.position;
        }        
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");            
            currentTime = attackDelay;

            animator.SetTrigger("MoveToAttackDelay");

            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    void Attack()
    {
        
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {            
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;

                animator.SetTrigger("StartAttack");
            }
        }        
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;

            animator.SetTrigger("AttackToMove");
        }
    }

    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {        
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //Vector3 dir = (originPos - transform.position).normalized;
            //cc.Move(dir * moveSpeed * Time.deltaTime);

            //transform.forward = dir;

            agent.destination = originPos;
            agent.stoppingDistance = 0;
        }       
        else
        {
            agent.isStopped = true;
            agent.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;

            hp = maxhp;

            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");

            animator.SetTrigger("MoveToIdle");            
        }
    }

    public void HitEnemy(int hitPower)
    {        
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }
        
        hp -= hitPower;

        agent.isStopped = true;
        agent.ResetPath();
        
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");

            animator.SetTrigger("Damaged");
            Damaged();
        }        
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");

            animator.SetTrigger("Die");
            Die();
        }
    }

    void Damaged()
    {        
        StartCoroutine(DamageProcess());
    }
    
    IEnumerator DamageProcess()
    {        
        yield return new WaitForSeconds(1.0f);
        
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    void Die()
    {        
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {        
        cc.enabled = false;
        
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);

        GameObject smObject = GameObject.Find("ScoreManager");
        ScoreManager sm = smObject.GetComponent<ScoreManager>();
        sm.currentScore++;
        sm.currentScoreUI.text = "현재점수 : " + sm.currentScore;
    }
}
