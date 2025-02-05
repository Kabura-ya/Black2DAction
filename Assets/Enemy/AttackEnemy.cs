using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;
    public float enableTime = 2f;//攻撃コライダーを有効化する時間
    public Collider2D col;
    public Vector2 vector = new Vector2(10,5);
    public int damageType = 0;
    Coroutine disableTimeCoroutine;
    /*
    private void Update()
    {
        if (col.enabled)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;  // トリガーコライダーも検出する

            int numColliders = col.OverlapCollider(filter, results);

            for (int i = 0; i < numColliders; i++)
            {
                Debug.Log(results[i].gameObject.name + " に触れています");
                if (results[i].gameObject.tag == "Enemy")
                {
                    var damageTarget = results[i].gameObject.GetComponent<IDamageable>();
                    if (damageTarget != null)
                    {
                        damageTarget.Damage(damage);
                    }
                }
            }
        }        
    }
    */

    public void EnableAttack()
    {
        col.enabled = true;
        disableTimeCoroutine = StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        // コライダーを有効化
        yield return new WaitForSeconds(enableTime);
        col.enabled = false;//DisableAttack()でもコライダーを無効化できるが、バグなどでDisableAttackが発動しない時のために一応時間経過でもkライダー無効化できるようにしている
    }

    public void DisableAttack()
    {
        if (disableTimeCoroutine != null) { StopCoroutine(disableTimeCoroutine); }//時間経過で自動的にコライダーを無効化するコルーチンを中止
        disableTimeCoroutine = null;
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(damage, transform.right * vector.x + transform.up * vector.y, damageType);
            }
        }
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.size);
    }
    */
}
