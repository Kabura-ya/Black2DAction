using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainTrue : MonoBehaviour, IDrainable
{
    // Start is called before the first frame update
    //常にドレイン可能なものにつける用、Drain()関数でtrueを返すだけ
    public bool Drain()
    {
        return true;
    }
}
