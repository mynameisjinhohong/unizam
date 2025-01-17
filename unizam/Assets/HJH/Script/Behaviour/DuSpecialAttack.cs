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
    public int hp;

    private void OnEnable()
    {
        wait = true;
        hp = character.hp;
    }

    public override void Do(Character[] target)
    {
        if (wait)
        {
            wait = false;
        }
        else
        {
            BattleManager.instance.PlaySound(4);
            if (unit.name == "Player")
            {
                Instantiate(effect2);
            }
            else
            {
                Instantiate(effect);
            }
            for (int i = 0; i < target.Length; i++)
            {
                int damage = (hp -character.hp)*5;
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
            unit.GetComponent<BattleEnemy>().SkillCanvasGo();
        }
        hp = character.hp;
    }


}
