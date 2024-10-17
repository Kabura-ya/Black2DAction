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
    //ダッシュ関係
    public float dashSpeed = 20;
    public float dashDistance = 50;
    private float dashY;//ダッシュするときに高さが変わらないように
    private bool dashing = false;
    public float dashRecastTime = 0.5f;//ダッシュをまたできるまでの時間
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    private string drainTag = "Enemy";
    //エナジー関係
    public float maxEnergy = 10;
    public float energy;
    public float getEnergy = 1;
    public Slider sliderEnergy;
    //エナジー攻撃関係
    public float energyCost = 4;
    public GameObject energyBullet;
    public float shootPos = 1; //弾をプレイヤーからどの程度前で撃つか
    //ジャンプ関係
    public float jumpForce = 10f;  // ジャンプ力
    public float holdJumpMultiplier = 0.5f;  // ジャンプボタンを押し続けた場合の力
    public float maxHoldTime = 0.2f;  // ジャンプボタンを押し続ける最大時間
    private float jumpTimeCounter;
    private bool isJumping = false;
    public GroundCheck ground;
    private bool isGround;
    private float originagGravity;
    //攻撃関係
    public Attack attack;
    private bool isAttacking = false;
    private float countAttack = 0;//攻撃のリキャストまでの時間を記録する用の変数

    private Animator anim;//アニメーター
    private Vector2 inputDirection;
    private Rigidbody2D rb;
    public int maxHp = 10;
    public int hp;//体力
    public Slider sliderHp;
    private bool damaged = false;//ダメージを受けたらdamagedTimeの時間分だけtrueになる（ダメージを受けたアニメーションを再生する用）
    public float damagedTime = 0.2f;//ダメージを受けたアニメーションを再生する時間

    public float invincibleTime = 0.5f;  // 無敵時間（点滅時間）
    public float blinkInterval = 0.1f;  // 点滅の間隔
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;


    public enum PlayerState
    {
        Idle,       // 待機中
        Moving,     // 移動中
        Jumping,    // ジャンプ中
        Attacking,  // 攻撃中
        Dashing,    // ダッシュ中
        Stunned     // スタン中（例として追加）
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
        isGround = ground.IsGround();//設置判定
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
            attack.DisableAttack();//今は使ってない
        }
    }

    private void Flip()
    {
        if (!(isAttacking || dashing)) {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                //何もしない
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

    private void Dash()//ダッシュ用
    {
        if (Input.GetKey(KeyCode.C) && (dashTimeRecast == false))
        {
            StartCoroutine(DashC());//dashTimeRecastを一定時間trueにしてダッシュできなくするためだけのコルーチン
        }else if (dashing)
        {
            rb.velocity = transform.right * dashSpeed;
        }
    }
    IEnumerator DashC()//ダッシュ中のコルーチン
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
    private void Jump()//ジャンプ用
    {
        // ジャンプ開始
        if (isGround && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            jumpTimeCounter = maxHoldTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // ジャンプボタンを押し続けた場合の処理
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

        // ジャンプボタンを離したらジャンプ終了
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

    private bool InvincibleJudge()//ダメージを受ける状態ならfalseを返す
    {
        return !(isInvincible || dashing);//どれか1つでもtrueならfalseを返し、ダメージを受けない
    } 

    public void Damage(int damage)
    {
        if (InvincibleJudge())//ダメージを受ける状態か判断
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

    IEnumerator BlinkCoroutine(float duration)//ダメージを受けた時に点滅したり一定時間無敵にしたりする
    {
        Debug.Log("BlinkCoroutine");
        isInvincible = true;  // 一定時間無敵状態にする
        float elapsedTime = 0f;
        StartCoroutine(DamageAnim());
        while (elapsedTime < duration)
        {
            // スプライトの可視性を切り替える
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // 点滅の間隔だけ待つ
            yield return new WaitForSeconds(blinkInterval);

            // 経過時間を更新
            elapsedTime += blinkInterval;
        }

        // 点滅終了後にスプライトを表示する
        spriteRenderer.enabled = true;

        isInvincible = false;  // 無敵状態を解除
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
