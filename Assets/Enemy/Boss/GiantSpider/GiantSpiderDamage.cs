using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpiderDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    [SerializeField] GameObject damageEffect = null;
    private int hp = 0;

    void Awake()
    {
        hp = giantSpiderStatus.MaxHp;
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        //死亡時と生成時以外はダメージを受ける
        if (!giantSpiderStatus.IsDead() && !giantSpiderStatus.IsSpawn())
        {
            hp -= value;
            Instantiate(damageEffect, this.transform.position, Quaternion.Euler(0f, 0f, 80f));
            if(hp <= 0)
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        giantSpiderStatus.DeadPlay();
    }
}
