using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//巨大蜘蛛の行動パターン
public class GiantSpiderPattern : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private Collider2D bottomCollider2D = null;
    [SerializeField] private GiantSpiderMove giantSpiderMove = null;
    [SerializeField] private Animator anim = null;

    [SerializeField] private float coolTime = 0;//ステータスで攻撃ごとにクールタイム付けたい
    private float walkTime = 0;
    private int actionNum = 0;

    void Awake()
    {
        coolTime = 0;
    }

    void Update()
    {
        if(giantSpiderStatus.IsStand())
        {
            if (bottomGroundChecker.IsGround())
            {
                coolTime -= Time.deltaTime;
                if (coolTime < 0)
                {
                    actionNum = Random.Range(0, 10);
                    if (actionNum > 5)
                    {
                        bottomCollider2D.enabled = false;
                        giantSpiderMove.JumpUp();
                        anim.SetBool("jump", true);
                        coolTime = 0.5f;
                        bottomCollider2D.enabled = true;
                    }
                    else
                    {
                        anim.SetBool("walk", true);
                        walkTime = 2;
                        coolTime = 0.2f;
                    }
                }
            }
        }
        else if(giantSpiderStatus.IsJump())
        {
            if(bottomGroundChecker.IsGround())
            {
                anim.SetBool("jump", false);
            }
        }
        else if (giantSpiderStatus.IsWalk())
        {
            walkTime -= Time.deltaTime;
            if (walkTime < 0)
            {
                anim.SetBool("walk", false);
            }
        }
    }
}
