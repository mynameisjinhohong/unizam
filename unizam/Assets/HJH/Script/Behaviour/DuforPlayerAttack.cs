using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuforPlayerAttack", menuName = "Scriptable Object/DuforPlayerAttack")]
public class DuforPlayerAttack : Behaviour
{
    public GameObject effect;
    public override void Do(Character[] target)
    {
        Instantiate(effect);
        BattleManager.instance.PlaySound(4);
        for (int i = 0; i < target.Length; i++)
        {
            int damage = BattleManager.instance.beforeDamage * 2;
            for (int j = 0; j < character.buffs.Count; j++)
            {
                damage = character.buffs[j].BuffEffect(true, damage);
            }
            for (int k = 0; k < target[i].buffs.Count; k++)
            {
                damage = target[i].buffs[k].BuffEffect(false, damage);
            }
            target[i].hp -= damage;
        }
    }
}
