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
        if (lizardWarriorStatus.IsPreSpawn())
        {
            if (bottomGroundChecker.IsGround())
            {
                lizardWarriorStatus.SpawnTrigger();
            }
        }
        else if (lizardWarriorStatus.IsStand())
        {
            if (bottomGroundChecker.IsGround())
            {
                coolTime -= Time.deltaTime;
                if (coolTime < 0)
                {
                    lizardWarriorStatus.RunTrigger();
                    runTime = lizardWarriorStatus.RunTime;
                    coolTime = lizardWarriorStatus.CoolTime;

                    
                    //lizardWarriorStatus.SlashTrigger();
                    
                }
            }
        }
        else if (lizardWarriorStatus.IsRun())
        {
            runTime -= Time.deltaTime;
            if (runTime < 0 || IsClose())
            {
                lizardWarriorStatus.UpperTrigger();
            }
        }
    }

    //ƒvƒŒƒCƒ„[‚Æ‚Ì‹——£‚É‚æ‚Á‚Ä‹Z‚ª•Ï‚í‚é‚æ‚¤‚É‚µ‚½‚¢
    private bool IsClose()
    {
        return Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x) <= 4;
    }
    private bool IsMiddle()
    {
        float distance = Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x);
        return distance > 4 && distance <= 8;
    }
    private bool IsFar()
    {
        return Mathf.Abs(this.transform.position.x - lizardWarriorStatus.PlayerTrans.position.x) > 8;
    }
}
