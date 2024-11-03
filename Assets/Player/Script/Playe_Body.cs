using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playe_Body : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public void Damage(int damage)
    {
        transform.root.gameObject.GetComponent<Player>().Damage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)//Body部分に触れたらそのまま親オブジェクトのスクリプトに情報を送る
    {
        Debug.Log("Player_OnTrigger");
        transform.root.gameObject.GetComponent<Player>().BodyEnter(collision);
    }
}
