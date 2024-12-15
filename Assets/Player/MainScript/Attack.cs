using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    public int damage = 1;
    public float knockBackSpeed = 10;
    public float recastTime = 0.5f;
    public float enableTime = 0.4f;//攻撃コライダーを有効化する時間
    public BoxCollider2D col;
    Collider2D[] results = new Collider2D[10];  // 最大10個のコライダーを検出
    
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
                        damageTarget.Damage(damage, transform.right * knockBackSpeed, 0);
                    }
                }
            }
        }        
    }
    */

    public void EnableAttack()
    {
        StartCoroutine(EnableCollider());
    }

    IEnumerator EnableCollider()
    {
        // コライダーを有効化
        col.enabled = true;
        yield return new WaitForSeconds(enableTime); ;
        col.enabled = false;
    }

    public void DisableAttack()
    {
        col.enabled = false;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(damage, transform.right * knockBackSpeed, 0);
            }
        }
    }
    

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, col.size);
    }*/
}
