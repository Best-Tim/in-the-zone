using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSetter : MonoBehaviour
{
    public CoinEnum eCoinEnum;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public CoinEnum CoinEnumGet()
    {
        return eCoinEnum;
    }
}

