using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SnakeAttack", menuName = "Scriptable Object/SnakeAttack")]
public class SnakeAttack : Behaviour
{
    public int turn;
    public int nomalPercent;
    public int MinDamage;
    public int MaxDamage;
    public int fireDamage;
    public Buff buff;
    bool fire = false;
    public string[] skillName;
    public GameObject effect;

    private void OnEnable()
    {
        fire = false;
        turn = 0;
    }

    public override void Do(Character[] target)
    {
        int ran = Random.Range(0, 100);
        unit.transform.GetChild(0).gameObject.SetActive(false);
        turn += 1;
        if(turn >= 10)
        {
            BattleManager.instance.PlaySound(8);
            BattleManager.instance.DragonPower();
        }
        else
        {
            if (fire)
            {
                Instantiate(effect);
                BattleManager.instance.PlaySound(9);
                int damage = Random.Range(fireDamage - 2, fireDamage + 3);
                for (int i = 0; i < character.buffs.Count; i++)
                {
                    damage = character.buffs[i].BuffEffect(true, damage);
                }
                for (int k = 0; k < target[0].buffs.Count; k++)
                {
                    damage = target[0].buffs[k].BuffEffect(false, damage);
                }
                target[0].hp -= damage;
                fire = false;
            }
            else
            {
                if (ran < nomalPercent)
                {
                    BattleManager.instance.PlaySound(6);
                    int damage = Random.Range(MinDamage, MaxDamage);
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
                else
                {
                    BattleManager.instance.PlaySound(7);
                    fire = true;
                    character.nextBuffs.Add(buff);
                    unit.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

}
