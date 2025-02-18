using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorStatus : MonoBehaviour
{
    [Header("体力"), SerializeField] private int maxHp = 200;
    public int MaxHp => maxHp;

    [Header("並行速度"), SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//外部取得だけ可能
    [Header("垂直速度"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//外部取得だけ可能

    [Header("走る時間"), SerializeField] private float runTime = 1.5f;
    public float RunTime => runTime;

    [Header("後隙"), SerializeField] private float coolTime = 0.3f;
    public float CoolTime => coolTime;

    [SerializeField] private Animator anim = null;

    private Transform playerTrans = null;
    public Transform PlayerTrans => playerTrans;

    private Transform cameraTrans = null;
    public Transform CameraTrans => cameraTrans;

    void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
        cameraTrans = GameObject.Find("Main Camera").transform;
    }

    public bool IsPreSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreSpawn");
    }
    public void SpawnTrigger()
    {
        anim.SetTrigger("spawnground");
    }
    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsPreRun()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreRun");
    }
    public bool IsRun()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Run");
    }
    public void RunTrigger()
    {
        anim.SetTrigger("run");
    }
    public bool IsUpper()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Upper");
    }
    public void UpperTrigger()
    {
        anim.SetTrigger("upper");
    }

    public bool IsSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Slash");
    }
    public void SlashTrigger()
    {
        anim.SetTrigger("slash");
    }
    public bool IsSmmersault()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Summersault");
    }

    public bool IsJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    }
    public void JumpSwitch(int i)
    {
        if (i > 0)
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
    }


    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
    public void DeadPlay()
    {
        anim.Play("Dead");
    }
}
