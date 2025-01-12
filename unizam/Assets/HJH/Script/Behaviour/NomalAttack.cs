using UnityEngine;

[CreateAssetMenu(fileName = "NomalAttack", menuName = "Scriptable Object/NomalAttack")]
public class NomalAttack : Behaviour
{
    public int MaxDamage;
    public int MinDamage;
    public override void Do(Character[] target)
    {
        Animator ani;
        if(unit.TryGetComponent<Animator>(out ani))
        {
            ani.SetTrigger("Attack");
        }
        BattleManager.instance.PlaySound(6);
        int damage = Random.Range(MinDamage, MaxDamage);
        for(int i = 0; i < character.buffs.Count; i++)
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