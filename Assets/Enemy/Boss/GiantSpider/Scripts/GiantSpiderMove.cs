using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����w偂̈ړ�����B�X�e�[�^�X����A�j���[�V�����̏�Ԃ�ǂ݁A����ɉ����Ĉړ��B
public class GiantSpiderMove : MonoBehaviour
{
    [SerializeField] private GiantSpiderStatus giantSpiderStatus = null;
    private Rigidbody2D rb2D = null;
    private float xSpeed = 0;
    private float ySpeed = 0;
    [SerializeField] private GroundCheck bottomGroundChecker = null;
    [SerializeField] private GroundCheck wallGroundChecker = null;

    private Vector2 beemStartPos = Vector2.zero;//�r�[�����˂Ɉڂ�O�̈ʒu
    private Vector2 toCentralVec = Vector2.zero;//��ʒ���
    private float progress = 0;//�r�[�����ˏ����ʒu�Ɣ��ˈʒu�܂ł̐i�s�x

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
            else if ((giantSpiderStatus.IsJump() || giantSpiderStatus.IsTackle()) && wallGroundChecker.IsGround())
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
        if(giantSpiderStatus.IsStand() || giantSpiderStatus.IsDead())
        {
            xSpeed = 0;
            if (bottomGroundChecker.IsGround())
            {
                ySpeed = 0;
            }
            else
            {
                ySpeed = -giantSpiderStatus.VerSpeed;
            }
        }
        else if (giantSpiderStatus.IsTackle())
        {
            xSpeed = NowDerection(this.transform.localScale) * giantSpiderStatus.HorSpeed;
            ySpeed = 0;
        }
        else if (giantSpiderStatus.IsPostTackle())
        {
            xSpeed = NowDerection(this.transform.localScale) * giantSpiderStatus.HorSpeed / 2;
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
        else if(giantSpiderStatus.IsPreWebBeem())
        {
            progress += Time.deltaTime * 2;
            this.transform.position = Vector2.Lerp(beemStartPos, (Vector2)giantSpiderStatus.CameraTrans.position, progress);
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

    //�W�����v���Ăяo��
    public void JumpUp()
    {
        ySpeed = giantSpiderStatus.VerSpeed;
        this.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
    }

    //�E�F�u�r�[���̗\������J�n��
    public void BeemStandby()
    {
        progress = 0;
        this.transform.position = this.transform.position + new Vector3(0, 0.1f, 0);
        beemStartPos = this.transform.position;
        toCentralVec = ((Vector2)(giantSpiderStatus.CameraTrans.position) - beemStartPos).normalized;
    }
    public bool IsReachCentral()
    {
        if (progress > 0.9f)
        {
            return true;
        }
        return false;
    }
}
