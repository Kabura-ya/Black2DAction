using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldKnightEffect : MonoBehaviour
{
    [SerializeField] private GameObject sparkle = null;
    [SerializeField] private GameObject guard = null;
    [SerializeField] private GameObject powerguard = null;
    [SerializeField] private GameObject brake = null;

    [SerializeField] private GameObject brokenGuard = null;
    [SerializeField] private GameObject brokenPowerGuard = null;
    [SerializeField] private GameObject counterSuccess = null;
    [SerializeField] private GameObject powerCounterSuccess = null;

    void Awake()
    {
        SparkleOff();
        GuardOff();
        PowerGuardOff();
        BrakeOff();
    }

    public void SparkleOn()
    {
        sparkle.SetActive(true);
    }
    public void SparkleOff()
    {
        sparkle.SetActive(false);
    }

    public void CounterSuccess()
    {
        Instantiate(counterSuccess, this.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0f, 0f, 0f));
    }
    public void PowerCounterSuccess()
    {
        Instantiate(powerCounterSuccess, this.transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0f, 0f, 0f));
    }

    public void GuardOn()
    {
        guard.SetActive(true);
    }
    public void GuardOff()
    {
        guard.SetActive(false);
    }
    public void BrokenGuard()
    {
        Instantiate(brokenGuard, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
    }

    public void PowerGuardOn()
    {
        powerguard.SetActive(true);
    }
    public void PowerGuardOff()
    {
        powerguard.SetActive(false);
    }
    public void BrokenPowerGuard()
    {
        Instantiate(brokenPowerGuard, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
    }

    public void BrakeOn()
    {
        brake.SetActive(true);
    }
    public void BrakeOff()
    {
        brake.SetActive(false);
    }

    public void AllClear()
    {
        SparkleOff();
        GuardOff();
        PowerGuardOff();
        BrakeOff();
    }
}
