using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : EnemyBase1//����1�AEnemyBase1���p��
{
    public override void BodyStay(Collider2D collision)//Body�����̎q�I�u�W�F�N�g��OnTriggerStay�ŌĂ΂��
    {
        Debug.Log("EnemyBase1_Override_SimpleEnemy_BodyStay");
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
}
