using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Base : MonoBehaviour, IDamageable, IDrainable
{
    public GameManager gameManager;//ゲームオーバーやクリアなどを処理するGamemanagerについているスクリプトの情報を取得するための関数

    public int thisBossNum = 5;//このボスの番号、ボスが倒されたらGameManager.Instance.GameCrearに
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

    public float startTime = 2;
    private bool onGround = true;
    bool enableHit = true;
    private bool superDashStunn = false;//これがtrueの時（主に一部の赤攻撃中）にスーパーダッシュでぶつかられるとスタンする。
    private bool stunn = false;//スタン中にtrue、ダメージを受けるがプレイヤーに触れてもダメージを与えない
    public float stunnTime = 3;//スタン時間
    private bool moving = false;
    int action = 0;

    public GameObject redDashEffect;//突進を開始した時の衝撃波のエフェクト

    public GameObject damageEffect;
    public GameObject defeatEffect;//倒したときのエフェクト
    public float invincibleTime = 0.1f;  //ダメージ受けた直後の点滅時間
    private SpriteRenderer spriteRenderer;
    bool dead = false;
    Coroutine actionCoroutine;//死亡時などにコルーチンを停止させるために、行動のコルーチンの引数を入れておく

    public AttackEnemy sword;//剣攻撃用のクラス

    public Slider sliderHp;//HPバー

    private Animator anim;//アニメーター
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        rigidbody2d = GetComponent<Rigidbody2D>();//自身のRigidbodyを変数に入れる
        sliderHp.value = maxHp;
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
        enableHit = true;
        action = 0;
        yield return new WaitForSeconds(0.1f);
        ChooseAction();
    }

    // Update is called once per frame
    void Update()
    {

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

    }

    private IEnumerator Dash()//突進攻撃、なぜか終わり際にスーパーダッシュでぶつかるとスタンしない上にダッシュし続けるバグがある
    {
        Debug.Log("DashRedBegin");
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        yield return new WaitForSeconds(0.8f);
        superDashStunn = true;
        Instantiate(redDashEffect, transform.position, transform.rotation);
        rigidbody2d.velocity = transform.right * dashSpeed;
        sword.EnableAttack();
        moving = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        rigidbody2d.velocity = new Vector2(0, 0);
        sword.DisableAttack();
        moving = false;
        superDashStunn = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        Debug.Log("DashRedEnd");
        ChooseAction();
    }

    public void BodyStay(Collider2D collision)//Body部分の子オブジェクトのOnTriggerStayで呼ばれる(接触ダメージとかで使えるかと思ったけど特に使っていない)
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

    }
    public void Damage(int value, Vector2 vector, int type)
    {
        Debug.Log("Damage(int value, Vector2 vector, int type)");
        if (enableHit)
        {
            hp -= value;
            if (hp < 0) hp = 0;
            Instantiate(damageEffect, transform.position, transform.rotation);
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
    }
    public void Death()//体力が0以下になった時に呼ばれ、消滅する
    {
        StartCoroutine(DeathC());
    }


    IEnumerator BlinkCoroutine(float duration)//ダメージを受けた時に一瞬点滅する
    {
        // 点滅の時間だけ待つ
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(duration);

        // 点滅終了後にスプライトを表示する
        spriteRenderer.enabled = true;
    }

    private void DisableAllAttack()//スタン時などに全ての攻撃用コライダーを無効化する
    {

    }

    private void Stunn()
    {
        if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }//現在の行動を中止させてからスタンさせる
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
        rigidbody2d.velocity = transform.right * -3;
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

    IEnumerator DeathC()
    {
        FlipToPlayer();
        DisableAllAttack();
        Instantiate(defeatEffect, transform.position, transform.rotation);
        GameManager.instance.GameClear(thisBossNum);
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
        yield return new WaitForSeconds(3);
        //Destroy(this.gameObject);
    }



    public bool Drain()//Bodyの子オブジェクトから呼ばれる
    {
        return enableHit;//ダメージ判定とかを有効にしている間のみドレイン可能
    }

    public bool SuperDrain()
    {
        if (superDashStunn)//一部の赤攻撃中などにスーパーダッシュでぶつかられるとスタンする
        {
            Debug.Log("SuperDashStunn");
            Stunn();
        }
        return enableHit;
    }


}
