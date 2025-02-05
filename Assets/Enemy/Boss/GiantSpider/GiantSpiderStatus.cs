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

    [SerializeField] private Animator anim = null;

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

    public bool IsJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Jump");
    }

    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
}
