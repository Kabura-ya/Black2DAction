using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldKnightAttack : MonoBehaviour
{
    [SerializeField] private ShieldKnightStatus shieldKnightStatus = null;

    [SerializeField] private GameObject slash = null;
    [SerializeField] private GameObject powerslash = null;

    [SerializeField] private GameObject assault = null;
    [SerializeField] private GameObject powerassault = null;

    [SerializeField] private GameObject counter = null;
    [SerializeField] private GameObject powercounter = null;

    [SerializeField] private GameObject surroundspear = null;
    [SerializeField] private GameObject upperspear = null;
    [SerializeField] private GameObject smashspear = null;

    public UnityEvent clearEvent;

    void Awake()
    {
        SlashOff();
        PowerSlashOff();

        AssaultOff();
        PowerAssaultOff();

        CounterOff();
        PowerCounterOff();
    }

    public void SlashOn()
    {
        slash.SetActive(true);
    }
    public void SlashOff()
    {
        slash.SetActive(false);
    }
    public void PowerSlashOn()
    {
        powerslash.SetActive(true);
    }
    public void PowerSlashOff()
    {
        powerslash.SetActive(false);
    }

    public void AssaultOn()
    {
        assault.SetActive(true);
    }
    public void AssaultOff()
    {
        assault.SetActive(false);
    }
    public void PowerAssaultOn()
    {
        powerassault.SetActive(true);
    }
    public void PowerAssaultOff()
    {
        powerassault.SetActive(false);
    }

    public void CounterOn()
    {
        counter.SetActive(true);
    }
    public void CounterOff()
    {
        counter.SetActive(false);
    }
    public void PowerCounterOn()
    {
        powercounter.SetActive(true);
    }
    public void PowerCounterOff()
    {
        powercounter.SetActive(false);
    }

    public void GenerateSurroundSpear()
    {
        GameObject generated = null;
        if (shieldKnightStatus.PlayerTrans == null)
        {
            generated = Instantiate(surroundspear, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            generated = Instantiate(surroundspear, shieldKnightStatus.PlayerTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
        DestroyRegist(generated);
    }
    public void GenerateUpperSpear()
    {
        GameObject generated = null;
        if (shieldKnightStatus.PlayerTrans == null)
        {
            generated = Instantiate(upperspear, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            generated = Instantiate(upperspear, shieldKnightStatus.PlayerTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
        DestroyRegist(generated);
    }
    public void GenerateSmashSpear()
    {
        GameObject generated = null;
        if (shieldKnightStatus.PlayerTrans == null)
        {
            generated = Instantiate(smashspear, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            generated = Instantiate(smashspear, shieldKnightStatus.PlayerTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
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
        SlashOff();
        PowerSlashOff();

        AssaultOff();
        PowerAssaultOff();

        CounterOff();
        PowerCounterOff();

        clearEvent.Invoke();
    }
}
