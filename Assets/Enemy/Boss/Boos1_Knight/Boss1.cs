using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss1 : MonoBehaviour, IDamageable, IDrainable
{
    public int attack = 1;
    public int hp = 10;
    protected GameObject player;//プレイヤーの情報を使えるようにしておく
    protected Transform playerTrans;
    protected Rigidbody2D rigidbody2d;
    private bool dashing;
    public float dashDistance = 20;
    public float dashSpeed = 10;
    public float idleTime = 1;

    public float fallHight = 3;
    public float fallSpeed = 10;

    public float groundHight;

    private bool onGround = true;
    private bool enableHit = true;//これがtrueの時だけダメージを受けたり与える
    private bool moving = false;
    private int action = 1;

    public AttackEnemy sword;//剣攻撃用のクラス
    public SightEnemy swordSight;

    private Animator anim;//アニメーター

    public float invincibleTime = 0.2f;  // 無敵時間（点滅時間）
    public float blinkInterval = 0.1f;  // 点滅の間隔
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    public Material whiteFlashMaterial; // 白く点滅させるためのマテリアル
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();//自身のRigidbodyを変数に入れる
        player = GameObject.Find("Player");
        playerTrans = player.transform;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseAction();
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("OnTrigger");
        AnimSet();
    }

    private void AnimSet()
    {
        //Debug.Log("AnimSet");
        //Debug.Log(action);
        anim.SetInteger("action", action);
        anim.SetBool("onGround", onGround);
        if (rigidbody2d.velocity.x != 0 || rigidbody2d.velocity.y != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
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

        if (swordSight.IsPlayerinSight())
        {
            if (0.5 < Random.value)//プレイヤーが視界のコライダー内に居たら1/2の確率で剣で攻撃
            {
                action = 3;
                StartCoroutine(Sword());
                return;
            }
        }
        action = Random.Range(1, 3);
        if (action == 1)
        {
            StartCoroutine(Dash());
            return;
        }
        else if (action == 2)
        {
            StartCoroutine(Fall());
            return;
        }
        
        /*
        else if (action == 3)
        {
            StartCoroutine(Sword());
            return;
        }*/
        
    }

    private IEnumerator Dash()
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = transform.right * dashSpeed;
        dashing = true;
        yield return new WaitForSeconds(dashDistance / dashSpeed);
        rigidbody2d.velocity = new Vector2(0, 0);
        dashing = false;
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator Fall()
    {
        enableHit = false;
        yield return new WaitForSeconds(idleTime);
        onGround = false;
        transform.position = new Vector2(playerTrans.position.x, fallHight);
        enableHit = true;
        rigidbody2d.velocity = new Vector2(0, 0);
        moving = true;
        yield return new WaitForSeconds(idleTime);
        rigidbody2d.velocity = new Vector2(0, -1 * fallSpeed);
        moving = true;
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(idleTime);
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
    }

    private IEnumerator Sword()
    {
        FlipToPlayer();
        yield return new WaitForSeconds(idleTime);
        sword.EnableAttack();
        yield return new WaitForSeconds(idleTime);
        sword.DisableAttack();
        action = 0;
        yield return new WaitForSeconds(idleTime);
        ChooseAction();
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
    public void BodyStay(Collider2D collision)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyGroundCheck")
        {
            onGround = true;
            moving = false;
        }
    }

    public void Damage(int damage)
    {
        if (enableHit)
        {
            hp -= damage;

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
        isInvincible = false;
        // 点滅終了後にスプライトを表示する
        spriteRenderer.enabled = true;
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }

    public bool Drain()
    {
        return enableHit;//ダメージ判定とかを有効にしている間のみドレイン可能
    }
}