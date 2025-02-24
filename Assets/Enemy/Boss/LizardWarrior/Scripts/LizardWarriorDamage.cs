using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorDamage : MonoBehaviour, IDamageable, IDrainable
{
    [SerializeField] private LizardWarriorStatus lizardWarriorStatus = null;
    [SerializeField] GameObject damageEffect = null;
    private int hp = 0;
    private int stanCount = 0;
    private bool hadkstunned = false;

    void Awake()
    {
        hp = lizardWarriorStatus.MaxHp;
        stanCount = hp / 2;
        hadkstunned = false;
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        //死亡時とフェイント、生成時以外はダメージを受ける
        if (!lizardWarriorStatus.IsDead() && !lizardWarriorStatus.IsSpawn() && !lizardWarriorStatus.IsFeint())
        {
            hp -= value;
            stanCount -= value;
            Instantiate(damageEffect, this.transform.position, Quaternion.Euler(0f, 0f, 80f));
            if (hp <= 0)
            {
                Dead();
            }
            if(stanCount <= 0 && !hadkstunned)
            {
                Stan();
                hadkstunned = true;
            }
        }
    }

    void Stan()
    {
        lizardWarriorStatus.StanPlay();
    }
    void Dead()
    {
        lizardWarriorStatus.DeadPlay();
    }

    public bool Drain()
    {
        return true;
    }
    public bool SuperDrain()
    {
        return true;
    }
}
