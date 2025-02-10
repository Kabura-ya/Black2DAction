using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//巨大蜘蛛をアニメーション制御するために使う。
//アニメーターコントローラーを付けたオブジェクトに付ける。
public class GiantSpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] private GiantSpiderAttack giantSpiderAttack = null;
    
    void GuillotineAttack()
    {
        giantSpiderAttack.GenerateGuillotine();
    }

    void WebBeemAttack()
    {
        giantSpiderAttack.GenerateWebBeem();
    }
}
