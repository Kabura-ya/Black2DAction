using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorMove : MonoBehaviour
{
    [SerializeField] private LizardWarriorStatus lizardWarriorStatus = null;
    private Rigidbody2D rb2D = null;
    private float xSpeed = 0;
    private float ySpeed = 0;

    private float jumpTime = 0;
    private Vector2 toJumpPos = Vector2.zero;
    private Vector2 jumpVec = Vector2.zero;

    [SerializeField] private GroundCheck bottomGroundChecker = null;

    private Vector3 originScale = new Vector3(0, 0, 0);

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originScale = this.transform.localScale;
        if (originScale.x < 0)
        {
            originScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }
        if (originScale.y < 0)
        {
            originScale = new Vector3(this.transform.localScale.x, -this.transform.localScale.y, this.transform.localScale.z);
        }
        if (originScale.z < 0)
        {
            originScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, -this.transform.localScale.z);
        }
    }

    void Update()
    {
        if (lizardWarriorStatus.PlayerTrans != null)
        {
            if (lizardWarriorStatus.IsStand() ||
                lizardWarriorStatus.IsPreRun() ||
                lizardWarriorStatus.IsPreTailBlade())
            {
                if (this.transform.position.x < lizardWarriorStatus.PlayerTrans.position.x)
                {
                    this.transform.localScale = originScale;
                }
                else
                {
                    this.transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (lizardWarriorStatus.IsStand() || lizardWarriorStatus.IsDead() || lizardWarriorStatus.IsPreSpawn())
        {
            xSpeed = 0;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed = -lizardWarriorStatus.VerSpeed;
            }
        }
        else if (lizardWarriorStatus.IsRun())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.RunSpeed;
            ySpeed = 0;
        }
        else if (lizardWarriorStatus.IsBackSlash())
        {
            xSpeed = -NowDerection(this.transform.localScale) * lizardWarriorStatus.BackSpeed;
            ySpeed = 0;
        }
        else if (lizardWarriorStatus.IsPressJump() || lizardWarriorStatus.IsSmashJump())
        {
            xSpeed = jumpVec.x * lizardWarriorStatus.JumpSpeed;
            ySpeed = jumpVec.y * lizardWarriorStatus.JumpSpeed;
            jumpTime -= Time.deltaTime;
        }
        else if (lizardWarriorStatus.IsPress() || lizardWarriorStatus.IsSmash())
        {
            xSpeed = 0;
            ySpeed = -lizardWarriorStatus.MeteorSpeed;
        }
        else if (lizardWarriorStatus.IsFeint())
        {
            xSpeed = -NowDerection(this.transform.localScale) * lizardWarriorStatus.FeintSpeed;
            ySpeed = 0;
        }
        else if (lizardWarriorStatus.IsPunch())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.FeintSpeed;
            ySpeed = 0;
        }
        else if (lizardWarriorStatus.IsTailBlade())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.TailBradeXSpeed;
            ySpeed = lizardWarriorStatus.TailBradeYSpeed;
        }
        else if (lizardWarriorStatus.IsPostTailBlade())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.TailBradeXSpeed;
            ySpeed = -lizardWarriorStatus.TailBradeYSpeed;
        }
        else
        {
            xSpeed = 0;
            ySpeed = 0;
        }
        rb2D.velocity = new Vector2(xSpeed, ySpeed);
    }

    private int NowDerection(Vector3 scale)
    {
        if (scale.x > 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    //ÉWÉÉÉìÉvéûåƒÇ—èoÇµ
    public void JumpUp()
    {
        toJumpPos = new Vector2(lizardWarriorStatus.PlayerTrans.position.x, this.transform.position.y + lizardWarriorStatus.JumpHigh);
        jumpTime = lizardWarriorStatus.JumpTime;
        jumpVec = new Vector2(lizardWarriorStatus.PlayerTrans.position.x - this.transform.position.x, lizardWarriorStatus.JumpHigh).normalized;
        xSpeed = jumpVec.x * lizardWarriorStatus.JumpSpeed;
        ySpeed = jumpVec.y * lizardWarriorStatus.JumpSpeed;
        this.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
    }
    public bool IsReach()
    {
        return ((lizardWarriorStatus.IsPressJump() || lizardWarriorStatus.IsSmashJump()) && 
                (jumpTime < 0 || Vector2.Distance(toJumpPos, this.transform.position) < 0.5f));
    }

    public void TailBladeUp()
    {
        xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.TailBradeXSpeed;
        ySpeed = lizardWarriorStatus.TailBradeYSpeed;
        this.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
    }
}
