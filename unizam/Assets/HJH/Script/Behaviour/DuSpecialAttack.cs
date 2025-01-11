using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DuSpecialAttack", menuName = "Scriptable Object/DuSpecialAttack")]
public class DuSpecialAttack : Behaviour
{
    public EnemeyData duData;
    public bool wait = false;
    public GameObject effect;
    public GameObject effect2;

    private void OnEnable()
    {
        wait = true;
    }

    public override void Do(Character[] target)
    {
        if (wait)
        {
            wait = false;
        }
        else
        {
            BattleManager.instance.audioPlay.clip = BattleManager.instance.audios[4];
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
                int damage = (duData.enemy.hp -character.hp)*5;
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
            wait = true;
        }
    }
}
