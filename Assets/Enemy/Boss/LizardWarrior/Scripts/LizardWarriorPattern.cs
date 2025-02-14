using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorPattern : MonoBehaviour
{
    [SerializeField] private LizardWarriorStatus lizardWarriorStatus = null;
    [SerializeField] private LizardWarriorAttack lizardWarriorAttack = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private Collider2D bottomCollider2D = null;
    [SerializeField] private LizardWarriorMove lizardWarriorMove = null;

    private float runTime = 0;
    private float coolTime = 0;

    void Awake()
    {
        runTime = lizardWarriorStatus.RunTime;
        coolTime = lizardWarriorStatus.CoolTime;
    }

    void Update()
    {
        if (lizardWarriorStatus.IsStand())
        {
            if (bottomGroundChecker.IsGround())
            {
                coolTime -= Time.deltaTime;
                if (coolTime < 0)
                {
                    if (runTime > 0)
                    {
                        lizardWarriorStatus.RunSwitch(1);
                        coolTime = lizardWarriorStatus.CoolTime;
                    }
                    else
                    {
                        runTime = lizardWarriorStatus.RunTime;
                        coolTime = lizardWarriorStatus.CoolTime;
                    }
                }
            }
        }
        else if (lizardWarriorStatus.IsRun())
        {
            runTime -= Time.deltaTime;
            if (runTime < 0)
            {
                lizardWarriorStatus.UpperTrigger();
                lizardWarriorStatus.RunSwitch(0);
            }
        }
    }
}
