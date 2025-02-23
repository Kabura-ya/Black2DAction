using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightEffect : MonoBehaviour
{
    [SerializeField] private GameObject sparkle = null;
    [SerializeField] private GameObject guard = null;
    [SerializeField] private GameObject powerguard = null;

    void Awake()
    {
        SparkleOff();
        GuardOff();
        PowerGuardOff();
    }

    public void SparkleOn()
    {
        sparkle.SetActive(true);
    }
    public void SparkleOff()
    {
        sparkle.SetActive(false);
    }

    public void GuardOn()
    {
        guard.SetActive(true);
    }
    public void GuardOff()
    {
        guard.SetActive(false);
    }

    public void PowerGuardOn()
    {
        powerguard.SetActive(true);
    }
    public void PowerGuardOff()
    {
        powerguard.SetActive(false);
    }

    public void AllClear()
    {
        SparkleOff();
        GuardOff();
        PowerGuardOff();
    }
}
