using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainSuperTrue : MonoBehaviour, IDrainable
{
    // Start is called before the first frame update
    //赤攻撃など、常にスーパーダッシュでドレイン可能なものにつける用、Drain()関数でtrueを返すだけ
    public bool Drain()
    {
        return false;
    }

    public bool SuperDrain()
    {
        return true;
    }
}

