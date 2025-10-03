using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantSpiderEffect : MonoBehaviour
{
    [SerializeField] private GameObject web = null;
    [SerializeField] private GameObject brake = null;

    void Awake()
    {
        WebOff();
        BrakeOff();
    }

    public void BrakeOn()
    {
        brake.SetActive(true);
    }
    public void BrakeOff()
    {
        brake.SetActive(false);
    }

    public void WebOn()
    {
        web.SetActive(true);
    }
    public void WebOff()
    {
        web.SetActive(false);
    }

    public void AllClear()
    {
        WebOff();
        BrakeOff();
    }
}
