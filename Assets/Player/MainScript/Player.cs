using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Player;
public class Player : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public GameManager gameManager;//�Q�[���I�[�o�[��N���A�Ȃǂ���������Gamemanager�ɂ��Ă���X�N���v�g�̏����擾���邽�߂̊֐�

    public enum PlayerState//�v���C���[�̏��
    {
        Stop,       //��~���i���o���Ȃǂœ����Ȃ��j        
        Idle,       // �ʏ��ԁA�����Ă��Ȃ��ҋ@��
        Moving,     // �ړ���
        Jumping,    // �W�����v��
        NormalAttacking,  // �ʏ�U����
        EnergyBullet,//�G�i�W�[�e�������Ă���d��
        Dashing,    // �_�b�V����
        SuperDashCharging,//�X�[�p�[�_�b�V�����`���[�W��
        SuperDashCharged,//�X�[�p�[�_�b�V�����`���[�W����
        SuperDashing,//�X�[�p�[�_�b�V����
        Stunned     // �X�^�����A�_���[�W���󂯂������u����ł��Ȃ�
    }

    PlayerState playerState = PlayerState.Idle;
    private PlayerState previousPlayerState = PlayerState.Idle;//1�t���[���O��playerState���L�^���A�A�j���]�V�����J�ڂɎg������

    public bool printLog = false;//���ꂪtrue�̎��ɐF�X��Log���o�͂���
    public float speed = 10;
    //�_�b�V���֌W
    private bool beginDash = false;
    public float dashSpeed = 20;
    public float dashDistance = 50;
    private float dashY;//�i���͂����Ă��Ȃ��j�_�b�V������Ƃ��ɍ������ς��Ȃ��悤��
    public float dashRecastTime = 0.3f;//�_�b�V�����܂��ł���܂ł̎���
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    public GameObject dashDrainEffect;//�z���ł������ɏo��G�t�F�N�g

    //�X�[�p�[�_�b�V���i�`���[�W�_�b�V���j�֌W
    public float superDashChargeTime = 0.5f;//�`���[�W�_�b�V���p�̃`���[�W����
    private float superDashChargeTimeCount = 0;//�`���[�W���Ԃ̃J�E���g
    public float superDashSpeed = 20;
    public float superDashDistance = 60;
    private float superDashY;//�i���͂����Ă��Ȃ��j�`���[�W���ƃX�[�p�[�_�b�V������Ƃ��ɍ������ς��Ȃ��悤��
    public float superDashRecastTime = 0.5f;//�_�b�V�����܂��ł���܂ł̎���
    private bool superDashGroundRecast = false;
    public GameObject superDashDrainEffect;//�X�[�p�[�_�b�V���ŋz���ł������ɏo��G�t�F�N�g
    //�G�i�W�[�֌W
    public float maxEnergy = 10;
    public float energy;
    public float getEnergy = 1;//�ʏ�_�b�V���Ŏ擾����G�i�W�[
    public float getSuperEnergy = 2;//�X�[�p�[�_�b�V���Ŏ擾����G�i�W�[
    public Slider sliderEnergy;
    public Image sliderEnergyImage;//�G�i�W�[�̃X���C�_�[�̐F��ς���p
    private Color originalEnergySliderColor;
    //�G�i�W�[�U���֌W
    public float energyCost = 4;//1��ł̃G�i�W�[�����
    public GameObject energyBullet;
    public float energyBulletRecastTime = 0.1f;
    public float shootPos = 1; //�e���v���C���[����ǂ̒��x�O�ɏo�������Č���

    //�W�����v�֌W
    public float jumpForce = 100f;  // �W�����v��
    public float holdJumpMultiplier = 0.5f;  // �W�����v�{�^���������������ꍇ�̗�
    public float maxHoldTime = 0.5f;  // �W�����v�{�^��������������ő厞��
    private float jumpTimeCounter;//2�e�W�����v�Ƃ����鎞�̂��߂����܂��g���Ă��Ȃ�
    private bool isJumping = false;
    public GroundCheck ground;//�ڒn����p�̃X�N���v�g
    private bool isGround;
    private float originagGravity = 4;
    //�U���֌W
    public Attack attack;
    private bool beginAttack = false;//�U���̃A�j���[�V�������n�߂邽�߂����̂��́A�U���J�n���̈�u����true�ɂȂ�
    private float countAttack = 0;//�U���̃��L���X�g�܂ł̎��Ԃ��L�^����p�̕ϐ�

    private Animator anim;//�A�j���[�^�[
    private Vector2 inputDirection;//�C���v�b�g�V�X�e�����g���Ď������悤�Ƃ��������ł܂��g���Ă��Ȃ�
    private Rigidbody2D rb;//�v���C���[�̃��W�b�h�{�f�B������ϐ��A���x�Ƃ��d�͂Ƃ����W�b�h�{�f�B���g���K�v�̂��鑀��̂��߂ɕϐ�������Ă���
    public int maxHp = 10;//�̗͂̍ő�l
    public int hp;//�̗�
    public Slider sliderHp;//HP�o�[
    public float damagedTime = 0.1f;//�_���[�W���󂯂�����ő���o���Ȃ�����

    public float invincibleTime = 0.5f;  // ���G���ԁi�_�Ŏ��ԁj
    public float blinkInterval = 0.1f;  // �_�ł̊Ԋu
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    Coroutine actionCoroutine;//�X�^�����ȂǂɃR���[�`�����~�����邽�߂ɁA�s���̃R���[�`���̈��������Ă���


    void Start()
    {
        playerState = PlayerState.Idle;//�ŏ��͑ҋ@��Ԃ�
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sliderHp.value = maxHp;
        sliderEnergy.value = 0;
        sliderEnergyImage = sliderEnergy.fillRect.GetComponent<Image>();
        originalEnergySliderColor = sliderEnergyImage.color;//�G�i�W�[�̃X���C�_�[�̐F���L�^
        if (energyCost > energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor / 3;
        }//energy���Z����ĂȂ��ʂȂ�X���C�_�[�̐F���Â�����
        if (sliderEnergyImage == null)
        {
            Debug.Log("sliderEnergyImage == null");
        }
        hp = maxHp;
        energy = 0;
        originagGravity = rb.gravityScale;//�ŏ��̏d�͂̒l���L�^
    }

    // Update is called once per frame
    void Update()
    {
        rb.gravityScale = originagGravity;
        if (playerState == PlayerState.Stop)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else{
            Jump();
            Flip();
            Attack();
            Dash();
            SuperDashCharging();
            SuperDashCharged();
            SuperDash();
            Move();
            EnergyBullet();
        }
        AnimSet();//�A�j���[�V�����p�Ȃ̂ŏ�̐F�X�Ȋ֐��̉��ł���K�v������
        PrintPlayerState();

        previousPlayerState = playerState;//Update�̍Ō�ɒu��
    }
    private void PrintPlayerState()
    {
        if (printLog)
        {
            switch (playerState)
            {
                case PlayerState.Stop:
                    Debug.Log("PlayerState.Stop");
                    break;
                case PlayerState.Idle:
                    Debug.Log("PlayerState.Idle");
                    break;
                case PlayerState.Moving:
                    Debug.Log("PlayerState.Moving");
                    break;
                case PlayerState.Jumping:
                    Debug.Log("PlayerState.Jumping");
                    break;
                case PlayerState.NormalAttacking:
                    Debug.Log("PlayerState.NormalAttacking");
                    break;
                case PlayerState.EnergyBullet:
                    Debug.Log("PlayerState.EnergyBullet");
                    break;
                case PlayerState.Dashing:
                    Debug.Log("PlayerState.Dashing");
                    break;
                case PlayerState.SuperDashCharging:
                    Debug.Log("PlayerState.SuperDashCharging");
                    break;
                case PlayerState.SuperDashCharged:
                    Debug.Log("PlayerState.SuperDashCharged");
                    break;
                case PlayerState.SuperDashing:
                    Debug.Log("PlayerState.SuperDashing");
                    break;
                case PlayerState.Stunned:
                    Debug.Log("PlayerState.Stunned");
                    break;
                default:
                    Debug.Log("Unknown PlayerState");
                    break;
            }
        }
    }

    private bool JudgeNormalState()//�U����_�b�V�����͑��̓��삪�ł��Ȃ��悤�ɂ������̂ŁA���̓�������Ă�����Idle,Moving,Jumping���̂�true��Ԃ��֐�
    {
        return (playerState == PlayerState.Idle || playerState == PlayerState.Moving || playerState == PlayerState.Jumping);
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//�ݒn����
        if(printLog)//if (isGround){Debug.Log("PlayerGround");}
        
        if (!isGround && JudgeNormalState())
        { 
            playerState = PlayerState.Jumping;//�n�ʂ��痣�ꂽ�痎����Ԃ�
        }
        if (isGround && playerState == PlayerState.Jumping)
        {
            playerState = PlayerState.Idle;//�W�����v���◎�����ɒn�ʂɒ�������Idle��Ԃ�
        }
    }

    public void StopPlayer()//���o�ȂǂŃv���C���[�𑀍�ł��Ȃ�������p
    {
        playerState = PlayerState.Stop;
    }
    public void StopInterruptPlayer()//StopPlayer()�Ŏ~�߂��̂�߂��p
    {
        playerState = PlayerState.Idle;
    }
    private void AnimSet()
    {
        anim.SetBool("moving", playerState == PlayerState.Moving);
        anim.SetBool("jumping", !isGround);
        anim.SetBool("attacking", beginAttack);
        anim.SetBool("damage", playerState == PlayerState.Stunned);
        anim.SetBool("beginDash", beginDash);
        anim.SetBool("dashing", playerState == PlayerState.Dashing);
        anim.SetBool("charging", playerState == PlayerState.SuperDashCharging);
        anim.SetBool("charged", playerState == PlayerState.SuperDashCharged);
        anim.SetBool("superDashing", playerState == PlayerState.SuperDashing);
    }
    private void Attack()//�ߋ����U���i�U���p�̎q�I�u�W�F�N�g�̊֐��Łj
    {
        if (!(JudgeNormalState() || playerState == PlayerState.NormalAttacking))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.X) && (countAttack <= 0))
        {
            countAttack = attack.GetComponent<Attack>().recastTime;
            //EnableAttack()���A�j���[�V�����̕��ŌĂ�
            beginAttack = true;//�A�j���[�V�����J�ڗp�Ɉ�u����true�ɂ���
            playerState = PlayerState.NormalAttacking;//��Ԃ��U������
            //Debug.Log("Attack");
        }
        else
        {
            countAttack -= Time.deltaTime;//�U���̃J�E���g����o�ߎ��Ԃ������Ă���
            beginAttack = false;//�A�j���[�V�����J�ڗp�Ɉ�u����true�ɂ����炷��false�ɂ���
            if (countAttack <= 0)
            {
                countAttack = 0;
                playerState = PlayerState.Idle;
                attack.DisableAttack();//�U���R���C�_�[�𖳌����A�{���̓A�j���[�V������DisableAttack()���Ă�ōU���R���C�_�[�𖳌������邪�A�A�j���[�V���������f���ꂽ���Ȃǂ̂��߂ɂ����Ă���
            }
        }

    }

    private void EnableAttack()//�ʏ�U���p�̃R���C�_�[��L��������A�ʏ�U�����ɁA�A�j���[�V�����̕�����Ă�
    {
        attack.EnableAttack();
    }
    private void DisableAttack()//�ʏ�U���p�̃R���C�_�[�𖳌�������A�ʏ�U�����I���ɁA�A�j���[�V�����̕�����Ă�
    {
        attack.DisableAttack();//���͎g���ĂȂ�
    }

    private void Flip()//���](�_�b�V������U�����͔��]���Ȃ�)
    {
        if (JudgeFlip()) {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                //���E�������ɉ�����Ă����牽�����Ȃ�
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);//�E������
            } else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);//��������
            }
        }
    }

     private bool JudgeFlip()//�U��������Ԃ̎��ɂ�true��Ԃ��B����JudgeNormalState()�ƃX�[�p�[�_�b�V���̃`���[�W���͐U�������
    {
        return JudgeNormalState() || (playerState == PlayerState.SuperDashCharging);
    }

    private void Move()//���E�ړ��i�W�����v���A�U�����ł��ړ��ł���j
    {
        if (JudgeMovable()) {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (isGround && playerState != PlayerState.NormalAttacking) { playerState = PlayerState.Idle; }
            }
            else if (Input.GetKey(KeyCode.RightArrow)) {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (isGround && playerState != PlayerState.NormalAttacking) { playerState = PlayerState.Moving; }
            }
            else if (Input.GetKey(KeyCode.LeftArrow)) {
                rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
                if (isGround && playerState != PlayerState.NormalAttacking){ playerState = PlayerState.Moving;}
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (isGround && playerState != PlayerState.NormalAttacking) {playerState = PlayerState.Idle;}
            }
        }
    }

    private bool JudgeMovable()//�ړ��\�ȏ�ԂȂ�true��Ԃ�
    {
        return playerState == PlayerState.Idle ||playerState == PlayerState.Moving || playerState == PlayerState.Jumping || playerState == PlayerState.NormalAttacking;
    }

    private void Jump()//�W�����v�p
    {

        // �W�����v�{�^���𗣂�����A�_�b�V���Ƃ��`���[�W�Ƃ�������W�����v�I��
        if (Input.GetKeyUp(KeyCode.Z) || playerState == PlayerState.Dashing || playerState == PlayerState.SuperDashCharging)
        {
            isJumping = false;
            jumpTimeCounter = 0;
        }

        if (!(JudgeNormalState() || playerState == PlayerState.Jumping))
        {
            return;
        }
        // �W�����v�J�n
        if (isGround && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            playerState = PlayerState.Jumping;
            jumpTimeCounter = maxHoldTime;
            //rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // �W�����v�{�^���������������ꍇ�̏���
        if (Input.GetKey(KeyCode.Z) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                jumpTimeCounter = 0;
            }
        }
    }

    private void Dash()//�_�b�V���p
    {
        if (!(JudgeNormalState() || playerState == PlayerState.Dashing))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && (dashTimeRecast == false))
        {
            beginDash = true;//�A�j���[�V�����J�ڗp�Ɉ�u����true�ɂ���
            StartCoroutine(DashRecastCoroutine()); //�_�b�V���̃��L���X�g�������������AdashTimeRecast����莞��true�ɂ��ă_�b�V���ł��Ȃ������肷��R���[�`��
            actionCoroutine = StartCoroutine(DashCoroutine());//��Ԃ���莞��Dashing�ɂ���
        }else if (playerState == PlayerState.Dashing)
        {
            beginDash = false;
            rb.velocity = transform.right * dashSpeed;//���x��ݒ�
            rb.gravityScale = 0;
            if(printLog) Debug.Log("PlayerState.Dashing");
        }
    }
    IEnumerator DashCoroutine()//�_�b�V�����̃R���[�`��
    {
        rb.velocity = transform.right * dashSpeed;
        playerState = PlayerState.Dashing;//�_�b�V�����͏�Ԃ�Dashing��
        yield return new WaitForSeconds(dashDistance / dashSpeed /*�_�b�V�����̎���*/);
        playerState = PlayerState.Idle;
        rb.velocity = Vector2.zero;//�_�b�V������ɑ��x��0��
    }
    IEnumerator DashRecastCoroutine()//�_�b�V���̃��L���X�g���Ԃ��߂���܂�true�ɂ��邾��
    {
        dashTimeRecast = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed /*�_�b�V�����̎���*/ + dashRecastTime);
        dashTimeRecast = false;
    }

    private void SuperDashCharging()
    {
        if (playerState != PlayerState.SuperDashCharging) { superDashChargeTimeCount = 0; }
        if (!(JudgeNormalState() || playerState == PlayerState.SuperDashCharging))
        {
            return;
        }

        if (JudgeNormalState() && Input.GetKey(KeyCode.D))//�`���[�W�J�n�̏���
        {
            playerState = PlayerState.SuperDashCharging;
        }
        if (playerState == PlayerState.SuperDashCharging)//�`���[�W���̏���
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = Vector2.zero;//�`���[�W���͂��̏�ɒ�~������
                rb.gravityScale = 0;
                superDashChargeTimeCount += Time.deltaTime;
                if (superDashChargeTimeCount >= superDashChargeTime)
                {
                    playerState = PlayerState.SuperDashCharged;
                }
            }
            else
            {//�`���[�W�{�^���������Ă��Ȃ����
                superDashChargeTimeCount = 0;
                playerState = PlayerState.Idle;
            }
        }
        else { superDashChargeTimeCount = 0; }
}
    private void SuperDashCharged()
    {
        if (playerState != PlayerState.SuperDashCharged){ return;}
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        else {
            playerState = PlayerState.SuperDashing;
            StartCoroutine(SuperDashRecastCoroutine()); //�_�b�V���̃��L���X�g�������������AdashTimeRecast����莞��true�ɂ��ă_�b�V���ł��Ȃ������肷��R���[�`��
            actionCoroutine = StartCoroutine(SuperDashCoroutine());
        }
    }
    
    private void SuperDash()//�X�[�p�[�_�b�V���������ɑ��x�����ɂ��邾���AplayerState��SuperDashing�ɂ��鏈����SuperDashCharge()�̕��ōs��
    {
        if (playerState == PlayerState.SuperDashing)
        {
            rb.velocity = transform.right * superDashSpeed;
            rb.gravityScale = 0;
        }
    }
    IEnumerator SuperDashCoroutine()//�`���[�W�_�b�V�����̃R���[�`��
    {
        playerState = PlayerState.SuperDashing;
        rb.velocity = transform.right * superDashSpeed;
        playerState = PlayerState.SuperDashing;
        yield return new WaitForSeconds(superDashDistance / superDashSpeed);
        playerState = PlayerState.Idle;
    }
    IEnumerator SuperDashRecastCoroutine()//�_�b�V���̃��L���X�g���Ԃ��߂���܂�true�ɂ��邾��
    {
        dashTimeRecast = true;
        yield return new WaitForSeconds(superDashDistance / superDashSpeed /*�_�b�V�����̎���*/ + dashRecastTime);
        dashTimeRecast = false;
    }

    private void EnergyBullet()//�G�l���M�[�e�i�O���ɒ��i����e�j������
    {
        if (!(JudgeNormalState() || playerState == PlayerState.EnergyBullet))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.S) && energyCost <= energy)
        {
            actionCoroutine = StartCoroutine(EnergyBulletCoroutine());
        }
        if (playerState == PlayerState.EnergyBullet)
        {
            rb.velocity = Vector2.zero;//�G�i�W�[�e����ƈ�u��~
            rb.gravityScale = 0;
        }
    }

    IEnumerator EnergyBulletCoroutine()
    {
        Instantiate(energyBullet, transform.position + shootPos * transform.right, transform.rotation);
        useEnergy(energyCost);
        playerState = PlayerState.EnergyBullet;
        yield return new WaitForSeconds(energyBulletRecastTime);
        playerState = PlayerState.Idle;
    }

    private bool useEnergy(float useEnergy)//�G�i�W�[����p
    {
        if (energy >= useEnergy)
        {
            energy -= energyCost;
            sliderEnergy.value = (float)energy / maxEnergy;
            if (energyCost > energy)
            {
                sliderEnergyImage.color = originalEnergySliderColor / 3;
            }
            return true;
        }
        return false;
    }

    private void recoverEnergy(float value)//�G�i�W�[��
    {
        energy += value;
        if (energy > maxEnergy) { energy = maxEnergy; }
        sliderEnergy.value = (float)energy / maxEnergy;
        if (energyCost <= energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor;
        }
    }

    public void BodyEnter(Collider2D collision)//�̂̓����蔻��p�̃R���C�_�[�ɂ����X�N���v�g����Ă΂��
    {
        if(printLog) Debug.Log("Player_BodyEnter");
        if (playerState == PlayerState.Dashing)
        {
            if (printLog) Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//�G�ꂽ����Ƀh���C���p�C���^�[�t�F�[�X�����邩
            if (drainTarget != null && drainTarget.Drain())//�h���C���\��
            {
                Instantiate(dashDrainEffect, transform.position, transform.rotation);
                if (printLog) Debug.Log("DashDrainSucceed");
                recoverEnergy(getEnergy);
            }
        }
        if (playerState == PlayerState.SuperDashing)
        {
            if (printLog) Debug.Log("SuperDashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//�G�ꂽ����Ƀh���C���p�C���^�[�t�F�[�X�����邩
            if (drainTarget != null && (drainTarget.Drain() || drainTarget.SuperDrain()))//�h���C���\���B�ʏ��Drain()��True�Ȃ��SuperDrain()��True�Ȃ͂������A�~�X�����������̂��߂�
            {
                Instantiate(superDashDrainEffect, transform.position, transform.rotation);
                if (printLog) Debug.Log("SuperDashDrainSucceed");
                recoverEnergy(getSuperEnergy);
            }

            //�X�^��������p�̏���
            var stunnTarget = collision.gameObject.GetComponent<ISuperDashStunn>();
            if (stunnTarget != null)//�X�^���\��
            {
                if (printLog) Debug.Log("SuperDasStunnSucceed");
                stunnTarget.SuperDashStunn();
            }
        }

        if (printLog) Debug.Log("OntrrigerEnter_Player");
    }
    private bool JudgeInvincible()//��΂Ƀ_���[�W���󂯂Ȃ���ԂȂ�true��Ԃ�
    {
        return isInvincible || playerState == PlayerState.Stop;
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int damage, Vector2 vector, int type)
    {
        if (printLog) Debug.Log("PlayerDamage");
        if (!JudgeInvincible() && JudgeGetDamageType(type))
        {
            GetDamage(damage);
            if (type == 1) { if (printLog) { Debug.Log("Player_DamageRed"); } } ;
        }
        rb.velocity = vector;
        if (vector != Vector2.zero) { rb.velocity = vector; }//�m�b�N�o�b�N 
    }

    private bool JudgeGetDamageType(int type)//�_���[�W�̎�ނƃv���C���[�̃_�b�V���Ȃǂ̏�Ԃ���A�_���[�W���󂯂��ԂȂ�true��Ԃ�
    {
        return !((playerState == PlayerState.Dashing && type == 0) || (playerState == PlayerState.SuperDashing && type <= 1));
    }
    private void GetDamage(int damage)//���ۂɃ_���[�W���󂯂�HP�����炵���薳�G���ԂƂ��̏���
    {
        hp -= damage;//Hp�����炷
        sliderHp.value = (float)hp / maxHp;//Hp�̃X���C�_�[�̍X�V
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(StuuneTime());
            StartCoroutine(BlinkCoroutine(invincibleTime));
        }
    }

    public bool StateSuperDashing()//�X�[�p�[�_�b�V������������true��Ԃ������A�G�ɃX�[�p�[�_�b�V���łԂ�������X�^���Ƃ��Ɏg��
    {
        return playerState == PlayerState.SuperDashing;
    }
    IEnumerator BlinkCoroutine(float duration)//�_���[�W���󂯂����ɓ_�ł������莞�Ԗ��G�ɂ����肷��
    {
        if (printLog) Debug.Log("BlinkCoroutine");
        isInvincible = true;  // ��莞�Ԗ��G��Ԃɂ���
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

        isInvincible = false;  // ���G��Ԃ�����
    }

    IEnumerator StuuneTime()//��莞�ԃX�^����Ԃ�
    {
        playerState = PlayerState.Stunned;
        yield return new WaitForSeconds(damagedTime);
        playerState = PlayerState.Idle;
    }
    public void Death()
    {
        gameManager.GameOver();
        Destroy(this.gameObject);
    }

}
