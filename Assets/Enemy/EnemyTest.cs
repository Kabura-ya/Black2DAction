using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour, IDamageable
{
    public int attack = 1;
    public int hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemyest_OnTrigger");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Enemyest_OntrrigerEnter_Player");
            var damageTarget = collision.gameObject.GetComponent<IDamageable>();
            if (damageTarget != null)
            {
                damageTarget.Damage(attack);
            }else{
                Debug.Log("NULL!!_Enemyest_OnTrigger");
            }
        }
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        hp -= value;
        if (hp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
