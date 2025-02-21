using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//巨大蜘蛛をアニメーション制御するために使う。
//アニメーターコントローラーを付けたオブジェクトに付ける。
public class GiantSpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    [SerializeField] private GameObject web = null;
    [SerializeField] private GameObject brake = null;

    void Awake()
    {
        WebOff();
        BrakeOff();
    }

    void TackleOn()
    {
        giantSpiderAttack.TackleOn();
    }
    void TackleOff()
    {
        giantSpiderAttack.TackleOff();
    }
    void BrakeOn()
    {
        brake.SetActive(true);
    }
    void BrakeOff()
    {
        brake.SetActive(false);
    }

    void GuillotineAttack()
    {
        giantSpiderAttack.GenerateGuillotine();
    }

    void WebBeemAttack()
    {
        giantSpiderAttack.GenerateWebBeem();
    }

    void WebOn()
    {
        web.SetActive(true);
    }
    void WebOff()
    {
        web.SetActive(false);
    }

    void PreDead()
    {
        WebOff();
        TackleOff();
        BrakeOff();
        giantSpiderAttack.AllClear();
    }
    void Dead()
    {
        Destroy(transform.root.gameObject);
    }
}
