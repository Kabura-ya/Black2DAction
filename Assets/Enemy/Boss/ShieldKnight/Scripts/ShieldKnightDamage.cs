using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightDamage : MonoBehaviour, IDamageable, IDrainable
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;
    [SerializeField] private ShieldKnightPattern shieldKnightPattern = null;
    [SerializeField] GameObject damageEffect = null;
    private int hp = 0;

    void Awake()
    {
        hp = shieldKnightStatus.MaxHp;
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        //ガード中なら
        if (shieldKnightStatus.IsGuard() ||
            shieldKnightStatus.IsPowerGuard())
        {
            shieldKnightPattern.CounterSuccess();
        }
        //それ以外でダメージ受ける状態
        else if (!shieldKnightStatus.IsDead() && 
                 !shieldKnightStatus.IsSpawn() && 
                 !shieldKnightStatus.IsCounter() &&
                 !shieldKnightStatus.IsPowerCounter())
        {
            hp -= value;
            Instantiate(damageEffect, this.transform.position, Quaternion.Euler(0f, 0f, 80f));
            if (hp <= 0)
            {
                Dead();
            }

            //歩いている時にダメージ受けたらガードに遷移
            if(shieldKnightStatus.IsWalk())
            {
                shieldKnightPattern.GuardActive();
            }
        }
    }

    void Stan()
    {
        shieldKnightStatus.StanPlay();
    }
    void Dead()
    {
        shieldKnightStatus.DeadPlay();
    }

    public bool Drain()
    {
        if (shieldKnightStatus.IsGuard() ||
            shieldKnightStatus.IsPowerGuard())
        {
            shieldKnightStatus.StanPlay();
        }
        return true;
    }
    public bool SuperDrain()
    {
        if (shieldKnightStatus.IsGuard() ||
            shieldKnightStatus.IsPowerGuard())
        {
            shieldKnightStatus.StanPlay();
        }
        return true;
    }
}
