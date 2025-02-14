using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorMove : MonoBehaviour
{
    [SerializeField] private LizardWarriorStatus lizardWarriorStatus = null;
    private Rigidbody2D rb2D = null;
    private float xSpeed = 0;
    private float ySpeed = 0;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private GroundCheck wallGroundChecker = null;

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
            if (lizardWarriorStatus.IsStand())
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
            if ((lizardWarriorStatus.IsJump() || lizardWarriorStatus.IsRun()) && wallGroundChecker.IsGround())
            {
                if (this.transform.localScale.x < 0)
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
        if (lizardWarriorStatus.IsStand() || lizardWarriorStatus.IsDead())
        {
            xSpeed = 0;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed -= Time.deltaTime * lizardWarriorStatus.VerSpeed;
                if (ySpeed < -lizardWarriorStatus.VerSpeed)
                {
                    ySpeed = -lizardWarriorStatus.VerSpeed;
                }
            }
        }
        else if (lizardWarriorStatus.IsRun())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.HorSpeed;
            ySpeed = 0;
        }
        else if (lizardWarriorStatus.IsJump())
        {
            xSpeed = NowDerection(this.transform.localScale) * lizardWarriorStatus.HorSpeed;
            ySpeed -= Time.deltaTime * lizardWarriorStatus.VerSpeed;
            if (ySpeed < -lizardWarriorStatus.VerSpeed)
            {
                ySpeed = -lizardWarriorStatus.VerSpeed;
            }
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
        ySpeed = lizardWarriorStatus.VerSpeed;
        this.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
    }
}
