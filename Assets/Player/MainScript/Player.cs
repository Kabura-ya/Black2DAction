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
    public GameManager gameManager;//ゲームオーバーやクリアなどを処理するGamemanagerについているスクリプトの情報を取得するための関数

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
    public Image sliderEnergyImage;//エナジーのスライダーの色を変える用
    private Color originalEnergySliderColor;
    //エナジー攻撃関係
    public float energyCost = 4;
    public GameObject energyBullet;
    public float shootPos = 1; //弾をプレイヤーからどの程度前で撃つか
    //ジャンプ関係
    public float jumpForce = 10f;  // ジャンプ力
    public float holdJumpMultiplier = 0.5f;  // ジャンプボタンを押し続けた場合の力
    public float maxHoldTime = 0.2f;  // ジャンプボタンを押し続ける最大時間
    private float jumpTimeCounter;//2弾ジャンプとかする時のためだがまだ使えていない
    private bool isJumping = false;
    public GroundCheck ground;//接地判定用のスクリプト
    private bool isGround;
    private float originagGravity;
    //攻撃関係
    public Attack attack;
    private bool isAttacking = false;
    private float countAttack = 0;//攻撃のリキャストまでの時間を記録する用の変数

    private Animator anim;//アニメーター
    private Vector2 inputDirection;//インプットシステムを使って実装しようとしただけでまだ使っていない
    private Rigidbody2D rb;//プレイヤーのリジッドボディを入れる変数、速度とか重力とかリジッドボディを使う必要のある操作のために変数を作ってある
    public int maxHp = 10;//体力の最大値
    public int hp;//体力
    public Slider sliderHp;//HPバー
    private bool damaged = false;//ダメージを受けたらdamagedTimeの時間分だけtrueになる（ダメージを受けたアニメーションを再生する用）
    public float damagedTime = 0.2f;//ダメージを受けたアニメーションを再生する時間
    private bool stop = false;//この変数がtrueの時はプレイヤーを停止させる（演出時など用）

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
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sliderHp.value = maxHp;
        sliderEnergy.value = 0;
        sliderEnergyImage = sliderEnergy.fillRect.GetComponent<Image>();
        originalEnergySliderColor = sliderEnergyImage.color;
        if (energyCost > energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor / 3;
        }//energyが技を放てない量ならスライダーの色を暗くする
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
        isGround = ground.IsGround();//設置判定
        if (isGround)
        {
            //Debug.Log("PlayerGround");
        }

    }

    public void StopPlayer()//演出などでプレイヤーの動作を停止させる用
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
    private void Attack()//近距離攻撃（攻撃用の子オブジェクトの関数で）
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

    private void Flip()//反転(ダッシュ中は反転しない)
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
        dashing = true;//ダッシュ中はtrueにする
        dashTimeRecast = true;//ダッシュのリキャスト時間が過ぎるまでtrueにする
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        //rb.gravityScale = originagGravity;
        dashing = false;
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }

    private void EnergyBullet()//エネルギー弾（前方に直進する弾）を撃つ
    {
        if (Input.GetKeyDown(KeyCode.S) && energyCost <= energy)
        {
            Instantiate(energyBullet, transform.position + shootPos * transform.right, transform.rotation);
            useEnergy(energyCost);
        }
    }

    private bool useEnergy(float useEnergy)//エナジー消費用
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

    private void recoverEnergy()//エナジー回復、現状一回のドレインで回復するエナジーの値は一定なので引数なしにしている
    {
        energy += getEnergy;
        sliderEnergy.value = (float)energy / maxEnergy;
        if (energyCost <= energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor;
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
    public void BodyEnter(Collider2D collision)
    {
        Debug.Log("Player_BodyEnter");
        if (dashing)//ダッシュ中か
        {
            Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//触れた相手にドレイン用インターフェースがあるか
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

    private bool InvincibleJudge()//ダメージを受ける状態ならtrueを返す、攻撃にtypeを設定した影響で現在は無意味
    {
        return !(isInvincible || dashing || stop);//どれか1つでもtrueならfalseを返し、ダメージを受けない
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int damage, Vector2 vector, int type)
    {
        Debug.Log("PlayerDamage");
        if (!(isInvincible || stop))
        {
            if (!dashing || (type > 0)){//ダッシュしていなかったり、typeが0でなかったらダメージを受ける
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
        gameManager.GameOver();
        Destroy(this.gameObject);
    }

}
