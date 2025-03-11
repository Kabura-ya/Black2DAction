using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightMove : MonoBehaviour
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;
    private Rigidbody2D rb2D = null;
    private float xSpeed = 0;
    private float ySpeed = 0;

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
        if (shieldKnightStatus.PlayerTrans != null)
        {
            if (shieldKnightStatus.IsStand() ||
                shieldKnightStatus.IsWalk() ||
                shieldKnightStatus.IsPrePowerSlash() ||
                shieldKnightStatus.IsGuard() ||
                shieldKnightStatus.IsPowerGuard())
            {
                if (this.transform.position.x < shieldKnightStatus.PlayerTrans.position.x)
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
        if (shieldKnightStatus.IsWalk())
        {
            xSpeed = NowDerection(this.transform.localScale) * shieldKnightStatus.WalkSpeed;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed = -shieldKnightStatus.FallSpeed;
            }
        }
        else if (shieldKnightStatus.IsAssault() || shieldKnightStatus.IsPowerAssault())
        {
            xSpeed = NowDerection(this.transform.localScale) * shieldKnightStatus.AssaultSpeed;
            ySpeed = 0;
        }
        else if (shieldKnightStatus.IsPowerSlash())
        {
            xSpeed = NowDerection(this.transform.localScale) * shieldKnightStatus.PowerSlashSpeed;
            ySpeed = 0;
        }
        else if (shieldKnightStatus.IsStan())
        {
            xSpeed = -NowDerection(this.transform.localScale) * shieldKnightStatus.KnockBackSpeed;
            ySpeed = 0;
        }
        else
        {
            xSpeed = 0;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed = -shieldKnightStatus.FallSpeed;
            }
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
}
