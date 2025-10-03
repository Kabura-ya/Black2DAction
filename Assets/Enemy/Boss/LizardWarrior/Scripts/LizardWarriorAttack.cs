using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LizardWarriorAttack : MonoBehaviour
{
    [SerializeField] private GameObject claw = null;
    [SerializeField] private GameObject punch = null;
    [SerializeField] private GameObject tailblade = null;

    [SerializeField] private GameObject press = null;

    [SerializeField] private GameObject upper = null;
    [SerializeField] private GameObject smash = null;

    [SerializeField] private Transform slashwaveShotTrans = null;
    [SerializeField] private GameObject slashwave = null;
    [SerializeField] private Transform slashwave2ShotTrans = null;
    [SerializeField] private GameObject slashwave2 = null;

    public UnityEvent clearEvent;

    void Awake()
    {
        ClawOff();
        PunchOff();
        TailBladeOff();

        PressOff();

        UpperOff();
        SmashOff();
    }

    public void ClawOn()
    {
        claw.SetActive(true);
    }
    public void ClawOff()
    {
        claw.SetActive(false);
    }

    public void PunchOn()
    {
        punch.SetActive(true);
    }
    public void PunchOff()
    {
        punch.SetActive(false);
    }

    public void TailBladeOn()
    {
        tailblade.SetActive(true);
    }
    public void TailBladeOff()
    {
        tailblade.SetActive(false);
    }

    public void PressOn()
    {
        press.SetActive(true);
    }
    public void PressOff()
    {
        press.SetActive(false);
    }

    public void UpperOn()
    {
        upper.SetActive(true);
    }
    public void UpperOff()
    {
        upper.SetActive(false);
    }

    public void SmashOn()
    {
        smash.SetActive(true);
    }
    public void SmashOff()
    {
        smash.SetActive(false);
    }

    public void GenerateSlashWave()
    {
        GameObject generated = null; 
        if (this.transform.localScale.x > 0)
        {
            generated = Instantiate(slashwave, slashwaveShotTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            generated = Instantiate(slashwave, slashwaveShotTrans.position, Quaternion.Euler(0f, 0f, 180f));
        }
        DestroyRegist(generated);
    }
    public void GenerateSlashWave2()
    {
        GameObject generated = null;
        if (this.transform.localScale.x > 0)
        {
            generated = Instantiate(slashwave2, slashwave2ShotTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            generated = Instantiate(slashwave2, slashwave2ShotTrans.position, Quaternion.Euler(0f, 0f, 180f));
        }
        DestroyRegist(generated);
    }
    public void SmashSlashWave2()
    {
        GameObject generated = null;
        generated = Instantiate(slashwave2, slashwave2ShotTrans.position, Quaternion.Euler(0f, 0f, 0f));
        DestroyRegist(generated);

        generated = Instantiate(slashwave2, slashwave2ShotTrans.position, Quaternion.Euler(0f, 0f, 180f));
        DestroyRegist(generated);
    }

    //攻撃オブジェクトは死亡、スタン時に消去させたい。
    public void DestroyRegist(GameObject attackObject)
    {
        clearEvent.AddListener(() => { Destroy(attackObject); });
    }

    //独立している攻撃オブジェクトをすべて破壊する。
    public void AllClear()
    {
        ClawOff();
        PunchOff();
        TailBladeOff();

        PressOff();

        UpperOff();
        SmashOff();

        clearEvent.Invoke();
    }
}
