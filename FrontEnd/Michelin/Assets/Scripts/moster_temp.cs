using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Monster_temp : MonoBehaviour
{
    private Animator monsterAnim;
    private MonsterSpawnManager spawnManager;
    private NavMeshAgent agent; // NavMeshAgent

    [SerializeField]
    private float minSpeed; // �ּ� �ӵ� (��� ��)

    [SerializeField]
    private float maxSpeed; // �ִ� �ӵ� (��� ��)

    [SerializeField]
    private float trackingSpeed; // ���� ���� �� �ӵ�

    [SerializeField]
    private float moveTime = 4f; // �̵� �ð�

    [SerializeField]
    private float restTime = 2f; // ���� �ð�

    [SerializeField]
    private float hp = 10f; // ü��

    [SerializeField]
    private SOItemDropTable dropTable; // ��� ���̺�

    [SerializeField]
    private GameObject damageText; // �������� ���� ���� ����� Text ������Ʈ
    [SerializeField]
    private Transform damagePos; // ������ Text ��ġ

    [SerializeField]
    private Transform attackHitBoxPos; // ���� ��Ʈ �ڽ� pivot��ġ
    [SerializeField]
    private Vector2 attackHitBoxSize; // ���� ��Ʈ �ڽ� ������

    private Vector3 moveDir; // �̵� ����
    private float moveSpeed; // �̵� �ӵ�
    private bool isMove; // �̵� ����
    public bool isTracking; // �÷��̾� ���� ����

    private float latestMoveStartTime; // �ֱ� �̵� ���� �ð�
    private float latestRestStartTime; // �ֱ� �޽� ���� �ð�

    public bool is_right;

    private void Awake()
    {
        monsterAnim = GetComponent<Animator>();
        spawnManager = GameObject.Find("MonsterSpawnManager").GetComponent<MonsterSpawnManager>();
    }

    private void Start()
    {
        isMove = false;
        isTracking = false;
        latestMoveStartTime = Time.time;
        latestRestStartTime = Time.time;

        // NavMeshAgent ����
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = trackingSpeed;

        is_right = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (is_right)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180f, 0);
        }


        // �ǰ�, ����, ��� ���°ų� ���� ���¸� ������Ʈ �Լ��� ����
        if (DoHurtAnim() || DoAttackAnim() || DoDeathAnim() || isTracking)
        {
            return;
        }

        

        // �̵� ���� ��
        if (isMove)
        {
            // �̵��� ������ �޽����� ����
            if (Time.time - latestMoveStartTime >= moveTime)
            {
                StartRest();
            }
            else
            {
                transform.position += moveSpeed * Time.deltaTime * moveDir;
            }
        }
        // �޽����� ��
        else
        {
            // �޽��� ������ �̵� ����
            if (Time.time - latestRestStartTime >= restTime)
            {
                StartMove();
            }
            else
            {
                return;
            }
        }
    }

    // ���� ���� �ִϸ��̼��� ���� ���ΰ�
    private bool DoAttackAnim()
    {
        return monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack L")
            || monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack R");
    }

    // ���� �ǰ� �ִϸ��̼��� ���� ���ΰ�
    private bool DoHurtAnim()
    {
        return monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Hurt L")
            || monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Hurt R");
    }

    // ���� ��� �ִϸ��̼��� ���� ���ΰ�
    private bool DoDeathAnim()
    {
        return monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Death L")
            || monsterAnim.GetCurrentAnimatorStateInfo(0).IsName("Death R");
    }

    // �̵� ����
    void StartMove()
    {
        // ���� �̵����� �� �ӵ� ����
        float radius = Random.Range(-180, 180);
        moveDir = new(Mathf.Cos(radius), Mathf.Sin(radius), 0f);
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        isMove = true;
        monsterAnim.SetBool("isWalking", true);
        if (moveDir.x != 0)
        {
            //monsterAnim.SetInteger("hDir", moveDir.x > 0 ? 1 : -1);
            if (moveDir.x < 0)
            {
                is_right = false;
            }
            else
            {
                is_right = true;
            }
        }
        latestMoveStartTime = Time.time;
    }

    // �޽� ����
    void StartRest()
    {
        isMove = false;
        monsterAnim.SetBool("isWalking", false);
        latestRestStartTime = Time.time;
    }

    // �ǰ�
    public void Damaged()
    {
        if (hp > 0)
        {
            // �ǰ� ���
            monsterAnim.SetTrigger("Damaged");

            // ������ ǥ��
            GameObject text = Instantiate(damageText, transform.position, Quaternion.identity);
            text.GetComponent<DamageText>().damage = 1;
            text.transform.position = damagePos.position;
            hp -= 1;

            // ü���� 0 ���ϰ� �Ǹ� ���
            if (hp <= 0)
            {
                Death();
            }
        }
    }

    // ���
    private void Death()
    {
        // ���� �÷��̾ ��ġ�� Ÿ�ϸ��� ��´�.
        Player player = GameObject.Find("Player").GetComponent<Player>();
        Tilemap tilemap = GameObject.Find(player.currentFieldName).transform.Find("Map").GetComponent<Tilemap>();

        monsterAnim.SetBool("isDeath", true); // ��� ���
        dropTable.DropItem(tilemap, transform.position); // ������ ���
        spawnManager.monsterCount--; // SpawnManager�� monsterCount 1 ����
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���⿡ ������ �ǰ�
        if (other.CompareTag("Weapon"))
        {
            Damaged();
        }
    }

    // ���� ����
    public void StartTracking()
    {
        isTracking = true;
        isMove = true;
        monsterAnim.SetBool("isWalking", true);
        moveSpeed = trackingSpeed;
    }

    // ����
    public void Tracking(Vector3 playerPos)
    {
        // �ǰ�, ����, ��� ���°� �ƴϸ� �÷��̾� �������� �̵��Ѵ�.
        if (!DoHurtAnim() && !DoAttackAnim() && !DoDeathAnim())
        {
            // ��ǥ���� �÷��̾�� ����
            agent.SetDestination(playerPos);
            // ������ �����ӿ� ���� �ִϸ��̼� ����
            if (agent.desiredVelocity.x != 0)
            {
                //monsterAnim.SetInteger("hDir", agent.desiredVelocity.x > 0 ? 1 : -1);
                if (agent.desiredVelocity.x < 0)
                {
                    is_right = false;
                }
                else
                {
                    is_right = true;
                }
            }
        }
        // �ǰ�, ����, ��� ���¸� ��� ���� ����
        else
        {
            if (agent.hasPath)
            {
                agent.ResetPath();
            }
        }
    }

    // ���� ����
    public void StopTracking()
    {
        isTracking = false;
        if (agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    // ������ �þ߸� ���� ���ͷ� ��ȯ
    public Vector3 GetSight()
    {
        int h_dir = is_right ? 1 : -1;
        return new(1.0f * h_dir, 0f, 0f);
    }

    // ����
    public void Attack()
    {
        monsterAnim.SetTrigger("Attack");

        // ���� ��Ʈ�ڽ��� ������ ��� �ݶ��̴����� ������ �� ���Ͱ� ������ �ǰ��Ѵ�.
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackHitBoxPos.position, attackHitBoxSize, 0);
        Debug.Log(collider2Ds.Length);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Player"))
            {
                collider2D.GetComponent<Player>().Damaged();
                break;
            }
        }
    }

    // ���� ��Ʈ �ڽ��� Ȯ���ϱ� ���� Gizmos �׸���
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackHitBoxPos.position, attackHitBoxSize);
    }
}
