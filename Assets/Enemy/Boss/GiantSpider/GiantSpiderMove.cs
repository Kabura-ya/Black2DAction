using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//巨大蜘蛛の移動制御。ステータスからアニメーションの状態を読み、それに応じて移動。
public class GiantSpiderMove : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    private Rigidbody2D rb2D = null;
    private float xSpeed = 0;
    private float ySpeed = 0;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private GroundCheck wallGroundChecker = null;

    private Vector3 originScale = new Vector3 (0, 0, 0);

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        originScale = this.transform.localScale;
        if(originScale.x < 0)
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
        if (giantSpiderStatus.PlayerTrans != null)
        {
            if (giantSpiderStatus.IsStand())
            {
                if (this.transform.position.x < giantSpiderStatus.PlayerTrans.position.x)
                {
                    this.transform.localScale = originScale;
                }
                else
                {
                    this.transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
                }
            }
            if ((giantSpiderStatus.IsJump() || giantSpiderStatus.IsWalk()) && wallGroundChecker.IsGround())
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
        if(giantSpiderStatus.IsStand())
        {
            xSpeed = 0;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed -= Time.deltaTime * giantSpiderStatus.VerSpeed;
                if (ySpeed < -giantSpiderStatus.VerSpeed)
                {
                    ySpeed = -giantSpiderStatus.VerSpeed;
                }
            }
        }
        else if (giantSpiderStatus.IsWalk())
        {
            xSpeed = NowDerection(this.transform.localScale) * giantSpiderStatus.HorSpeed;
            ySpeed = 0;
        }
        else if (giantSpiderStatus.IsJump())
        {
            xSpeed = NowDerection(this.transform.localScale) * giantSpiderStatus.HorSpeed;
            ySpeed -= Time.deltaTime * giantSpiderStatus.VerSpeed;
            if (ySpeed < -giantSpiderStatus.VerSpeed)
            {
                ySpeed = -giantSpiderStatus.VerSpeed;
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

    //ジャンプ時呼び出し
    public void JumpUp()
    {
        ySpeed = giantSpiderStatus.VerSpeed;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.1f, this.transform.position.z);
    }
}
