using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//巨大蜘蛛のステータスをまとめたもの。
//アニメーションのクリップ名から現状態を取得。
public class GiantSpiderStatus : MonoBehaviour
{
    [Header("並行速度"),SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//外部取得だけ可能
    [Header("垂直速度"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//外部取得だけ可能

    [Header("ジャンプ回数"), SerializeField] private int jumpCount = 3;
    public int JumpCount => jumpCount;

    [Header("突進時間"), SerializeField] private float walkTime = 1.5f;
    public float WalkTime => walkTime;

    [Header("後隙"), SerializeField] private float coolTime = 0.3f;
    public float CoolTime => coolTime;

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

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsWalk()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Walk");
    }
    public void WalkSwitch(int i)
    {
        if(i > 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
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

    public bool IsGuillotine()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Guillotine");
    }
    public void GuillotineTrigger()
    {
        anim.SetTrigger("guillotine");
    }

    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
}
