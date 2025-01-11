using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour
{
    public EnemyCharacter enemy;
    public Transform hpPos;
    public int maxHp;
    public Slider hpBar;
    public Transform BuffParent;
    bool start;
    // Start is called before the first frame update
    void Start()
    {
        start= true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if(enemy != null)
            {
                maxHp = enemy.hp;
                for(int i = 0; enemy.behaviours.Count < 0; i++)
                {
                    enemy.behaviours[i].character = enemy;
                }
                start= false;
            }
        }
        else
        {
            hpBar.value = ((float)enemy.hp/ (float)maxHp);
            if(hpBar.value <= 0)
            {
                EnemyDie();
            }
            if(hpPos!= null)
            {
                hpBar.transform.position = Camera.main.WorldToScreenPoint(hpPos.position);
            }
        }
    }

    public void MakeBuff()
    {
        for(int i = BuffParent.childCount-1; i>=0; i--)
        {
            Destroy(BuffParent.GetChild(i).gameObject);
        }
        for(int i =0; i<enemy.buffs.Count; i++)
        {
            GameObject buff = Instantiate(enemy.buffs[i].buffIcon, BuffParent);
        }
    }

    public void EnemyDie()
    {
        //Á×À»¶§ ¿¬Ãâ
        gameObject.SetActive(false);
        hpBar.gameObject.SetActive(false);
    }
}
