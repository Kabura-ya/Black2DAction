using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightPattern : MonoBehaviour
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;
    [SerializeField] private ShieldKnightAttack shieldKnightAttack = null;
    [SerializeField] private ShieldKnightEffect shieldKnightEffect = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;

    private float assaultTime = 0;
    private float guardTime = 0;
    private float coolTime = 0;

    void Awake()
    {
        assaultTime = shieldKnightStatus.AssaultTime;
        guardTime = shieldKnightStatus.GuardTime;
        coolTime = shieldKnightStatus.CoolTime;
    }

    void Update()
    {
        if (bottomGroundChecker.IsGround() && shieldKnightStatus.IsStand())
        {
            if(!IsClose())
            {
                shieldKnightStatus.WalkOn();
            }
            coolTime -= Time.deltaTime;
            if (coolTime <= 0)
            {
                coolTime = shieldKnightStatus.CoolTime;
                SelectPattern();
            }
        }
        else if (bottomGroundChecker.IsGround() && shieldKnightStatus.IsWalk())
        {
            if (IsClose())
            {
                shieldKnightStatus.WalkOff();
            }
            coolTime -= Time.deltaTime;
            if (coolTime <= 0)
            {
                coolTime = shieldKnightStatus.CoolTime;
                SelectPattern();
            }
        }
        else if (shieldKnightStatus.IsGuard())
        {
            guardTime -= Time.deltaTime;
            if (guardTime <= 0)
            {
                shieldKnightEffect.GuardOff();
                assaultTime = shieldKnightStatus.AssaultTime;
                shieldKnightStatus.AssaultOn();
                shieldKnightAttack.AssaultOn();
            }
        }
        else if (shieldKnightStatus.IsPowerGuard())
        {
            guardTime -= Time.deltaTime;
            if (guardTime <= 0)
            {
                shieldKnightEffect.PowerGuardOff();
                assaultTime = shieldKnightStatus.AssaultTime;
                shieldKnightStatus.AssaultOn();
                shieldKnightAttack.PowerAssaultOn();
            }
        }
        else if (shieldKnightStatus.IsAssault())
        {
            assaultTime -= Time.deltaTime;
            if (assaultTime <= 0)
            {
                shieldKnightStatus.AssaultOff();
                shieldKnightAttack.AssaultOff();
            }
        }
        else if (shieldKnightStatus.IsPowerAssault())
        {
            assaultTime -= Time.deltaTime;
            if (assaultTime <= 0)
            {
                shieldKnightStatus.AssaultOff();
                shieldKnightAttack.PowerAssaultOff();
            }
        }
    }

    public void GuardActive()
    {
        coolTime = shieldKnightStatus.CoolTime;
        guardTime = shieldKnightStatus.GuardTime;
        shieldKnightStatus.GuardTrigger();
        shieldKnightEffect.GuardOn();
    }
    public void CounterSuccess()
    {
        shieldKnightStatus.CounterTrigger();
        shieldKnightEffect.GuardOff();
    }

    public void PowerGuardActive()
    {
        coolTime = shieldKnightStatus.CoolTime;
        guardTime = shieldKnightStatus.GuardTime;
        shieldKnightStatus.PowerGuardTrigger();
        shieldKnightEffect.PowerGuardOn();
    }
    public void PowerCounterSuccess()
    {
        shieldKnightStatus.CounterTrigger();
        shieldKnightEffect.PowerGuardOff();
    }

    public void GuardBreak()
    {
        coolTime = 0;
        shieldKnightStatus.StanPlay();
    }

    private void SelectPattern()
    {
        float randomValue = Random.Range(0f, 1f);
        if (IsClose())
        {
            if (randomValue < 0.5f)
            {
                shieldKnightStatus.SlashTrigger();
            }
            else if(randomValue >= 0.5f && randomValue < 0.8f)
            {
                shieldKnightStatus.PowerSlashTrigger();
            }
            else
            {
                shieldKnightStatus.SandwichSpearSpearTrigger();
            }
        }
        else if(IsMiddle())
        {
            if (randomValue < 0.7f)
            {
                shieldKnightStatus.PowerSlashTrigger();
            }
            else
            {
                shieldKnightStatus.SandwichSpearSpearTrigger();
            }

        }
        else if(IsFar())
        {
            if (randomValue < 0.5f)
            {
                shieldKnightStatus.SurroundSpearTrigger();
            }
            else
            {
                shieldKnightStatus.SandwichSpearSpearTrigger();
            }
        }
    }

    //ƒvƒŒƒCƒ„[‚Æ‚Ì‹——£‚É‚æ‚Á‚Ä‹Z‚ª•Ï‚í‚é‚æ‚¤‚É‚µ‚½‚¢
    private bool IsClose()
    {
        return Mathf.Abs(this.transform.position.x - shieldKnightStatus.PlayerTrans.position.x) <= 3;
    }
    private bool IsMiddle()
    {
        float distance = Mathf.Abs(this.transform.position.x - shieldKnightStatus.PlayerTrans.position.x);
        return distance > 1 && distance <= 6;
    }
    private bool IsFar()
    {
        return Mathf.Abs(this.transform.position.x - shieldKnightStatus.PlayerTrans.position.x) > 8;
    }
}
