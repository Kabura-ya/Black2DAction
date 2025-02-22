using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightStatus : MonoBehaviour
{
    [Header("体力"), SerializeField] private int maxHp = 200;
    public int MaxHp => maxHp;

    [Header("落下速度"), SerializeField] private float fallSpeed = 5;
    public float FallSpeed => fallSpeed;//外部取得だけ可能

    [Header("歩行速度"), SerializeField] private float walkSpeed = 5;
    public float WalkSpeed => walkSpeed;//外部取得だけ可能
    [Header("歩行時間"), SerializeField] private float walkTime = 0.3f;
    public float WalkTime => walkTime;

    [Header("ガード時間"), SerializeField] private float guardTime = 1.5f;
    public float GuardTime => guardTime;

    [Header("突進速度"), SerializeField] private float assaultSpeed = 10;
    public float AssaultSpeed => assaultSpeed;//外部取得だけ可能
    [Header("突進時間"), SerializeField] private float assaultTime = 1.5f;
    public float AssaultTime => assaultTime;

    [Header("強切り速度"), SerializeField] private float powerslashSpeed = 10;
    public float PowerSlashSpeed => powerslashSpeed;//外部取得だけ可能

    [SerializeField] private Animator anim = null;

    private Transform playerTrans = null;
    public Transform PlayerTrans => playerTrans;

    void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
    }

    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsWalk()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    }

    public bool IsSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Slash");
    }
    public void SlashTrigger()
    {
        anim.SetTrigger("slash");
    }

    public bool IsThunderStorm()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("ThunderStorm");
    }
    public void ThunderStormTrigger()
    {
        anim.SetTrigger("thunderstorm");
    }

    public bool IsPrePowerSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PrePowerSlash");
    }
    public bool IsPowerSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PowerSlash");
    }
    public bool IsPostPowerSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostPowerSlash");
    }
    public void PowerSlashTrigger()
    {
        anim.SetTrigger("powerslash");
    }

    public bool IsGuard()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Guard");
    }
    public void GuardTrigger()
    {
        anim.SetTrigger("guard");
    }
    public bool IsPowerGuard()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PowerGuard");
    }
    public void PowerGuardTrigger()
    {
        anim.SetTrigger("powerguard");
    }

    public bool IsAssault()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Assault");
    }
    public bool IsPowerAssault()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PowerAssault");
    }
    public void AssaultOn()
    {
        anim.SetBool("assault", true);
    }
    public void AssaultOff()
    {
        anim.SetBool("assault", false);
    }

    public bool IsCounter()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Counter");
    }
    public bool IsPowerCounter()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PowerCounterSlash");
    }
    public void CounterTrigger()
    {
        anim.SetTrigger("counter");
    }

    public bool IsStan()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stan");
    }
    public void StanPlay()
    {
        anim.Play("Stan");
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
