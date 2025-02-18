using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorAnimationEvent : MonoBehaviour
{
    [SerializeField] private LizardWarriorAttack lizardWarriorAttack = null;
    [SerializeField] private GameObject run = null;
    [SerializeField] private GameObject slash = null;
    [SerializeField] private GameObject summersault = null;

    void Awake()
    {
        RunOff();
        SlashOff();
        SummersaultOff();
    }

    void RunOn()
    {
        run.SetActive(true);
    }
    void RunOff()
    {
        run.SetActive(false);
    }
    void UpperOn()
    {
        lizardWarriorAttack.UpperOn();
    }
    void UpperOff()
    {
        lizardWarriorAttack.UpperOff();
    }

    public void SlashOn()
    {
        slash.SetActive(true);
        lizardWarriorAttack.GenerateSlashWave();
    }
    public void SlashOff()
    {
        slash.SetActive(false);
    }
    public void SummersaultOn()
    {
        summersault.SetActive(true);
        lizardWarriorAttack.GenerateSlashWave();
    }
    public void SummersaultOff()
    {
        summersault.SetActive(false);
    }

    void PreDead()
    {

    }
    void Dead()
    {
        Destroy(transform.root.gameObject);
    }
}
