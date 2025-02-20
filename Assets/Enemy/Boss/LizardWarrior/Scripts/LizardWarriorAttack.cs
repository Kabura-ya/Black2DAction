using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardWarriorAttack : MonoBehaviour
{
    [SerializeField] private Transform slashwaveShotTrans = null;
    [SerializeField] private GameObject slashwave = null;
    [SerializeField] private GameObject upper = null;

    void Awake()
    {
        UpperOff();
    }

    public void UpperOn()
    {
        upper.SetActive(true);
    }
    public void UpperOff()
    {
        upper.SetActive(false);
    }

    public void GenerateSlashWave()
    {
        if (this.transform.localScale.x > 0)
        {
            Instantiate(slashwave, slashwaveShotTrans.position, Quaternion.Euler(0f, 0f, 0f));
        }
        else
        {
            Instantiate(slashwave, slashwaveShotTrans.position, Quaternion.Euler(0f, 0f, 180f));
        }
    }
}
