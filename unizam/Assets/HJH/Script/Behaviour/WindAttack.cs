using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WindAttack", menuName = "Scriptable Object/WindAttack")]
public class WindAttack : Behaviour
{
    public Buff buff;
    public int maxDamage;
    public int minDamage;
    public override void Do(Character[] target)
    {
        BattleManager.instance.PlaySound(2);
        for (int i = 0; i < target.Length; i++)
        {
            int damage = Random.Range(minDamage, maxDamage);
            for (int j = 0; j < character.buffs.Count; j++)
            {
                damage = character.buffs[j].BuffEffect(true,damage);
            }
            for(int k = 0; k < target[i].buffs.Count; k++)
            {
                damage = target[i].buffs[k].BuffEffect(false,damage);
            }
            target[i].hp -= damage;
        }

        character.nextBuffs.Add(buff);
    }
}
