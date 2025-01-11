using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNomalAttack", menuName = "Scriptable Object/PlayerNomalAttack")]
public class PlayerNomalAttack : Behaviour
{
    public int MaxDamage;
    public int MinDamage;
    public override void Do(Character[] target)
    {
        int damage = Random.Range(MinDamage, MaxDamage);
        BattleManager.instance.audioPlay.clip = BattleManager.instance.audios[1];
        BattleManager.instance.audioPlay.Play();
        for (int i = 0; i < character.buffs.Count; i++)
        {
            damage = character.buffs[i].BuffEffect(true, damage);
        }
        for (int k = 0; k < target[0].buffs.Count; k++)
        {
            damage = target[0].buffs[k].BuffEffect(false, damage);
        }
        target[0].hp -= damage;
    }
}
