//using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public class Boss1 : MonoBehaviour, IDamageable, IDrainable, ISuperDashStunn
{
    public GameManager gameManager;//�Q�[���I�[�o�[��N���A�Ȃǂ���������Gamemanager�ɂ��Ă���X�N���v�g�̏����擾���邽�߂̊֐�

    public int attack = 1;
    public int maxHp = 300;
    public int hp = 300;
    protected GameObject player;//�v���C���[�̏����g����悤�ɂ��Ă���
    protected Transform playerTrans;
    Player playerScript;//�v���C���[�̃X�N���v�g�̊֐��𗘗p�ł���悤�ɂ���
    protected Rigidbody2D rigidbody2d;
    public float dashDistance = 20;
    public float dashSpeed = 10;
    public float idleTime = 1;

    public float fallHight = 3;
    public float fallSpeed = 10;

    public float groundHight;

    private bool start = true;
    private float startTime = 1.3f;//�Ȃ����ŏ��̃A�j���[�V�������������b���ɂ���ƍs�����̃A�j���[�V�����̍Đ�������ɂł��Ȃ���������
    private bool onGround = true;
    private bool enableHit = false;//���ꂪtrue�̎������_���[�W���󂯂���^����
    private bool superDashStunn = false;//���ꂪtrue�̎��i��Ɉꕔ�̐ԍU�����j�ɃX�[�p�[�_�b�V���łԂ�����ƃX�^������B
    private bool stunn =false;//�X�^������true�A�_���[�W���󂯂邪�v���C���[�ɐG��Ă��_���[�W��^���Ȃ�
    private bool moving = false;
    private int action = 1;
    private bool dead = false;
    private bool last = false;

    public AttackEnemy sword;//���U���p�̃N���X
    public AttackEnemy swordFall;//�����U���̎��̓����蔻��
    public SightEnemy swordSight;

    public AttackEnemy redSword;//�ԍU���p

    public GameObject bullet;//�������U���ŕ����
    public GameObject bulletRed;//�ԉ������U���ŕ����

    private Animator anim;//�A�j���[�^�[

    public GameObject damageEffect;
    public float invincibleTime = 0.2f;  // ���G���ԁi�_�Ŏ��ԁj
    public float blinkInterval = 0.1f;  // �_�ł̊Ԋu
    private SpriteRenderer spriteRenderer;
    public Material whiteFlashMaterial; // �����_�ł����邽�߂̃}�e���A��

    public float stunnTime = 3;//�X�^������
    public int stunnMaxCount = 1;
    public int stunnCount = 0;//����܂ŉ���X�^�����������L�^�i�̗͂�������x���邲�ƂɃX�^������悤�ɂ��邽�߁j

    Coroutine actionCoroutine;//���S���ȂǂɃR���[�`�����~�����邽�߂ɁA�s���̃R���[�`���̈��������Ă���
    float noActionCoroutineTime = 0;//actionCoroutine���o�O�Œ�����null�ɂȂ��Ă����ۂɁA���̎��Ԃ��L�^����

    public GameObject redDashEffect;//�ԓːi���J�n�������̏Ռ��g�̃G�t�F�N�g
    public GameObject defeatEffect;//�|�����Ƃ��̃G�t�F�N�g
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        rigidbody2d = GetComponent<Rigidbody2D>();//���g��Rigidbody��ϐ��ɓ����
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();//�v���C���[�̃X�N���v�g�ɑ΂��đ���ł���悤�ɂ���
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        actionCoroutine = StartCoroutine(StartC());
    }

    IEnumerator StartC()//�ŏ��̉��o������
    {
        playerScript.StopPlayer();
        yield return new WaitForSeconds(startTime);
        playerScript.StopInterruptPlayer();
        start = false;
        enableHit = true;
        action = 0;
        yield return new WaitForSeconds(0.1f);
        ChooseAction();
    }

    // Update is called once per frame
    void Update()
    {
        AnimSet();
        /*
        //�Ȃ����ԓːi�̏I���ۂɃX�[�p�[�_�b�V���łԂ���ƃX�^�����Ȃ���Ƀ_�b�V����������o�O������(�����������̂ŃR�����g�A�E�g)�̂ł��̑΍�
        if (actionCoroutine == null)//�o�O�Œ����ԍs�����Ă��Ȃ��ꍇ�s��������
        {
            noActionCoroutineTime += Time.deltaTime;
            if (noActionCoroutineTime > 1)
            {
                noActionCoroutineTime = 0;
                ChooseAction();
            }
        }
        */
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
        anim.SetBool("last", last);
        anim.SetBool("stunn", stunn);
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
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); actionCoroutine = null;}
        if (swordSight.IsPlayerinSight())
        {
            if (0.7 < Random.value)//�v���C���[�����E�̃R���C�_�[���ɋ�����3/10�̊m���Ō��ōU��
            {
                action = 9;
                actionCoroutine = StartCoroutine(Sword());
                return;
            }
        }
        action = Random.Range(1, 5);
        if (action == 1)
        {
            actionCoroutine = StartCoroutine(DashRed());
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
        else if (action == 4)
        {
            actionCoroutine = StartCoroutine(LongRangeRed());
            return;
        }
        /*
        else if (action == 3)
        {
            StartCoroutine(Sword());
            return;
        }*/

    }

    private IEnumerator Dash()//�ʏ�ːi�U���A�ԓːi�������ɂ���̂ő��������g��Ȃ�
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = transform.right * dashSpeed;
        moving = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator DashRed()//�ԓːi�U���A�Ȃ����I���ۂɃX�[�p�[�_�b�V���łԂ���ƃX�^�����Ȃ���Ƀ_�b�V����������o�O������
    {
        Debug.Log("DashRedBegin");
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        yield return new WaitForSeconds(0.8f);
        superDashStunn = true;
        Instantiate(redDashEffect, transform.position, transform.rotation);
        rigidbody2d.velocity = transform.right * dashSpeed;
        redSword.EnableAttack();
        moving = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        rigidbody2d.velocity = new Vector2(0, 0);
        redSword.DisableAttack();
        moving = false;
        superDashStunn = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        Debug.Log("DashRedEnd");
        ChooseAction();
    }

    private IEnumerator Fall()//�����U��
    {
        enableHit = false;
        yield return new WaitForSeconds(idleTime);
        onGround = false;
        transform.position = new Vector2(playerTrans.position.x, fallHight);
        swordFall.EnableAttack();
        enableHit = true;
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = false;
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = new Vector2(0, -1 * fallSpeed);
        moving = true;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(idleTime);
        //�A�j���[�V�����̕��ł��A���n�����ۂ̃A�j���[�V�����ŃR���C�_�[�𖳌������Ă���
        swordFall.DisableAttack();
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private void EnabelAttack_Fall()
    {
        swordFall.EnableAttack();
    }
    private void DisableAttack_Fall()
    {
        swordFall.DisableAttack();
    }

    private IEnumerator Sword()//�ߋ����U��
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        moving = true;
        //�A�j���[�V�����̕��� EnabeleAttack_Sword()�����s
        yield return new WaitForSeconds(idleTime);
        sword.DisableAttack();//
        moving = false;
        yield return new WaitForSeconds(idleTime);
        action = 0;
        sword.DisableAttack();
        yield return new WaitForSeconds(0.1f);
        ChooseAction();
    }
    private void EnableAttack_Sword()
    {
        sword.EnableAttack();
    }
    private void DisableAttack_Sword()
    {
        sword.DisableAttack();
    }

    private IEnumerator LongRange()//�������U��
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        //���̂ւ�ŃA�j���[�V�����̕�����LongRangeSpawn();���Ă΂�Ēe���o��
        yield return new WaitForSeconds(idleTime);
        action = 0;
        yield return new WaitForSeconds(0.1f);
        if (0.7 < Random.value)//�ʏ퉓�����U����3/10�̊m���Őԉ������U��������
        {
            action = 4;
            actionCoroutine = StartCoroutine(LongRangeRed());
        }
        else
        {
            ChooseAction();
        }
    }

    private void LongRangeSpawn()//�e�𔭎˂��邽�߂̊֐��A�A�j���[�V�����̕��ŌĂ�
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    private IEnumerator LongRangeRed()//�Ԃ̉������U��
    {
        FlipToPlayer();
        yield return new WaitForSeconds(2.5f);
        //���̂ւ�ŃA�j���[�V�����̕�����LongRangeSpawn();���Ă΂�Ēe���o��
        yield return new WaitForSeconds(idleTime);
        action = 0;
        yield return new WaitForSeconds(0.1f);
        ChooseAction();
    }

    private void LongRangeRedSpawn()//�e�𔭎˂��邽�߂̊֐��A�A�j���[�V�����̕��ŌĂ�
    {
        Instantiate(bulletRed, transform.position, transform.rotation);
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
        
        if (collision.gameObject.tag == "Player" && enableHit && !stunn)
        {
            //Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                //damageTarget.Damage(attack);�v���C���[�ւ̐ڐG�_���[�W
            }
        }
        
        if (superDashStunn && collision.gameObject.tag == "Player" && playerScript.StateSuperDashing())//�ꕔ�̐ԍU�����ȂǂɃX�[�p�[�_�b�V���łԂ�����ƃX�^������
        {
            //Stunn();
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
            }else if (hp <= maxHp / 2 && stunnCount == 0)//HP�������ɂȂ�����X�^��(�X�^���̃e�X�g�p�Ȃ̂Ō�ŏ�������)
            {
                FlipToPlayer();
                Stunn();
                stunnCount++;
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
        // �_�ŏI����ɃX�v���C�g��\������
        spriteRenderer.enabled = true;
    }

    private void DisableAllAttack()//�X�^�����ȂǂɑS�Ă̍U���p�R���C�_�[�𖳌�������
    {
        sword.DisableAttack();
        swordFall.DisableAttack();
        redSword.DisableAttack();

    }

    private void NextAction(IEnumerator nextAction)//�܂��g���Ă��Ȃ��ォ��g������
    {
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }//���݂̍s���𒆎~�����Ă���X�^��������
        actionCoroutine = null;
        actionCoroutine = StartCoroutine(nextAction);
    }
    public void SuperDashStunn()
    {
        if (superDashStunn)//�ꕔ�̐ԍU�����ȂǂɃX�[�p�[�_�b�V���łԂ�����ƃX�^������
        {
            Debug.Log("SuperDashStunn");
            Stunn();
        }
    }
    private void Stunn()
    {
        if (actionCoroutine != null){ StopCoroutine(actionCoroutine);}//���݂̍s���𒆎~�����Ă���X�^��������
        actionCoroutine = null;
        actionCoroutine = StartCoroutine(StunnCoroutine());
    }

    IEnumerator StunnCoroutine()
    {
        Debug.Log("StunnCoroutine");

        //�X�^�����ɗl�X�ȗv�f�����Z�b�g
        superDashStunn = false;
        DisableAllAttack();
        action = -1;
        stunn = true;
        enableHit = true;
        moving = false;

        rigidbody2d.velocity = transform.right * -12 + transform.up * 3;
        rigidbody2d.gravityScale = 1;
        yield return new WaitForSeconds(0.2f);
        rigidbody2d.velocity = transform.right * -3 ;
        yield return new WaitForSeconds(0.4f);
        action = -2;
        rigidbody2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunnTime);
        rigidbody2d.gravityScale = 0;
        action = 0;
        yield return new WaitForSeconds(0.1f);
        stunn = false;
        ChooseAction();
    }
    public void Death()//�̗͂�0�ȉ��ɂȂ������ɌĂ΂�A���ł���
    {
        StartCoroutine(DeathC());
    }

    IEnumerator DeathC()
    {
        FlipToPlayer();
        DisableAllAttack();
        Instantiate(defeatEffect, transform.position, transform.rotation);
        gameManager.GameClear();
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
        enableHit = false;
        dead = true;
        action = 10;
        rigidbody2d.gravityScale = 1;
        rigidbody2d.velocity = transform.right * -7 + transform.up * 4;
        yield return new WaitForSeconds(0.4f);
        rigidbody2d.velocity = Vector2.zero;
        dead = false;
        last = true;
        yield return new WaitForSeconds(3);
        //Destroy(this.gameObject);
    }


    public bool Drain()//Body�̎q�I�u�W�F�N�g����Ă΂��
    {
        return  enableHit;//�_���[�W����Ƃ���L���ɂ��Ă���Ԃ̂݃h���C���\
    }
    public bool SuperDrain()
    {
        return Drain();
    }
}