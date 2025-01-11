using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuSpecialAttack", menuName = "Scriptable Object/GuSpecialAttack")]
public class GuSpecialAttack : Behaviour
{
    public int maxDamage;
    public int minDamage;
    public GameObject effect;
    public GameObject effect2;
    public override void Do(Character[] target)
    {
        BattleManager.instance.audioPlay.clip = BattleManager.instance.audios[5];
        BattleManager.instance.audioPlay.Play();
        if(unit.name == "Player")
        {
            Instantiate(effect2);
        }
        else
        {
            Instantiate(effect);
        }
        for (int i = 0; i < target.Length; i++)
        {
            int damage = Random.Range(minDamage, maxDamage);
            for (int j = 0; j < character.buffs.Count; j++)
            {
                damage = character.buffs[j].BuffEffect(true, damage);
            }
            for (int k = 0; k < target[i].buffs.Count; k++)
            {
                damage = target[i].buffs[k].BuffEffect(false, damage);
            }
            target[i].hp -= damage;
            character.hp += damage / 2;
        }
    }
}
