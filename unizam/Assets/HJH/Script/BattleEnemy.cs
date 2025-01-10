using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour
{
    public EnemyCharacter enemy;
    public int maxHp;
    public Slider hpBar;
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
            hpBar.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0,1.5f,0));
        }
    }

    public void EnemyDie()
    {
        //Á×À»¶§ ¿¬Ãâ
        gameObject.SetActive(false);
        hpBar.gameObject.SetActive(false);
    }
}
