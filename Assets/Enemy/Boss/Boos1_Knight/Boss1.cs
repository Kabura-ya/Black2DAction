using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Boss1 : MonoBehaviour, IDamageable, IDrainable
{
    public GameManager gameManager;//�Q�[���I�[�o�[��N���A�Ȃǂ���������Gamemanager�ɂ��Ă���X�N���v�g�̏����擾���邽�߂̊֐�

    public int attack = 1;
    public int hp = 10;
    protected GameObject player;//�v���C���[�̏����g����悤�ɂ��Ă���
    protected Transform playerTrans;
    Player playerScript;//�v���C���[�̃X�N���v�g�̊֐��𗘗p�ł���悤�ɂ���
    protected Rigidbody2D rigidbody2d;
    private bool dashing;
    public float dashDistance = 20;
    public float dashSpeed = 10;
    public float idleTime = 1;

    public float fallHight = 3;
    public float fallSpeed = 10;

    public float groundHight;

    private bool start = true;
    private bool onGround = true;
    private bool enableHit = false;//���ꂪtrue�̎������_���[�W���󂯂���^����
    private bool moving = false;
    private int action = 1;
    private bool dead = false;

    public AttackEnemy sword;//���U���p�̃N���X
    public SightEnemy swordSight;

    public GameObject bullet;//�������U���ŕ����

    private Animator anim;//�A�j���[�^�[

    public GameObject damageEffect;
    public float invincibleTime = 0.2f;  // ���G���ԁi�_�Ŏ��ԁj
    public float blinkInterval = 0.1f;  // �_�ł̊Ԋu
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    public Material whiteFlashMaterial; // �����_�ł����邽�߂̃}�e���A��


    Coroutine actionCoroutine;//���S���ȂǂɃR���[�`�����~�����邽�߂ɁA�s���̃R���[�`���̈��������Ă���
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();//���g��Rigidbody��ϐ��ɓ����
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();//�v���C���[�̃X�N���v�g�ɑ΂��đ���ł���悤�ɂ���
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartC());
    }

    IEnumerator StartC()//�ŏ��̉��o������
    {
        playerScript.StopPlayer();
        yield return new WaitForSeconds(2);
        playerScript.StopInterruptPlayer();
        start = false;
        enableHit = true;
        ChooseAction();
    }

    // Update is called once per frame
    void Update()
    {
        AnimSet();
    }

    private void AnimSet()
    {
        //Debug.Log("AnimSet");
        //Debug.Log(action);
        anim.SetBool("start", start);
        anim.SetInteger("action", action);
        anim.SetBool("onGround", onGround);
        /*
        if (rigidbody2d.velocity.x == 0 && rigidbody2d.velocity.y == 0)
        {
            anim.SetBool("moving", false);
        }
        else
        {
            anim.SetBool("moving", true);
        }
        */
        anim.SetBool("moving", moving);
        anim.SetBool("dead", dead);
    }

    protected void FlipToPlayer()//Player�̕�������
    {
        if (playerTrans.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void ChooseAction()
    {

        if (swordSight.IsPlayerinSight())
        {
            if (0.5 < Random.value)//�v���C���[�����E�̃R���C�_�[���ɋ�����1/2�̊m���Ō��ōU��
            {
                action = 3;
                actionCoroutine = StartCoroutine(Sword());
                return;
            }
        }
        action = Random.Range(1, 4);
        if (action == 1)
        {
            actionCoroutine = StartCoroutine(Dash());
            return;
        }
        else if (action == 2)
        {
            actionCoroutine = StartCoroutine(Fall());
            return;
        }
        else if (action == 3)
        {
            actionCoroutine = StartCoroutine(LongRange());
            return;
        }
        
        /*
        else if (action == 3)
        {
            StartCoroutine(Sword());
            return;
        }*/
        
    }

    private IEnumerator Dash()//�ːi�U��
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = transform.right * dashSpeed;
        dashing = true;
        moving = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        rigidbody2d.velocity = new Vector2(0, 0);
        dashing = false;
        moving = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator Fall()//�����U��
    {
        enableHit = false;
        yield return new WaitForSeconds(idleTime);
        onGround = false;
        transform.position = new Vector2(playerTrans.position.x, fallHight);
        enableHit = true;
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = false;
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = new Vector2(0, -1 * fallSpeed);
        moving = true;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator Sword()//�ߋ����U��
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        sword.EnableAttack();
        yield return new WaitForSeconds(idleTime);
        sword.DisableAttack();
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator LongRange()
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        Instantiate(bullet, transform.position, transform.rotation);
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("OnTrigger");
        if (collision.gameObject.tag == "Player" && enableHit)
        {
            //Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }
        }
    }
    */
    public void BodyStay(Collider2D collision)//Body�����̎q�I�u�W�F�N�g��OnTriggerStay�ŌĂ΂��
    {
        //Debug.Log("OnTrigger");
        if (collision.gameObject.tag == "Player" && enableHit)
        {
            //Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//�ڒn����i�G���n�ʂɐG�ꂽ���𔻒肷�邽�߂ɂ킴�킴EnemyGroundCheck�^�O�����ɂ������Ă���j
    {
        if (collision.gameObject.tag == "EnemyGroundCheck")
        {
            onGround = true;
            moving = false;
        }
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int value, Vector2 vector, int type)
    {
        if (enableHit)
        {
            hp -= value;
            Instantiate(damageEffect, transform.position, transform.rotation);

            if (hp <= 0)
            {
                Death();
            }
            else
            {
                StartCoroutine(BlinkCoroutine(invincibleTime));
            }
        }
    }

    IEnumerator BlinkCoroutine(float duration)//�_���[�W���󂯂����Ɉ�u�_�ł���
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // �X�v���C�g�̉�����؂�ւ���
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // �_�ł̊Ԋu�����҂�
            yield return new WaitForSeconds(blinkInterval);

            // �o�ߎ��Ԃ��X�V
            elapsedTime += blinkInterval;
        }
        isInvincible = false;
        // �_�ŏI����ɃX�v���C�g��\������
        spriteRenderer.enabled = true;
    }

    public void Death()//�̗͂�0�ȉ��ɂȂ������ɌĂ΂�A���ł���
    {
        StartCoroutine(DeathC());
    }

    IEnumerator DeathC()
    {
        gameManager.GameClear();
        StopCoroutine(actionCoroutine);
        enableHit = false;
        dead = true;
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    public bool Drain()//Body�̎q�I�u�W�F�N�g����Ă΂��
    {
        return  enableHit;//�_���[�W����Ƃ���L���ɂ��Ă���Ԃ̂݃h���C���\
    }
}