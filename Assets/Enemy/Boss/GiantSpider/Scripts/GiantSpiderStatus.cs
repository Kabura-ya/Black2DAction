using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//巨大蜘蛛のステータスをまとめたもの。
//アニメーションのクリップ名から現状態を取得。
public class GiantSpiderStatus : MonoBehaviour
{
    [Header("体力"), SerializeField] private int maxHp = 200;
    public int MaxHp => maxHp;

    [Header("並行速度"),SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//外部取得だけ可能
    [Header("垂直速度"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//外部取得だけ可能

    [Header("ジャンプ回数"), SerializeField] private int jumpCount = 3;
    public int JumpCount => jumpCount;

    [Header("突進時間"), SerializeField] private float tackleTime = 1.5f;
    public float TackleTime => tackleTime;

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

    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsPreTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreTackle");
    }
    public bool IsTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Tackle");
    }
    public bool IsPostTackle()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostTackle");
    }
    public void TackleSwitch(int i)
    {
        if(i > 0)
        {
            anim.SetBool("tackle", true);
        }
        else
        {
            anim.SetBool("tackle", false);
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

    public bool IsPreWebBeem()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreWebBeem");
    }
    public void PreWebBeemTrigger()
    {
        anim.SetTrigger("prewebbeem");
    }

    public bool IsWebBeem()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("WebBeem");
    }
    public void WebBeemTrigger()
    {
        anim.SetTrigger("webbeem");
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
