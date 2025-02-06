using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

//����w偂̃X�e�[�^�X���܂Ƃ߂����́B
//�A�j���[�V�����̃N���b�v�����猻��Ԃ��擾�B
public class GiantSpiderStatus : MonoBehaviour
{
    [Header("���s���x"),SerializeField] private float horSpeed = 5;
    public float HorSpeed => horSpeed;//�O���擾�����\
    [Header("�������x"), SerializeField] private float verSpeed = 10;
    public float VerSpeed => verSpeed;//�O���擾�����\

    [Header("�W�����v��"), SerializeField] private int jumpCount = 3;
    public int JumpCount => jumpCount;

    [Header("�ːi����"), SerializeField] private float walkTime = 1.5f;
    public float WalkTime => walkTime;

    [Header("�㌄"), SerializeField] private float coolTime = 0.3f;
    public float CoolTime => coolTime;

    [SerializeField] private Animator anim = null;

    private Transform playerTrans = null;
    public Transform PlayerTrans => playerTrans;

    void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
    }

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
    public void WalkSwitch(int i)
    {
        if(i > 0)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
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

    public bool IsDead()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName("Dead");
    }
}
