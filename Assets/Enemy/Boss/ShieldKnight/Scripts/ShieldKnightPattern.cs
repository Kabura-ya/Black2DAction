using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightPattern : MonoBehaviour
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;

    private float assaultTime = 0;
    private float guardTime = 0;
    private float walkTime = 0;

    void Awake()
    {
        assaultTime = shieldKnightStatus.AssaultTime;
        guardTime = shieldKnightStatus.GuardTime;
        walkTime = shieldKnightStatus.WalkTime;
    }

    void Update()
    {
        if (bottomGroundChecker.IsGround() && shieldKnightStatus.IsWalk())
        {
            walkTime -= Time.deltaTime;
            if (walkTime <= 0)
            {
                walkTime = shieldKnightStatus.WalkTime;
                int num = Random.Range(0, 3);
                if (num == 0)
                {
                    shieldKnightStatus.SlashTrigger();
                }
                else if (num == 1)
                {
                    shieldKnightStatus.ThunderStormTrigger();
                }
                else if (num == 2)
                {
                    shieldKnightStatus.PowerSlashTrigger();
                }
            }
        }
        else if (shieldKnightStatus.IsGuard() || shieldKnightStatus.IsPowerGuard())
        {
            guardTime -= Time.deltaTime;
            if (guardTime <= 0)
            {
                assaultTime = shieldKnightStatus.AssaultTime;
                shieldKnightStatus.AssaultOn();
            }
        }
        else if (shieldKnightStatus.IsAssault() || shieldKnightStatus.IsPowerAssault())
        {
            assaultTime -= Time.deltaTime;
            if (assaultTime <= 0)
            {
                shieldKnightStatus.AssaultOff();
            }
        }
    }

    public void GuardActive()
    {
        walkTime = shieldKnightStatus.WalkTime;
        guardTime = shieldKnightStatus.GuardTime;
        shieldKnightStatus.GuardTrigger();
    }

    public void CounterSuccess()
    {
        shieldKnightStatus.CounterTrigger();
    }
}
