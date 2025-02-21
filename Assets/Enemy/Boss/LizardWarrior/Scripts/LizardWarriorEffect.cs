using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorEffect : MonoBehaviour
{
    [SerializeField] private GameObject feint = null;
    [SerializeField] private GameObject pretailblade = null;

    [SerializeField] private GameObject summersault = null;
    [SerializeField] private GameObject prepowerslash = null;
    [SerializeField] private GameObject powerslash = null;

    [SerializeField] private GameObject run = null;
    [SerializeField] private GameObject slash = null;
    [SerializeField] private GameObject presmash = null;
    [SerializeField] private GameObject smashjump = null;

    void Awake()
    {
        FeintOff();
        PreTailBladeOff();

        SummersaultOff();
        PrePowerSlashOff();
        PowerSlashOff();
        SlashOff();

        RunOff();
        PreSmashOff();
        SmashJumpOff();
    }

    public void FeintOn()
    {
        feint.SetActive(true);
    }
    public void FeintOff()
    {
        feint.SetActive(false);
    }

    public void PreTailBladeOn()
    {
        pretailblade.SetActive(true);
    }
    public void PreTailBladeOff()
    {
        pretailblade.SetActive(false);
    }

    public void SummersaultOn()
    {
        summersault.SetActive(true);
    }
    public void SummersaultOff()
    {
        summersault.SetActive(false);
    }

    public void PrePowerSlashOn()
    {
        prepowerslash.SetActive(true);
    }
    public void PrePowerSlashOff()
    {
        prepowerslash.SetActive(false);
    }
    public void PowerSlashOn()
    {
        powerslash.SetActive(true);
    }
    public void PowerSlashOff()
    {
        powerslash.SetActive(false);
    }

    public void SlashOn()
    {
        slash.SetActive(true);
    }
    public void SlashOff()
    {
        slash.SetActive(false);
    }

    public void RunOn()
    {
        run.SetActive(true);
    }
    public void RunOff()
    {
        run.SetActive(false);
    }

    public void PreSmashOn()
    {
        presmash.SetActive(true);
    }
    public void PreSmashOff()
    {
        presmash.SetActive(false);
    }
    public void SmashJumpOn()
    {
        smashjump.SetActive(true);
    }
    public void SmashJumpOff()
    {
        smashjump.SetActive(false);
    }

    public void AllClear()
    {
        FeintOff();
        PreTailBladeOff();

        SummersaultOff();
        PrePowerSlashOff();
        PowerSlashOff();

        SlashOff();
        RunOff();
        PreSmashOff();
        SmashJumpOff();
    }
}
