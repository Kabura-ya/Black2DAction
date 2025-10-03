using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Drain : MonoBehaviour
{
    // Start is called before the first frame update
    //プレイヤーのドレイン用コライダーにつけるスクリプト
    //ドレイン用コライダーはダッシュ中にのみ有効化される
    //触れたものをそのままプレイヤーに送るだけ

    private void OnTriggerEnter2D(Collider2D collision)//Body部分に触れたものの情報を、そのまま親オブジェクトのPlayerスクリプトに送るだけ
    {
        Debug.Log("Player_Drain_OnTrigger");
        transform.root.gameObject.GetComponent<Player>().DrainEnter(collision);
    }
}
