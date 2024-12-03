using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //�܂�������Ԓe�p
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public float speed = 10;
    public int attack = 3;
    public string damageTag = "Player";//�_���[�W��^���鑊��̃^�O
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
