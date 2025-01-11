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

    private void OnEnable()
    {
        fire = false;
        turn = 0;
    }

    public override void Do(Character[] target)
    {
        int ran = Random.Range(0, 100);
        turn += 1;
        if(turn >= 8)
        {
            target[0].hp -= 9999;
        }
        else
        {
            if (fire)
            {
                int damage = Random.Range(fireDamage - 2, fireDamage + 3);
                Debug.Log(damage);
                for (int i = 0; i < character.buffs.Count; i++)
                {
                    damage = character.buffs[i].BuffEffect(true, damage);
                }
                for (int k = 0; k < target[0].buffs.Count; k++)
                {
                    damage = target[0].buffs[k].BuffEffect(false, damage);
                }
                Debug.Log(damage);
                target[0].hp -= damage;
                fire = false;
            }
            else
            {
                Debug.Log("기본공격");
                if (ran < nomalPercent)
                {
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
                    fire = true;
                    character.nextBuffs.Add(buff);
                }
            }
        }
    }

}
