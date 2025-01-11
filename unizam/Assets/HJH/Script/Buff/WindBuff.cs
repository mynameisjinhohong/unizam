using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindBuff", menuName = "Scriptable Object/WindBuff")]
public class WindBuff : Buff
{
    public int divide;
    public override int BuffEffect(bool myBuff,int su)
    {
        if (myBuff)
        {
            return su;
        }
        if(su < 0)
        {
            return su;
        }
        int real = su / divide;
        return real;
    }
}
