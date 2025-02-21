using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorAnimationEvent : MonoBehaviour
{
    [SerializeField] private LizardWarriorAttack lizardWarriorAttack = null;
    [SerializeField] private LizardWarriorEffect lizardWarriorEffect = null;
    [SerializeField] private LizardWarriorMove lizardWarriorMove = null;
    private Animator anim = null;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void ClawOn()
    {
        lizardWarriorAttack.ClawOn();
    }

    void PreTailBladeOn()
    {
        lizardWarriorEffect.PreTailBladeOn();
    }
    void PreTailBladeOff()
    {
        lizardWarriorEffect.PreTailBladeOff();
    }
    void TailBladeOn()
    {
        lizardWarriorAttack.TailBladeOn();
    }

    void SummersaultOn()
    {
        lizardWarriorEffect.SummersaultOn();
        lizardWarriorAttack.GenerateSlashWave();
    }

    void PrePowerSlashOn()
    {
        lizardWarriorEffect.PrePowerSlashOn();
    }
    void PrePowerSlashOff()
    {
        lizardWarriorEffect.PrePowerSlashOff();
    }
    void PowerSlashOn()
    {
        lizardWarriorAttack.GenerateSlashWave2();
        lizardWarriorEffect.PowerSlashOn();
    }

    void SlashOn()
    {
        lizardWarriorEffect.SlashOn();
        lizardWarriorAttack.GenerateSlashWave();
    }

    void RunOn()
    {
        lizardWarriorEffect.RunOn();
    }
    void UpperOn()
    {
        lizardWarriorAttack.UpperOn();
    }

    void PreSmashOn()
    {
        lizardWarriorEffect.PreSmashOn();
    }
    void PreSmashOff()
    {
        lizardWarriorEffect.PreSmashOff();
    }
    void SmashJumpOn()
    {
        lizardWarriorEffect.SmashJumpOn();
    }

    void JumpUp()
    {
        lizardWarriorMove.JumpUp();
    }

    void AllClear()
    {
        anim.ResetTrigger("run");
        anim.ResetTrigger("upper");
        anim.ResetTrigger("ground");
        anim.ResetTrigger("slash");
        anim.ResetTrigger("prepress");
        anim.ResetTrigger("press");
        anim.ResetTrigger("claw");
        anim.ResetTrigger("backslash");
        anim.ResetTrigger("feint");
        anim.ResetTrigger("tailblade");
        anim.ResetTrigger("presmash");
        anim.ResetTrigger("smash");
        anim.ResetTrigger("powerslash");
        anim.SetBool("punch", false);
        lizardWarriorEffect.AllClear();
        lizardWarriorAttack.AllClear();
    }
    void Dead()
    {
        Destroy(transform.root.gameObject);
    }
}
