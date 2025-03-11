using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightDamage : MonoBehaviour, IDamageable, IDrainable
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;
    [SerializeField] private ShieldKnightPattern shieldKnightPattern = null;
    [SerializeField] private ShieldKnightEffect shieldKnightEffect = null;
    [SerializeField] GameObject damageEffect = null;
    private int hp = 0;
    private int guardCount = 0;

    void Awake()
    {
        hp = shieldKnightStatus.MaxHp;
        guardCount = shieldKnightStatus.GuardCount;
    }

    public void Damage(int value) { Damage(value, Vector2.zero); }
    public void Damage(int value, Vector2 vector) { Damage(value, vector, 0); }

    public void Damage(int value, Vector2 vector, int type)
    {
        //ガード中なら
        if (shieldKnightStatus.IsGuard())
        {
            shieldKnightPattern.CounterSuccess();
            shieldKnightEffect.CounterSuccess();
        }
        else if (shieldKnightStatus.IsPowerGuard())
        {
            shieldKnightPattern.PowerCounterSuccess();
            shieldKnightEffect.PowerCounterSuccess();
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

            //直立か歩いている時にダメージ受けたらガードに遷移
            if(shieldKnightStatus.IsStand() || shieldKnightStatus.IsWalk())
            {
                guardCount--;
                if (guardCount <= 0)
                {
                    guardCount = shieldKnightStatus.GuardCount;
                    if (hp > shieldKnightStatus.MaxHp / 2)
                    {
                        shieldKnightPattern.GuardActive();
                    }
                    else
                    {
                        shieldKnightPattern.PowerGuardActive();
                    }
                }
            }
        }
    }

    void Stan()
    {
        shieldKnightPattern.GuardBreak();
    }
    void Dead()
    {
        shieldKnightStatus.DeadPlay();
    }

    public bool Drain()
    {
        if (shieldKnightStatus.IsGuard())
        {
            Stan();
            shieldKnightEffect.BrokenGuard();
            return true;
        }
        else if(shieldKnightStatus.IsPowerGuard())
        {
            shieldKnightPattern.PowerCounterSuccess();
            shieldKnightEffect.PowerCounterSuccess();
            return false;
        }
        return true;
    }
    public bool SuperDrain()
    {
        if (shieldKnightStatus.IsGuard())
        {
            Stan();
            shieldKnightEffect.BrokenGuard();
        }
        else if (shieldKnightStatus.IsPowerGuard())
        {
            Stan();
            shieldKnightEffect.BrokenPowerGuard();
        }
        return true;
    }
}
