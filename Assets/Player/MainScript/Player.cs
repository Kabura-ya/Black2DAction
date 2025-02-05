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
    public GameManager gameManager;//ゲームオーバーやクリアなどを処理するGamemanagerについているスクリプトの情報を取得するための関数

    public enum PlayerState//プレイヤーの状態
    {
        Stop,       //停止中（演出中などで動けない）        
        Idle,       // 通常状態、何していない待機中
        Moving,     // 移動中
        Jumping,    // ジャンプ中
        NormalAttacking,  // 通常攻撃中
        EnergyBullet,//エナジー弾を撃っている硬直
        Dashing,    // ダッシュ中
        SuperDashCharging,//スーパーダッシュをチャージ中
        SuperDashCharged,//スーパーダッシュをチャージ完了
        SuperDashing,//スーパーダッシュ中
        Stunned     // スタン中、ダメージを受けた直後一瞬操作できない
    }

    PlayerState playerState = PlayerState.Idle;
    private PlayerState previousPlayerState = PlayerState.Idle;//1フレーム前のplayerStateを記録し、アニメ‐ション遷移に使うだけ

    public bool printLog = false;//これがtrueの時に色々なLogを出力する
    public float speed = 10;
    //ダッシュ関係
    private bool beginDash = false;
    public float dashSpeed = 20;
    public float dashDistance = 50;
    private float dashY;//（今はつかっていない）ダッシュするときに高さが変わらないように
    public float dashRecastTime = 0.3f;//ダッシュをまたできるまでの時間
    private bool dashTimeRecast = false;
    private bool dashGroundRecast = false;
    public GameObject dashDrainEffect;//吸収できた時に出るエフェクト

    //スーパーダッシュ（チャージダッシュ）関係
    public float superDashChargeTime = 0.5f;//チャージダッシュ用のチャージ時間
    private float superDashChargeTimeCount = 0;//チャージ時間のカウント
    public float superDashSpeed = 20;
    public float superDashDistance = 60;
    private float superDashY;//（今はつかっていない）チャージ中とスーパーダッシュするときに高さが変わらないように
    public float superDashRecastTime = 0.5f;//ダッシュをまたできるまでの時間
    private bool superDashGroundRecast = false;
    public GameObject superDashDrainEffect;//スーパーダッシュで吸収できた時に出るエフェクト
    //エナジー関係
    public float maxEnergy = 10;
    public float energy;
    public float getEnergy = 1;//通常ダッシュで取得するエナジー
    public float getSuperEnergy = 2;//スーパーダッシュで取得するエナジー
    public Slider sliderEnergy;
    public Image sliderEnergyImage;//エナジーのスライダーの色を変える用
    private Color originalEnergySliderColor;
    //エナジー攻撃関係
    public float energyCost = 4;//1回でのエナジー消費量
    public GameObject energyBullet;
    public float energyBulletRecastTime = 0.1f;
    public float shootPos = 1; //弾をプレイヤーからどの程度前に出現させて撃つか

    //ジャンプ関係
    public float jumpForce = 100f;  // ジャンプ力
    public float holdJumpMultiplier = 0.5f;  // ジャンプボタンを押し続けた場合の力
    public float maxHoldTime = 0.5f;  // ジャンプボタンを押し続ける最大時間
    private float jumpTimeCounter;//2弾ジャンプとかする時のためだがまだ使えていない
    private bool isJumping = false;
    public GroundCheck ground;//接地判定用のスクリプト
    private bool isGround;
    private float originagGravity = 4;
    //攻撃関係
    public Attack attack;
    private bool beginAttack = false;//攻撃のアニメーションを始めるためだけのもの、攻撃開始時の一瞬だけtrueになる
    private float countAttack = 0;//攻撃のリキャストまでの時間を記録する用の変数

    private Animator anim;//アニメーター
    private Vector2 inputDirection;//インプットシステムを使って実装しようとしただけでまだ使っていない
    private Rigidbody2D rb;//プレイヤーのリジッドボディを入れる変数、速度とか重力とかリジッドボディを使う必要のある操作のために変数を作ってある
    public int maxHp = 10;//体力の最大値
    public int hp;//体力
    public Slider sliderHp;//HPバー
    public float damagedTime = 0.1f;//ダメージを受けた直後で操作出来ない時間

    public float invincibleTime = 0.5f;  // 無敵時間（点滅時間）
    public float blinkInterval = 0.1f;  // 点滅の間隔
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    Coroutine actionCoroutine;//スタン時などにコルーチンを停止させるために、行動のコルーチンの引数を入れておく


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
        AnimSet();//アニメーション用なので上の色々な関数の下である必要がある
        PrintPlayerState();

        previousPlayerState = playerState;//Updateの最後に置く
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

    private bool JudgeNormalState()//攻撃やダッシュ中は他の動作ができないようにしたいので、他の動作をしてもいいIdle,Moving,Jumping中のみtrueを返す関数
    {
        return (playerState == PlayerState.Idle || playerState == PlayerState.Moving || playerState == PlayerState.Jumping);
    }

    private void FixedUpdate()
    {
        isGround = ground.IsGround();//設地判定
        if(printLog)//if (isGround){Debug.Log("PlayerGround");}
        
        if (!isGround && JudgeNormalState())
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
        anim.SetBool("damage", playerState == PlayerState.Stunned);
        anim.SetBool("beginDash", beginDash);
        anim.SetBool("dashing", playerState == PlayerState.Dashing);
        anim.SetBool("charging", playerState == PlayerState.SuperDashCharging);
        anim.SetBool("charged", playerState == PlayerState.SuperDashCharged);
        anim.SetBool("superDashing", playerState == PlayerState.SuperDashing);
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
            //Debug.Log("Attack");
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
                transform.rotation = Quaternion.Euler(0, 0, 0);//右を向く
            } else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);//左を向く
            }
        }
    }

     private bool JudgeFlip()//振り向ける状態の時にはtrueを返す。今はJudgeNormalState()とスーパーダッシュのチャージ中は振り向ける
    {
        return JudgeNormalState() || (playerState == PlayerState.SuperDashCharging);
    }

    private void Move()//左右移動（ジャンプ中、攻撃中でも移動できる）
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

    private bool JudgeMovable()//移動可能な状態ならtrueを返す
    {
        return playerState == PlayerState.Idle ||playerState == PlayerState.Moving || playerState == PlayerState.Jumping || playerState == PlayerState.NormalAttacking;
    }

    private void Jump()//ジャンプ用
    {

        // ジャンプボタンを離したり、ダッシュとかチャージとかしたらジャンプ終了
        if (Input.GetKeyUp(KeyCode.Z) || playerState == PlayerState.Dashing || playerState == PlayerState.SuperDashCharging)
        {
            isJumping = false;
            jumpTimeCounter = 0;
        }

        if (!(JudgeNormalState() || playerState == PlayerState.Jumping))
        {
            return;
        }
        // ジャンプ開始
        if (isGround && Input.GetKeyDown(KeyCode.Z))
        {
            isJumping = true;
            playerState = PlayerState.Jumping;
            jumpTimeCounter = maxHoldTime;
            //rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // ジャンプボタンを押し続けた場合の処理
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

    private void Dash()//ダッシュ用
    {
        if (!(JudgeNormalState() || playerState == PlayerState.Dashing))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C) && (dashTimeRecast == false))
        {
            beginDash = true;//アニメーション遷移用に一瞬だけtrueにする
            StartCoroutine(DashRecastCoroutine()); //ダッシュのリキャスト部分だけをやる、dashTimeRecastを一定時間trueにしてダッシュできなくしたりするコルーチン
            actionCoroutine = StartCoroutine(DashCoroutine());//状態を一定時間Dashingにする
        }else if (playerState == PlayerState.Dashing)
        {
            beginDash = false;
            rb.velocity = transform.right * dashSpeed;//速度を設定
            rb.gravityScale = 0;
            if(printLog) Debug.Log("PlayerState.Dashing");
        }
    }
    IEnumerator DashCoroutine()//ダッシュ中のコルーチン
    {
        rb.velocity = transform.right * dashSpeed;
        playerState = PlayerState.Dashing;//ダッシュ中は状態をDashingに
        yield return new WaitForSeconds(dashDistance / dashSpeed /*ダッシュ中の時間*/);
        playerState = PlayerState.Idle;
        rb.velocity = Vector2.zero;//ダッシュ直後に速度を0に
    }
    IEnumerator DashRecastCoroutine()//ダッシュのリキャスト時間が過ぎるまでtrueにするだけ
    {
        dashTimeRecast = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed /*ダッシュ中の時間*/ + dashRecastTime);
        dashTimeRecast = false;
    }

    private void SuperDashCharging()
    {
        if (playerState != PlayerState.SuperDashCharging) { superDashChargeTimeCount = 0; }
        if (!(JudgeNormalState() || playerState == PlayerState.SuperDashCharging))
        {
            return;
        }

        if (JudgeNormalState() && Input.GetKey(KeyCode.D))//チャージ開始の処理
        {
            playerState = PlayerState.SuperDashCharging;
        }
        if (playerState == PlayerState.SuperDashCharging)//チャージ中の処理
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = Vector2.zero;//チャージ中はその場に停止させる
                rb.gravityScale = 0;
                superDashChargeTimeCount += Time.deltaTime;
                if (superDashChargeTimeCount >= superDashChargeTime)
                {
                    playerState = PlayerState.SuperDashCharged;
                }
            }
            else
            {//チャージボタンを押していなければ
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
            StartCoroutine(SuperDashRecastCoroutine()); //ダッシュのリキャスト部分だけをやる、dashTimeRecastを一定時間trueにしてダッシュできなくしたりするコルーチン
            actionCoroutine = StartCoroutine(SuperDashCoroutine());
        }
    }
    
    private void SuperDash()//スーパーダッシュ発動中に速度を一定にするだけ、playerStateをSuperDashingにする処理はSuperDashCharge()の方で行う
    {
        if (playerState == PlayerState.SuperDashing)
        {
            rb.velocity = transform.right * superDashSpeed;
            rb.gravityScale = 0;
        }
    }
    IEnumerator SuperDashCoroutine()//チャージダッシュ中のコルーチン
    {
        playerState = PlayerState.SuperDashing;
        rb.velocity = transform.right * superDashSpeed;
        playerState = PlayerState.SuperDashing;
        yield return new WaitForSeconds(superDashDistance / superDashSpeed);
        playerState = PlayerState.Idle;
    }
    IEnumerator SuperDashRecastCoroutine()//ダッシュのリキャスト時間が過ぎるまでtrueにするだけ
    {
        dashTimeRecast = true;
        yield return new WaitForSeconds(superDashDistance / superDashSpeed /*ダッシュ中の時間*/ + dashRecastTime);
        dashTimeRecast = false;
    }

    private void EnergyBullet()//エネルギー弾（前方に直進する弾）を撃つ
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
            rb.velocity = Vector2.zero;//エナジー弾を放つと一瞬停止
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

    private void recoverEnergy(float value)//エナジー回復
    {
        energy += value;
        if (energy > maxEnergy) { energy = maxEnergy; }
        sliderEnergy.value = (float)energy / maxEnergy;
        if (energyCost <= energy)
        {
            sliderEnergyImage.color = originalEnergySliderColor;
        }
    }

    public void BodyEnter(Collider2D collision)//体の当たり判定用のコライダーにつけたスクリプトから呼ばれる
    {
        if(printLog) Debug.Log("Player_BodyEnter");
        if (playerState == PlayerState.Dashing)
        {
            if (printLog) Debug.Log("DashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//触れた相手にドレイン用インターフェースがあるか
            if (drainTarget != null && drainTarget.Drain())//ドレイン可能時
            {
                Instantiate(dashDrainEffect, transform.position, transform.rotation);
                if (printLog) Debug.Log("DashDrainSucceed");
                recoverEnergy(getEnergy);
            }
        }
        if (playerState == PlayerState.SuperDashing)
        {
            if (printLog) Debug.Log("SuperDashDrain");
            var drainTarget = collision.gameObject.GetComponent<IDrainable>();//触れた相手にドレイン用インターフェースがあるか
            if (drainTarget != null && (drainTarget.Drain() || drainTarget.SuperDrain()))//ドレイン可能時。通常のDrain()がTrueならばSuperDrain()もTrueなはずだが、ミスがあった時のために
            {
                Instantiate(superDashDrainEffect, transform.position, transform.rotation);
                if (printLog) Debug.Log("SuperDashDrainSucceed");
                recoverEnergy(getSuperEnergy);
            }

            //スタンさせる用の処理
            var stunnTarget = collision.gameObject.GetComponent<ISuperDashStunn>();
            if (stunnTarget != null)//スタン可能時
            {
                if (printLog) Debug.Log("SuperDasStunnSucceed");
                stunnTarget.SuperDashStunn();
            }
        }

        if (printLog) Debug.Log("OntrrigerEnter_Player");
    }
    private bool JudgeInvincible()//絶対にダメージを受けない状態ならtrueを返す
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
        if (vector != Vector2.zero) { rb.velocity = vector; }//ノックバック 
    }

    private bool JudgeGetDamageType(int type)//ダメージの種類とプレイヤーのダッシュなどの状態から、ダメージを受ける状態ならtrueを返す
    {
        return !((playerState == PlayerState.Dashing && type == 0) || (playerState == PlayerState.SuperDashing && type <= 1));
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
            StartCoroutine(StuuneTime());
            StartCoroutine(BlinkCoroutine(invincibleTime));
        }
    }

    public bool StateSuperDashing()//スーパーダッシュ中だったらtrueを返すだけ、敵にスーパーダッシュでぶつかったらスタンとかに使う
    {
        return playerState == PlayerState.SuperDashing;
    }
    IEnumerator BlinkCoroutine(float duration)//ダメージを受けた時に点滅したり一定時間無敵にしたりする
    {
        if (printLog) Debug.Log("BlinkCoroutine");
        isInvincible = true;  // 一定時間無敵状態にする
        float elapsedTime = 0f;
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

    IEnumerator StuuneTime()//一定時間スタン状態に
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
