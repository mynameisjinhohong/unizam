using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DragonBuff", menuName = "Scriptable Object/DragonBuff")]
public class DragonBuff : Buff
{
    public int turn;
    private void OnEnable()
    {
        turn = 0;
    }
    public override int BuffEffect(bool mybuff, int su)
    {
        if(mybuff)
        {
            turn++;
        }
        return su;
    }
}
