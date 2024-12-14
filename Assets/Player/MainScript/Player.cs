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
    private float dashY;//�_�b�V������Ƃ��ɍ������ς��Ȃ��悤��
    private bool dashing = false;
    public float dashRecastTime = 0.5f;//�_�b�V�����܂��ł���܂ł̎���
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    private string drainTag = "Enemy";
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
    private bool isAttacking = false;
    private float countAttack = 0;//�U���̃��L���X�g�܂ł̎��Ԃ��L�^����p�̕ϐ�

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


    public enum PlayerState
    {
        Idle,       // �ҋ@��
        Moving,     // �ړ���
        Jumping,    // �W�����v��
        Attacking,  // �U����
        Dashing,    // �_�b�V����
        Stunned     // �X�^�����i��Ƃ��Ēǉ��j
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sliderHp.value = maxHp;
        sliderEnergy.value = 0;
        sliderEnergyImage = sliderEnergy.fillRect.GetComponent<Image>();
        originalEnergySliderColor = sliderEnergyImage.color;
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
        originagGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        AnimSet();
        if (!stop)
        {
            Jump();
            Flip();
            Move();
            Attack();
            Dash();
            EnergyBullet();
        }
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//�ݒu����
        if (isGround)
        {
            //Debug.Log("PlayerGround");
        }

    }

    public void StopPlayer()//���o�ȂǂŃv���C���[�̓�����~������p
    {
        stop = true;
    }
    public void StopInterruptPlayer()
    {
        stop = false;
    }
    private void AnimSet()
    {
        anim.SetBool("moving", moving);
        anim.SetBool("jumping", !isGround);
        anim.SetBool("attacking", isAttacking);
        anim.SetBool("damage", damaged);
        anim.SetBool("dashing", dashing);
    }
    private void Attack()//�ߋ����U���i�U���p�̎q�I�u�W�F�N�g�̊֐��Łj
    {
        if (Input.GetKeyDown(KeyCode.X) && (countAttack <= 0))
        {
            countAttack = attack.GetComponent<Attack>().recastTime;
            attack.EnableAttack();
            isAttacking = true;
            Debug.Log("Attack");
        }
        else
        {
            countAttack -= Time.deltaTime;
            if (countAttack <= 0) { countAttack = 0; }
            isAttacking = false;
            attack.DisableAttack();//���͎g���ĂȂ�
        }
    }

    private void Flip()//���](�_�b�V�����͔��]���Ȃ�)
    {
        if (!(isAttacking || dashing)) {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                //�������Ȃ�
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

    private void Move()//
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            moving = false;
        }
        else if(Input.GetKey(KeyCode.RightArrow)){
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.velocity = new Vector2(speed, rb.velocity.y);
            moving = true;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
            moving = true;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            moving = false;
        }
    }

    private void Dash()//�_�b�V���p
    {
        if (Input.GetKey(KeyCode.C) && (dashTimeRecast == false))
        {
            StartCoroutine(DashC());//dashTimeRecast����莞��true�ɂ��ă_�b�V���ł��Ȃ����邽�߂����̃R���[�`��
        }else if (dashing)
        {
            rb.velocity = transform.right * dashSpeed;
        }
    }
    IEnumerator DashC()//�_�b�V�����̃R���[�`��
    {
        rb.velocity = transform.right * dashSpeed;
        //rb.gravityScale = 0;
        dashing = true;//�_�b�V������true�ɂ���
        dashTimeRecast = true;//�_�b�V���̃��L���X�g���Ԃ��߂���܂�true�ɂ���
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        //rb.gravityScale = originagGravity;
        dashing = false;
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }

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
        if (dashing)//�_�b�V������
        {
            Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//�G�ꂽ����Ƀh���C���p�C���^�[�t�F�[�X�����邩
            if (drainTarget != null && drainTarget.Drain())
            {
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

    private bool InvincibleJudge()//�_���[�W���󂯂��ԂȂ�true��Ԃ��A�U����type��ݒ肵���e���Ō��݂͖��Ӗ�
    {
        return !(isInvincible || dashing || stop);//�ǂꂩ1�ł�true�Ȃ�false��Ԃ��A�_���[�W���󂯂Ȃ�
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int damage, Vector2 vector, int type)
    {
        Debug.Log("PlayerDamage");
        if (!(isInvincible || stop))
        {
            if (!dashing || (type > 0)){//�_�b�V�����Ă��Ȃ�������Atype��0�łȂ�������_���[�W���󂯂�
                Debug.Log("PlayerDamage_Get");
                hp -= damage;
                sliderHp.value = (float)hp / maxHp;
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
        rb.velocity = vector; 
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
