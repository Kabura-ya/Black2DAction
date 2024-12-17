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
    private float dashY;//（今はつかっていない）ダッシュするときに高さが変わらないように
    private bool dashing = false;//ダッシュ（とスーパーダッシュ）中にtrueになる
    public float dashRecastTime = 0.5f;//ダッシュをまたできるまでの時間
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    public GameObject dashDrainEffect;//吸収できた時に出るエフェクト

    //スーパーダッシュ（チャージダッシュ）関係
    public float superDashchargeTime;//チャージダッシュ用のチャージ時間
    private float superDashTimeCount = 0;//チャージ時間のカウント
    public float superDashSpeed = 20;
    public float superDashDistance = 60;
    private float superDashY;//（今はつかっていない）チャージ中とスーパーダッシュするときに高さが変わらないように
    private bool superDashing = false;//スーパーダッシュ中にtrueになる
    public float superDashRecastTime = 0.5f;//ダッシュをまたできるまでの時間
    private bool superDashTimeRecast = false;
    private bool superDashGroundRecast = false;
    public GameObject superDashdrainEffect;//スーパーダッシュで吸収できた時に出るエフェクト
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
    private bool beginAttack = false;//攻撃のアニメーションを始めるためだけのもの、攻撃開始時の一瞬だけtrueになる
    private float countAttack = 0;//攻撃のリキャストまでの時間を記録する用の変数
    private bool notFlipAttack = false;//攻撃中は反転しない用

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

    
    public enum PlayerState//プレイヤーの状態
    {
        Stop,       //停止中（演出中などで動けない）        
        Idle,       // 通常状態、何していない待機中
        Moving,     // 移動中
        Jumping,    // ジャンプ中
        NormalAttacking,  // 通常攻撃中
        EnergyBullet,//エナジー弾を撃っている硬直
        Dashing,    // ダッシュ中
        SuperDashing,//スーパーダッシュ中
        SuperDashCharging,//スーパーダッシュをチャージ中
        Stunned     // スタン中（例として追加）
    }

    private PlayerState playerState = PlayerState.Idle;
    void Start()
    {
        playerState = PlayerState.Idle;//最初は待機状態に
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sliderHp.value = maxHp;
        sliderEnergy.value = 0;
        sliderEnergyImage = sliderEnergy.fillRect.GetComponent<Image>();
        originalEnergySliderColor = sliderEnergyImage.color;//エナジーのスライダーの色を記録
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
        originagGravity = rb.gravityScale;//最初の重力の値を記録
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

    private bool JudgeNormalState()//攻撃やダッシュ中は他の動作ができないようにしたいので、他の動作をしてもいいIdle,Moving,Jumping中のみtrueを返す関数
    {
        return (playerState == PlayerState.Idle || playerState == PlayerState.Moving || playerState == PlayerState.Jumping);
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//設地判定
        if (isGround){/*Debug.Log("PlayerGround");*/}
        if (!isGround && (playerState == PlayerState.Idle || playerState == PlayerState.Moving))
        { 
            playerState = PlayerState.Jumping;//地面から離れたら落下状態に
        }
        if (isGround && playerState == PlayerState.Jumping)
        {
            playerState = PlayerState.Idle;//ジャンプ中や落下中に地面に着いたらIdle状態に
        }
    }

    public void StopPlayer()//演出などでプレイヤーを操作できなくさせる用
    {
        playerState = PlayerState.Stop;
    }
    public void StopInterruptPlayer()//StopPlayer()で止めたのを戻す用
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
    private void Attack()//近距離攻撃（攻撃用の子オブジェクトの関数で）
    {
        if (!(JudgeNormalState() || playerState == PlayerState.NormalAttacking))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.X) && (countAttack <= 0))
        {
            countAttack = attack.GetComponent<Attack>().recastTime;
            //EnableAttack()をアニメーションの方で呼ぶ
            beginAttack = true;//アニメーション遷移用に一瞬だけtrueにする
            playerState = PlayerState.NormalAttacking;//状態を攻撃中に
            Debug.Log("Attack");
        }
        else
        {
            countAttack -= Time.deltaTime;//攻撃のカウントから経過時間を引いていく
            beginAttack = false;//アニメーション遷移用に一瞬だけtrueにしたらすぐfalseにする
            if (countAttack <= 0)
            {
                countAttack = 0;
                playerState = PlayerState.Idle;
                attack.DisableAttack();//攻撃コライダーを無効化、本来はアニメーションでDisableAttack()を呼んで攻撃コライダーを無効化するが、アニメーションが中断された時などのためにおいてある
            }
        }

    }

    private void EnableAttack()//通常攻撃用のコライダーを有効化する、通常攻撃時に、アニメーションの方から呼ぶ
    {
        attack.EnableAttack();
    }
    private void DisableAttack()//通常攻撃用のコライダーを無効化する、通常攻撃時終了に、アニメーションの方から呼ぶ
    {
        attack.DisableAttack();//今は使ってない
    }

    private void Flip()//反転(ダッシュ中や攻撃中は反転しない)
    {
        if (JudgeFlip()) {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
            {
                //左右が同時に押されていたら何もしない
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

     private bool JudgeFlip()//振り向ける状態の時にはtrueを返す。今はJudgeNormalState()とスーパーダッシュのチャージ中は振り向ける
    {
        return JudgeNormalState() || (playerState == PlayerState.SuperDashCharging);
    }

    private void Move()//左右移動（ジャンプ中でも移動できる）
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

    private void Dash()//ダッシュ用
    {
        if (Input.GetKeyDown(KeyCode.C) && (dashTimeRecast == false))
        {
            StartCoroutine(DashCoroutine());//状態を一定時間Dashingにして、dashTimeRecastを一定時間trueにしてダッシュできなくしたりするコルーチン
        }else if (playerState == PlayerState.Dashing)
        {
            rb.velocity = transform.right * dashSpeed;//速度を設定
            Debug.Log("PlayerState.Dashing");
        }
    }
    IEnumerator DashCoroutine()//ダッシュ中のコルーチン
    {
        rb.velocity = transform.right * dashSpeed;
        //rb.gravityScale = 0;
        playerState = PlayerState.Dashing;//ダッシュ中は状態をDashingに
        dashTimeRecast = true;//ダッシュのリキャスト時間が過ぎるまでtrueにする
        yield return new WaitForSeconds(dashDistance / dashSpeed /*ダッシュ中の時間*/);
        //rb.gravityScale = originagGravity;
        playerState = PlayerState.Idle;
        rb.velocity = Vector2.zero;//ダッシュ直後に速度を0に
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }
    /*
    private void SuperDash()//チャージダッシュ用
    {
        if (Input.GetKey(KeyCode.D) && (dashTimeRecast == false))
        {
            StartCoroutine(SuperDashC());//dashTimeRecastを一定時間trueにしてダッシュできなくするためだけのコルーチン
        }
        else if (superDashing)
        {
            rb.velocity = transform.right * dashSpeed;
        }
    }
    IEnumerator SuperDashC()//チャージダッシュ中のコルーチン
    {
        rb.velocity = transform.right * dashSpeed;
        //rb.gravityScale = 0;
        playerState = PlayerState.SuperDashing;//ダッシュ中はtrueにする
        dashTimeRecast = true;//ダッシュのリキャスト時間が過ぎるまでtrueにする
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        //rb.gravityScale = originagGravity;
        playerState = PlayerState.Idle;
        yield return new WaitForSeconds(dashRecastTime);
        dashTimeRecast = false;
    }
    */

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
            playerState = PlayerState.Jumping;
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
        if (playerState == PlayerState.Dashing)//ダッシュ中か
        {
            Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//触れた相手にドレイン用インターフェースがあるか
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
    private bool JudgeInvincible()//絶対にダメージを受けない状態ならtrueを返す
    {
        return isInvincible || playerState == PlayerState.Stop;//どれか1つでもtrueならfalseを返し、ダメージを受けない
    }
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int damage, Vector2 vector, int type)
    {
        Debug.Log("PlayerDamage");
        if (!JudgeInvincible())
        {
            if (playerState == PlayerState.Dashing)//通常ダッシュ中
            {
                if (type >= 1)
                {
                    GetDamage(damage);
                }
            }else if (playerState == PlayerState.SuperDashing)//スーパーダッシュ中
            {
                if (type >= 2)
                {
                    GetDamage(damage);
                }
            }
            else{//それ以外の場合はダメージを受ける
                GetDamage(damage);
            }
            /*
            if (!dashing || (type >= 1)){//ダッシュしていなかったり、typeが0でなかったらダメージを受ける
                Debug.Log("PlayerDamage_Get");
                hp -= damage;//Hpを減らす
                sliderHp.value = (float)hp / maxHp;//Hpのスライダーの更新
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
        if (vector != Vector2.zero) { rb.velocity = vector; }//ノックバック 
    }
    private void GetDamage(int damage)//実際にダメージを受けてHPを減らしたり無敵時間とかの処理
    {
        hp -= damage;//Hpを減らす
        sliderHp.value = (float)hp / maxHp;//Hpのスライダーの更新
        if (hp <= 0)
        {
            Death();
        }
        else
        {
            StartCoroutine(BlinkCoroutine(invincibleTime));
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
        gameManager.GameOver();
        Destroy(this.gameObject);
    }

}
