using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//巨大蜘蛛の行動パターン。状態遷移制御。
//設定回数分回ジャンプ＞突進＞特殊行動のループ。各行動には後隙が存在。
public class GiantSpiderPattern : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private Collider2D bottomCollider2D = null;
    [SerializeField] private GiantSpiderMove giantSpiderMove = null;

    private int jumpCount = 0;
    private float walkTime = 0;
    private float coolTime = 0;

    void Awake()
    {
        jumpCount = giantSpiderStatus.JumpCount;
        walkTime = giantSpiderStatus.WalkTime;
        coolTime = giantSpiderStatus.CoolTime;
    }

    void Update()
    {
        if (giantSpiderStatus.IsStand())
        {
            if (bottomGroundChecker.IsGround())
            {
                coolTime -= Time.deltaTime;
                if (coolTime < 0)
                {
                    if (jumpCount > 0)
                    {
                        jumpCount--;
                        bottomCollider2D.enabled = false;
                        giantSpiderMove.JumpUp();
                        giantSpiderStatus.JumpSwitch(1);
                        giantSpiderAttack.GenerateWebShot();
                        coolTime = giantSpiderStatus.CoolTime;
                        bottomCollider2D.enabled = true;
                    }
                    else if(walkTime > 0) 
                    {
                        giantSpiderStatus.WalkSwitch(1);
                        giantSpiderAttack.TackleSwitch(1);
                        coolTime = giantSpiderStatus.CoolTime;
                    }
                    else
                    {
                        float randomValue = Random.Range(0f, 1f);
                        if (randomValue >= 0.5f)
                        {
                            giantSpiderStatus.GuillotineTrigger();
                        }
                        else
                        {
                            giantSpiderStatus.PreWebBeemTrigger();
                            giantSpiderMove.BeemStandby();
                        }
                        jumpCount = giantSpiderStatus.JumpCount;
                        walkTime = giantSpiderStatus.WalkTime;
                        coolTime = giantSpiderStatus.CoolTime;
                    }
                }
            }
        }
        else if(giantSpiderStatus.IsJump())
        {
            if(bottomGroundChecker.IsGround())
            {
                giantSpiderStatus.JumpSwitch(0);
            }
        }
        else if (giantSpiderStatus.IsWalk())
        {
            walkTime -= Time.deltaTime;
            if (walkTime < 0)
            {
                giantSpiderStatus.WalkSwitch(0);
                giantSpiderAttack.TackleSwitch(0);
            }
        }
        else if(giantSpiderStatus.IsPreWebBeem())
        {
            if(giantSpiderMove.IsReachCentral())
            {
                giantSpiderStatus.WebBeemTrigger();
            }
        }
    }
}
