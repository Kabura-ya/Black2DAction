using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
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
    //�G�i�W�[�U���֌W
    public float energyCost = 4;
    public GameObject energyBullet;
    public float shootPos = 1; //�e���v���C���[����ǂ̒��x�O�Ō���
    //�W�����v�֌W
    public float jumpForce = 10f;  // �W�����v��
    public float holdJumpMultiplier = 0.5f;  // �W�����v�{�^���������������ꍇ�̗�
    public float maxHoldTime = 0.2f;  // �W�����v�{�^��������������ő厞��
    private float jumpTimeCounter;
    private bool isJumping = false;
    public GroundCheck ground;
    private bool isGround;
    private float originagGravity;
    //�U���֌W
    public Attack attack;
    private bool isAttacking = false;
    private float countAttack = 0;//�U���̃��L���X�g�܂ł̎��Ԃ��L�^����p�̕ϐ�

    private Animator anim;//�A�j���[�^�[
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    public int maxHp = 10;
    public int hp;//�̗�
    public Slider sliderHp;
    private bool damaged = false;//�_���[�W���󂯂���damagedTime�̎��ԕ�����true�ɂȂ�i�_���[�W���󂯂��A�j���[�V�������Đ�����p�j
    public float damagedTime = 0.2f;//�_���[�W���󂯂��A�j���[�V�������Đ����鎞��

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        sliderHp.value = maxHp;
        sliderEnergy.value = 0;
        hp = maxHp;
        energy = 0;
        originagGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        AnimSet();
        Jump();
        Flip();
        Move();
        Attack();
        Dash();
        EnergyBullet();
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//�ݒu����
        if (isGround)
        {
            //Debug.Log("PlayerGround");
        }

    }
    private void AnimSet()
    {
        anim.SetBool("moving", moving);
        anim.SetBool("jumping", isJumping);
        anim.SetBool("attacking", isAttacking);
        anim.SetBool("damage", damaged);
        anim.SetBool("dashing", dashing);
    }
    private void Attack()
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

    private void Flip()
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
        dashing = true;
        dashTimeRecast = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        //rb.gravityScale = originagGravity;
        dashing = false;
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }

    private void EnergyBullet()
    {
        if (Input.GetKey(KeyCode.S) && energyCost <= energy)
        {
            Instantiate(energyBullet, transform.position + shootPos * transform.right, transform.rotation);
            energy -= energyCost;
            sliderEnergy.value = (float)energy / maxEnergy;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && dashing)
        {
            Debug.Log("DashDrain");
            energy += getEnergy;
            sliderEnergy.value = (float)energy / maxEnergy;
        }

        Debug.Log("OntrrigerEnter_Drain");
    }

    private bool InvincibleJudge()//�_���[�W���󂯂��ԂȂ�false��Ԃ�
    {
        return !(isInvincible || dashing);//�ǂꂩ1�ł�true�Ȃ�false��Ԃ��A�_���[�W���󂯂Ȃ�
    } 

    public void Damage(int damage)
    {
        if (InvincibleJudge())//�_���[�W���󂯂��Ԃ����f
        {
            Debug.Log("PlayerDamage");
            hp -= damage;
            sliderHp.value = (float) hp / maxHp;
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
        Destroy(this.gameObject);
    }

}
