using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase1 : MonoBehaviour
{
    //敵の基本的な機能、継承させる前提で、継承先で接触判定関係の色々な関数をオーバーライドして使用する想定

    public bool enableHit = true;//これがtrueの時だけダメージを受けたり与える
    public int attack = 1;
    public int hp = 3;
    public float speed = 10;
    protected GameObject player;//プレイヤーの情報を使えるようにしておく
    protected Transform playerTrans;
    Player playerScript;//プレイヤーのスクリプトの関数を利用できるようにする
    protected Rigidbody2D rigidbody2d;

    public GameObject damageEffect;
    public float invincibleTime = 0.2f;  // 無敵時間（点滅時間）
    public float blinkInterval = 0.1f;  // 点滅の間隔
    private SpriteRenderer spriteRenderer;
    private Animator anim;//アニメーター
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();//自身のRigidbodyを変数に入れる
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();//プレイヤーのスクリプトに対して操作できるようにする
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    protected virtual void Update()
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

    public virtual void BodyStay(Collider2D collision)//Body部分の子オブジェクトのOnTriggerStayで呼ばれる
    {
        Debug.Log("EnemyBase1_BodyStay");
        var damageTarget = collision.gameObject.GetComponent<IDamageable>();
        if (damageTarget != null)
        {
            damageTarget.Damage(attack);
        }
        if (collision.gameObject.tag == "Player" && enableHit)
        {
            Debug.Log("EnemyOntrrigerEnter_Player");
            //var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }
        }
    }

    //ダメージを受ける部分、疑似的にデフォルト引数を再現するために3つ変数の数の違うDamage関数を用意している
    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }
    public void Damage(int value, Vector2 vector, int type)
    {
        Debug.Log("EnemyBase_Damage");
        if (enableHit)
        {
            hp -= value;
            Instantiate(damageEffect, transform.position, transform.rotation);

            rigidbody2d.velocity = vector;

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
    public void Death()
    {
        Destroy(this.gameObject);
    }
    public bool Drain()//Bodyの子オブジェクトから呼ばれる
    {
        return enableHit;//ダメージ判定とかを有効にしている間のみドレイン可能
    }

    public bool SuperDrain()//Bodyの子オブジェクトから呼ばれる
    {
        return Drain();
    }
}
