using UnityEngine;

[CreateAssetMenu(fileName = "NomalAttack", menuName = "Scriptable Object/NomalAttack")]
public class NomalAttack : Behaviour
{
    public int MaxDamage;
    public int MinDamage;
    public override void Do(Character[] target)
    {
        int damage = Random.Range(MaxDamage, MinDamage);
        target[0].hp -= damage;
    }
}