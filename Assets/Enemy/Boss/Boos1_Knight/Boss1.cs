//using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


public class Boss1 : MonoBehaviour, IDamageable, IDrainable, ISuperDashStunn
{
    public GameManager gameManager;//ゲームオーバーやクリアなどを処理するGamemanagerについているスクリプトの情報を取得するための関数

    public int attack = 1;
    public int maxHp = 300;
    public int hp = 300;
    protected GameObject player;//プレイヤーの情報を使えるようにしておく
    protected Transform playerTrans;
    Player playerScript;//プレイヤーのスクリプトの関数を利用できるようにする
    protected Rigidbody2D rigidbody2d;
    public float dashDistance = 20;
    public float dashSpeed = 10;
    public float idleTime = 1;

    public float fallHight = 3;
    public float fallSpeed = 10;

    public float groundHight;

    private bool start = true;
    private float startTime = 1.3f;//なぜか最初のアニメーションよりも長い秒数にすると行こうのアニメーションの再生が正常にできない事がある
    private bool onGround = true;
    private bool enableHit = false;//これがtrueの時だけダメージを受けたり与える
    private bool superDashStunn = false;//これがtrueの時（主に一部の赤攻撃中）にスーパーダッシュでぶつかられるとスタンする。
    private bool stunn =false;//スタン中にtrue、ダメージを受けるがプレイヤーに触れてもダメージを与えない
    private bool moving = false;
    private int action = 1;
    private bool dead = false;
    private bool last = false;

    public AttackEnemy sword;//剣攻撃用のクラス
    public AttackEnemy swordFall;//落下攻撃の時の当たり判定
    public SightEnemy swordSight;

    public AttackEnemy redSword;//赤攻撃用

    public GameObject bullet;//遠距離攻撃で放つやつ
    public GameObject bulletRed;//赤遠距離攻撃で放つやつ

    private Animator anim;//アニメーター

    public GameObject damageEffect;
    public float invincibleTime = 0.2f;  // 無敵時間（点滅時間）
    public float blinkInterval = 0.1f;  // 点滅の間隔
    private SpriteRenderer spriteRenderer;
    public Material whiteFlashMaterial; // 白く点滅させるためのマテリアル

    public float stunnTime = 3;//スタン時間
    public int stunnMaxCount = 1;
    public int stunnCount = 0;//これまで何回スタンしたかを記録（体力がある程度減るごとにスタンするようにするため）

    Coroutine actionCoroutine;//死亡時などにコルーチンを停止させるために、行動のコルーチンの引数を入れておく
    float noActionCoroutineTime = 0;//actionCoroutineがバグで長時間nullになっていた際に、その時間を記録する

    public GameObject redDashEffect;//赤突進を開始した時の衝撃波のエフェクト
    public GameObject defeatEffect;//倒したときのエフェクト
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        rigidbody2d = GetComponent<Rigidbody2D>();//自身のRigidbodyを変数に入れる
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();//プレイヤーのスクリプトに対して操作できるようにする
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        actionCoroutine = StartCoroutine(StartC());
    }

    IEnumerator StartC()//最初の演出をする
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
        //なぜか赤突進の終わり際にスーパーダッシュでぶつかるとスタンしない上にダッシュし続けるバグがある(もう治ったのでコメントアウト)のでその対策
        if (actionCoroutine == null)//バグで長時間行動していない場合行動させる
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

    protected void FlipToPlayer()//Playerの方を向く
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
            if (0.7 < Random.value)//プレイヤーが視界のコライダー内に居たら3/10の確率で剣で攻撃
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

    private IEnumerator Dash()//通常突進攻撃、赤突進をかわりにするので多分もう使わない
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

    private IEnumerator DashRed()//赤突進攻撃、なぜか終わり際にスーパーダッシュでぶつかるとスタンしない上にダッシュし続けるバグがある
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

    private IEnumerator Fall()//落下攻撃
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
        //アニメーションの方でも、着地した際のアニメーションでコライダーを無効化している
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

    private IEnumerator Sword()//近距離攻撃
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        moving = true;
        //アニメーションの方で EnabeleAttack_Sword()を実行
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

    private IEnumerator LongRange()//遠距離攻撃
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        //このへんでアニメーションの方からLongRangeSpawn();が呼ばれて弾が出る
        yield return new WaitForSeconds(idleTime);
        action = 0;
        yield return new WaitForSeconds(0.1f);
        if (0.7 < Random.value)//通常遠距離攻撃後3/10の確率で赤遠距離攻撃をする
        {
            action = 4;
            actionCoroutine = StartCoroutine(LongRangeRed());
        }
        else
        {
            ChooseAction();
        }
    }

    private void LongRangeSpawn()//弾を発射するための関数、アニメーションの方で呼ぶ
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    private IEnumerator LongRangeRed()//赤の遠距離攻撃
    {
        FlipToPlayer();
        yield return new WaitForSeconds(2.5f);
        //このへんでアニメーションの方からLongRangeSpawn();が呼ばれて弾が出る
        yield return new WaitForSeconds(idleTime);
        action = 0;
        yield return new WaitForSeconds(0.1f);
        ChooseAction();
    }

    private void LongRangeRedSpawn()//弾を発射するための関数、アニメーションの方で呼ぶ
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
    public void BodyStay(Collider2D collision)//Body部分の子オブジェクトのOnTriggerStayで呼ばれる
    {
        //Debug.Log("OnTrigger");
        
        if (collision.gameObject.tag == "Player" && enableHit && !stunn)
        {
            //Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                //damageTarget.Damage(attack);プレイヤーへの接触ダメージ
            }
        }
        
        if (superDashStunn && collision.gameObject.tag == "Player" && playerScript.StateSuperDashing())//一部の赤攻撃中などにスーパーダッシュでぶつかられるとスタンする
        {
            //Stunn();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//接地判定（敵が地面に触れたかを判定するためにわざわざEnemyGroundCheckタグを床にだけつけている）
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
            }else if (hp <= maxHp / 2 && stunnCount == 0)//HPが半分になったらスタン(スタンのテスト用なので後で消すかも)
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

    IEnumerator BlinkCoroutine(float duration)//ダメージを受けた時に一瞬点滅する
    {
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
    }

    private void DisableAllAttack()//スタン時などに全ての攻撃用コライダーを無効化する
    {
        sword.DisableAttack();
        swordFall.DisableAttack();
        redSword.DisableAttack();

    }

    private void NextAction(IEnumerator nextAction)//まだ使っていない後から使いたい
    {
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }//現在の行動を中止させてからスタンさせる
        actionCoroutine = null;
        actionCoroutine = StartCoroutine(nextAction);
    }
    public void SuperDashStunn()
    {
        if (superDashStunn)//一部の赤攻撃中などにスーパーダッシュでぶつかられるとスタンする
        {
            Debug.Log("SuperDashStunn");
            Stunn();
        }
    }
    private void Stunn()
    {
        if (actionCoroutine != null){ StopCoroutine(actionCoroutine);}//現在の行動を中止させてからスタンさせる
        actionCoroutine = null;
        actionCoroutine = StartCoroutine(StunnCoroutine());
    }

    IEnumerator StunnCoroutine()
    {
        Debug.Log("StunnCoroutine");

        //スタン時に様々な要素をリセット
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
    public void Death()//体力が0以下になった時に呼ばれ、消滅する
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


    public bool Drain()//Bodyの子オブジェクトから呼ばれる
    {
        return  enableHit;//ダメージ判定とかを有効にしている間のみドレイン可能
    }
    public bool SuperDrain()
    {
        return Drain();
    }
}