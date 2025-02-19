using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorStatus : MonoBehaviour
{
    [Header("�̗�"), SerializeField] private int maxHp = 200;
    public int MaxHp => maxHp;

    [Header("�A���U����"), SerializeField] private int rushCount = 3;
    public int RushCount => rushCount;

    [Header("���鑬�x"), SerializeField] private float runSpeed = 20;
    public float RunSpeed => runSpeed;//�O���擾�����\
    [Header("���鎞��"), SerializeField] private float runTime = 1.5f;
    public float RunTime => runTime;

    [Header("�t�F�C���g���̈ړ����x"), SerializeField] private float feintSpeed = 2;
    public float FeintSpeed => feintSpeed;//�O���擾�����\
    [Header("�t�F�C���g���̈ړ�����"), SerializeField] private float feintTime = 0.2f;
    public float FeintTime => feintTime;//�O���擾�����\

    [Header("��ޑ��x"), SerializeField] private float backSpeed = 3;
    public float BackSpeed => backSpeed;//�O���擾�����\

    [Header("�W�����v���x"), SerializeField] private float jumpSpeed = 10;
    public float JumpSpeed => jumpSpeed;//�O���擾�����\
    [Header("�W�����v����"), SerializeField] private float jumpTime = 0.5f;
    public float JumpTime => jumpTime;//�O���擾�����\
    [Header("�����U�����x"), SerializeField] private float meteorSpeed = 20;
    public float MeteorSpeed => meteorSpeed;//�O���擾�����\

    [Header("�ːi���x"), SerializeField] private float tackleSpeed = 10;
    public float TackleSpeed => tackleSpeed;//�O���擾�����\
    [Header("���鎞��"), SerializeField] private float tackleTime = 0.5f;
    public float TackleTime => tackleTime;

    [Header("�e�C���u���[�h�����x"), SerializeField] private float tailbradeXSpeed = 10;
    public float TailBradeXSpeed => tailbradeXSpeed;//�O���擾�����\
    [Header("�e�C���u���[�h�c���x"), SerializeField] private float tailbradeYSpeed = 10;
    public float TailBradeYSpeed => tailbradeYSpeed;//�O���擾�����\

    [Header("�������x"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//�O���擾�����\

    [Header("�㌄"), SerializeField] private float coolTime = 0.3f;
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

    public bool IsPreSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreSpawn");
    }
    public void GroundTrigger()
    {
        anim.SetTrigger("ground");
    }
    public bool IsSpawn()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn");
    }

    public bool IsStand()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Stand");
    }

    public bool IsPreRun()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreRun");
    }
    public bool IsRun()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Run");
    }
    public void RunTrigger()
    {
        anim.SetTrigger("run");
    }
    public bool IsUpper()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Upper");
    }
    public void UpperTrigger()
    {
        anim.SetTrigger("upper");
    }

    public bool IsSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Slash");
    }
    public void SlashTrigger()
    {
        anim.SetTrigger("slash");
    }
    public bool IsSmmersault()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Summersault");
    }

    public bool IsPreBackSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreBackSlash");
    }
    public bool IsBackSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("BackSlash");
    }
    public void BackSlashTrigger()
    {
        anim.SetTrigger("backslash");
    }

    public bool IsMeteorJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("MeteorJump");
    }
    public bool IsPreMeteor()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreMeteor");
    }
    public bool IsMeteor()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Meteor");
    }
    public void MeteorJumpTrigger()
    {
        anim.SetTrigger("meteorjump");
    }
    public void MeteorTrigger()
    {
        anim.SetTrigger("meteor");
    }

    public bool IsFeint()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Feint");
    }
    public bool IsPunch()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Punch");
    }
    public bool IsPostPunch()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostPunch");
    }
    public void FeintTrigger()
    {
        anim.SetTrigger("feint");
    }
    public void PunchOn()
    {
        anim.SetBool("punch", true);
    }
    public void PunchOff()
    {
        anim.SetBool("punch", false);
    }

    public bool IsClaw()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Claw");
    }
    public void ClawTrigger()
    {
        anim.SetTrigger("claw");
    }

    public bool IsPreTailBrade()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PreTailBrade");
    }
    public bool IsTailBrade()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("TailBrade");
    }
    public bool IsPostTailBrade()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostTailBrade");
    }
    public bool IsPostTailBrade2()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostTailBrade2");
    }
    public void TailBradeTrigger()
    {
        anim.SetTrigger("tailbrade");
    }

    public bool IsPunishHammerJump()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PunishHammerJump");
    }
    public bool IsPrePunishHammer()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PrePunishHammer");
    }
    public bool IsPunishHammer()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PunishHammer");
    }
    public bool IsPostPunishHammer()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PostPunishHammer");
    }
    public void PunishHammerJumpTrigger()
    {
        anim.SetTrigger("punishhammerjump");
    }
    public void PunishHammerTrigger()
    {
        anim.SetTrigger("punishhammer");
    }

    public bool IsPowerSlash()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("PowerSlash");
    }
    public void PowerSlashTrigger()
    {
        anim.SetTrigger("powerslash");
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
