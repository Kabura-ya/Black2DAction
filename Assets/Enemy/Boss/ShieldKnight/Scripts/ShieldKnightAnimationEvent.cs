using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightAnimationEvent : MonoBehaviour
{
    [SerializeField] private ShieldKnightAttack shieldKnightAttack = null;
    [SerializeField] private ShieldKnightEffect shieldKnightEffect = null;
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void SlashOn()
    {
        shieldKnightAttack.SlashOn();
    }
    void PowerSlashOn()
    {
        shieldKnightAttack.PowerSlashOn();
    }

    void SparkleOn()
    {
        shieldKnightEffect.SparkleOn();
    }
    void SparkleOff()
    {
        shieldKnightEffect.SparkleOff();
    }
    void GenerateSurroundSpear()
    {
        shieldKnightAttack.GenerateSurroundSpear();
    }
    void GenerateUpperSpear()
    {
        shieldKnightAttack.GenerateUpperSpear();
    }
    void GenerateSmashSpear()
    {
        shieldKnightAttack.GenerateSmashSpear();
    }

    void CounterOn()
    {
        shieldKnightAttack.CounterOn();
    }
    void PowerCounterOn()
    {
        shieldKnightAttack.PowerCounterOn();
    }

    void BrakeOn()
    {
        shieldKnightEffect.BrakeOn();
    }
    void BrakeOff()
    {
        shieldKnightEffect.BrakeOff();
    }

    void AllClear()
    {
        anim.ResetTrigger("slash");
        anim.ResetTrigger("powerslash");
        anim.ResetTrigger("surroundspear");
        anim.ResetTrigger("sandwichspear");
        anim.ResetTrigger("guard");
        anim.ResetTrigger("powerguard");
        anim.ResetTrigger("counter");
        anim.SetBool("assault", false);
        shieldKnightEffect.AllClear();
        shieldKnightAttack.AllClear();
    }
    void Dead()
    {
        Destroy(transform.parent.gameObject);
    }
}
