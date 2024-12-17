using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public GameManager gameManager;//�Q�[���I�[�o�[��N���A�Ȃǂ���������Gamemanager�ɂ��Ă���X�N���v�g�̏����擾���邽�߂̊֐�

    public float speed = 10;
    private bool moving = false;
    //�_�b�V���֌W
    public float dashSpeed = 20;
    public float dashDistance = 50;
    private float dashY;//�i���͂����Ă��Ȃ��j�_�b�V������Ƃ��ɍ������ς��Ȃ��悤��
    private bool dashing = false;//�_�b�V���i�ƃX�[�p�[�_�b�V���j����true�ɂȂ�
    public float dashRecastTime = 0.5f;//�_�b�V�����܂��ł���܂ł̎���
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    public GameObject dashDrainEffect;//�z���ł������ɏo��G�t�F�N�g

    //�X�[�p�[�_�b�V���i�`���[�W�_�b�V���j�֌W
    public float superDashchargeTime;//�`���[�W�_�b�V���p�̃`���[�W����
    private float superDashTimeCount = 0;//�`���[�W���Ԃ̃J�E���g
    public float superDashSpeed = 20;
    public float superDashDistance = 60;
    private float superDashY;//�i���͂����Ă��Ȃ��j�`���[�W���ƃX�[�p�[�_�b�V������Ƃ��ɍ������ς��Ȃ��悤��
    private bool superDashing = false;//�X�[�p�[�_�b�V������true�ɂȂ�
    public float superDashRecastTime = 0.5f;//�_�b�V�����܂��ł���܂ł̎���
    private bool superDashTimeRecast = false;
    private bool superDashGroundRecast = false;
    public GameObject superDashdrainEffect;//�X�[�p�[�_�b�V���ŋz���ł������ɏo��G�t�F�N�g
    //�G�i�W�[�֌W
    public float maxEnergy = 10;
    public float energy;
    public float getEnergy = 1;
    public Slider sliderEnergy;
    public Image sliderEnergyImage;//�G�i�W�[�̃X���C�_�[�̐F��ς���p
    private Color originalEnergySliderColor;
    //�G�i�W�[�U���֌W
    public float energyCost = 4;
    public GameObject energyBullet;
    public float shootPos = 1; //�e���v���C���[����ǂ̒��x�O�Ō���
    //�W�����v�֌W
    public float jumpForce = 10f;  // �W�����v��
    public float holdJumpMultiplier = 0.5f;  // �W�����v�{�^���������������ꍇ�̗�
    public float maxHoldTime = 0.2f;  // �W�����v�{�^��������������ő厞��
    private float jumpTimeCounter;//2�e�W�����v�Ƃ����鎞�̂��߂����܂��g���Ă��Ȃ�
    private bool isJumping = false;
    public GroundCheck ground;//�ڒn����p�̃X�N���v�g
    private bool isGround;
    private float originagGravity;
    //�U���֌W
    public Attack attack;
    private bool beginAttack = false;//�U���̃A�j���[�V�������n�߂邽�߂����̂��́A�U���J�n���̈�u����true�ɂȂ�
    private float countAttack = 0;//�U���̃��L���X�g�܂ł̎��Ԃ��L�^����p�̕ϐ�
    private bool notFlipAttack = false;//�U�����͔��]���Ȃ��p

    private Animator anim;//�A�j���[�^�[
    private Vector2 inputDirection;//�C���v�b�g�V�X�e�����g���Ď������悤�Ƃ��������ł܂��g���Ă��Ȃ�
    private Rigidbody2D rb;//�v���C���[�̃��W�b�h�{�f�B������ϐ��A���x�Ƃ��d�͂Ƃ����W�b�h�{�f�B���g���K�v�̂��鑀��̂��߂ɕϐ�������Ă���
    public int maxHp = 10;//�̗͂̍ő�l
    public int hp;//�̗�
    public Slider sliderHp;//HP�o�[
    private bool damaged = false;//�_���[�W���󂯂���damagedTime�̎��ԕ�����true�ɂȂ�i�_���[�W���󂯂��A�j���[�V�������Đ�����p�j
    public float damagedTime = 0.2f;//�_���[�W���󂯂��A�j���[�V�������Đ����鎞��
    private bool stop = false;//���̕ϐ���true�̎��̓v���C���[���~������i���o���ȂǗp�j

    public float invincibleTime = 0.5f;  // ���G���ԁi�_�Ŏ��ԁj
    public float blinkInterval = 0.1f;  // �_�ł̊Ԋu
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    
    public enum PlayerState//�v���C���[�̏��
    {
        Stop,       //��~���i���o���Ȃǂœ����Ȃ��j        
        Idle,       // �ʏ��ԁA�����Ă��Ȃ��ҋ@��
        Moving,     // �ړ���
        Jumping,    // �W�����v��
        NormalAttacking,  // �ʏ�U����
        EnergyBullet,//�G�i�W�[�e�������Ă���d��
        Dashing,    // �_�b�V����
        SuperDashing,//�X�[�p�[�_�b�V����
        SuperDashCharging,//�X�[�p�[�_�b�V�����`���[�W��
        Stunned     // �X�^�����i��Ƃ��Ēǉ��j
    }

    private PlayerState playerState = PlayerState.Idle;
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
        AnimSet();
        if (playerState != PlayerState.Stop)
        {
            Jump();
            Flip();
            Move();
            Attack();
            Dash();
            EnergyBullet();
        }
    }

    private bool JudgeNormalState()//�U����_�b�V�����͑��̓��삪�ł��Ȃ��悤�ɂ������̂ŁA���̓�������Ă�����Idle,Moving,Jumping���̂�true��Ԃ��֐�
    {
        return (playerState == PlayerState.Idle || playerState == PlayerState.Moving || playerState == PlayerState.Jumping);
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//�ݒn����
        if (isGround){/*Debug.Log("PlayerGround");*/}
        if (!isGround && (playerState == PlayerState.Idle || playerState == PlayerState.Moving))
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
        anim.SetBool("damage", damaged);
        anim.SetBool("dashing", playerState == PlayerState.Dashing);
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
            Debug.Log("Attack");
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
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

     private bool JudgeFlip()//�U��������Ԃ̎��ɂ�true��Ԃ��B����JudgeNormalState()�ƃX�[�p�[�_�b�V���̃`���[�W���͐U�������
    {
        return JudgeNormalState() || (playerState == PlayerState.SuperDashCharging);
    }

    private void Move()//���E�ړ��i�W�����v���ł��ړ��ł���j
    {
        if (JudgeNormalState() || playerState == PlayerState.NormalAttacking) {
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

    private bool JudgeMovable()
    {
        return playerState == PlayerState.Idle ||playerState == PlayerState.Moving || playerState == PlayerState.Jumping || playerState == PlayerState.NormalAttacking;
    }

    private void Dash()//�_�b�V���p
    {
        if (Input.GetKeyDown(KeyCode.C) && (dashTimeRecast == false))
        {
            StartCoroutine(DashCoroutine());//��Ԃ���莞��Dashing�ɂ��āAdashTimeRecast����莞��true�ɂ��ă_�b�V���ł��Ȃ������肷��R���[�`��
        }else if (playerState == PlayerState.Dashing)
        {
            rb.velocity = transform.right * dashSpeed;//���x��ݒ�
            Debug.Log("PlayerState.Dashing");
        }
    }
    IEnumerator DashCoroutine()//�_�b�V�����̃R���[�`��
    {
        rb.velocity = transform.right * dashSpeed;
        //rb.gravityScale = 0;
        playerState = PlayerState.Dashing;//�_�b�V�����͏�Ԃ�Dashing��
        dashTimeRecast = true;//�_�b�V���̃��L���X�g���Ԃ��߂���܂�true�ɂ���
        yield return new WaitForSeconds(dashDistance / dashSpeed /*�_�b�V�����̎���*/);
        //rb.gravityScale = originagGravity;
        playerState = PlayerState.Idle;
        rb.velocity = Vector2.zero;//�_�b�V������ɑ��x��0��
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }
    /*
    private void SuperDash()//�`���[�W�_�b�V���p
    {
        if (Input.GetKey(KeyCode.D) && (dashTimeRecast == false))
        {
            StartCoroutine(SuperDashC());//dashTimeRecast����莞��true�ɂ��ă_�b�V���ł��Ȃ����邽�߂����̃R���[�`��
        }
        else if (superDashing)
        {
            rb.velocity = transform.right * dashSpeed;
        }
    }
    IEnumerator SuperDashC()//�`���[�W�_�b�V�����̃R���[�`��
    {
        rb.velocity = transform.right * dashSpeed;
        //rb.gravityScale = 0;
        playerState = PlayerState.SuperDashing;//�_�b�V������true�ɂ���
        dashTimeRecast = true;//�_�b�V���̃��L���X�g���Ԃ��߂���܂�true�ɂ���
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        //rb.gravityScale = originagGravity;
        playerState = PlayerState.Idle;
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }
    */

    private void EnergyBullet()//�G�l���M�[�e�i�O���ɒ��i����e�j������
    {
        if (Input.GetKeyDown(KeyCode.S) && energyCost <= energy)
        {
            Instantiate(energyBullet, transform.position + shootPos * transform.right, transform.rotation);
            useEnergy(energyCost);
        }
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

    private void recoverEnergy()//�G�i�W�[�񕜁A������̃h���C���ŉ񕜂���G�i�W�[�̒l�͈��Ȃ̂ň����Ȃ��ɂ��Ă���
    {
        energy += getEnergy;
        sliderEnergy.value = (float)energy / maxEnergy;
        if (energyCost <= energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor;
        }
    }
    private void Jump()//�W�����v�p
    {
        // �W�����v�J�n
        if (isGround && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            playerState = PlayerState.Jumping;
            jumpTimeCounter = maxHoldTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // �W�����v�{�^���������������ꍇ�̏���
        if (Input.GetKeyDown(KeyCode.Z) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(new Vector2(rb.velocity.x, jumpForce));
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // �W�����v�{�^���𗣂�����W�����v�I��
        if (Input.GetKeyUp(KeyCode.Z))
        {
            isJumping = false;
        }
    }
    public void BodyEnter(Collider2D collision)
    {
        Debug.Log("Player_BodyEnter");
        if (playerState == PlayerState.Dashing)//�_�b�V������
        {
            Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//�G�ꂽ����Ƀh���C���p�C���^�[�t�F�[�X�����邩
            if (drainTarget != null && drainTarget.Drain())
            {
                Instantiate(dashDrainEffect, transform.position, transform.rotation);
                Debug.Log("DashDrainSucceed");
                recoverEnergy();
            }
        }

        /*
        if (collision.tag == "Enemy" && dashing)
        {
            Debug.Log("DashDrain");
            energy += getEnergy;
            sliderEnergy.value = (float)energy / maxEnergy;
        }
        */

        Debug.Log("OntrrigerEnter_Player");
    }
    private bool JudgeInvincible()//��΂Ƀ_���[�W���󂯂Ȃ���ԂȂ�true��Ԃ�
    {
        return isInvincible || playerState == PlayerState.Stop;//�ǂꂩ1�ł�true�Ȃ�false��Ԃ��A�_���[�W���󂯂Ȃ�
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int damage, Vector2 vector, int type)
    {
        Debug.Log("PlayerDamage");
        if (!JudgeInvincible())
        {
            if (playerState == PlayerState.Dashing)//�ʏ�_�b�V����
            {
                if (type >= 1)
                {
                    GetDamage(damage);
                }
            }else if (playerState == PlayerState.SuperDashing)//�X�[�p�[�_�b�V����
            {
                if (type >= 2)
                {
                    GetDamage(damage);
                }
            }
            else{//����ȊO�̏ꍇ�̓_���[�W���󂯂�
                GetDamage(damage);
            }
            /*
            if (!dashing || (type >= 1)){//�_�b�V�����Ă��Ȃ�������Atype��0�łȂ�������_���[�W���󂯂�
                Debug.Log("PlayerDamage_Get");
                hp -= damage;//Hp�����炷
                sliderHp.value = (float)hp / maxHp;//Hp�̃X���C�_�[�̍X�V
                if (hp <= 0)
                {
                    Death();
                }
                else
                {
                    StartCoroutine(BlinkCoroutine(invincibleTime));
                }
            }
            */
        }
        if (vector != Vector2.zero) { rb.velocity = vector; }//�m�b�N�o�b�N 
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
            StartCoroutine(BlinkCoroutine(invincibleTime));
        }
    }
    IEnumerator BlinkCoroutine(float duration)//�_���[�W���󂯂����ɓ_�ł������莞�Ԗ��G�ɂ����肷��
    {
        Debug.Log("BlinkCoroutine");
        isInvincible = true;  // ��莞�Ԗ��G��Ԃɂ���
        float elapsedTime = 0f;
        StartCoroutine(DamageAnim());
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

    IEnumerator DamageAnim()
    {
        damaged = true;
        yield return new WaitForSeconds(damagedTime);
        damaged = false;
    }
    public void Death()
    {
        gameManager.GameOver();
        Destroy(this.gameObject);
    }

}
