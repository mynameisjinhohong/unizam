using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MangBuff", menuName = "Scriptable Object/MangBuff")]
public class MangBuff : Buff
{
    public int divide;
    public override int BuffEffect(bool mybuff,int su)
    {
        if (!mybuff)
        {
            return su;
        }
        int real = su / divide;
        return real;
    }
}
