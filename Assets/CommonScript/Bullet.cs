using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //まっすぐ飛ぶ弾用
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed = 10;
    public int attack = 3;
    public string damageTag = "Player";//ダメージを与える相手のタグ
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = speed * transform.right;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet_OnTrigger");
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }
        }
    }
}
