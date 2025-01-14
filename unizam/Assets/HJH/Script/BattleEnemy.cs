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
    public Transform HPCan;
    public GameObject HpCanImage;
    bool start;
    public bool destroy = false;
    public GameObject dieEffect1;
    public GameObject dieEffect2;
    public GameObject skillCanvas;
    bool die = false;
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
                if(HpCanImage != null)
                {
                    HPCan = hpBar.transform.GetChild(3);
                    for (int i = 0; i < enemy.hp / 3; i++)
                    {
                        Instantiate(HpCanImage, HPCan);
                    }
                }
                start= false;
            }
        }
        else
        {
            hpBar.value = ((float)enemy.hp/ (float)maxHp);
            if(hpBar.value <= 0 && !die)
            {
                EnemyDie();
                die = true;
            }
            if(hpPos!= null)
            {
                hpBar.transform.position = Camera.main.WorldToScreenPoint(hpPos.position);
            }
        }
    }

    public void SkillCanvasGo()
    {
        StartCoroutine(SkillCanvasOn());
    }
    IEnumerator SkillCanvasOn()
    {
        skillCanvas.SetActive(true);
        yield return new WaitForSeconds(1);
        skillCanvas.SetActive(false);
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
        if(dieEffect1!= null)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        BattleManager.instance.PlaySound(10);
        dieEffect1.SetActive(true);
        yield return new WaitForSeconds(0.35f);
        dieEffect2.SetActive(true);
        yield return new WaitForSeconds(1f);
        if (destroy)
        {
            hpBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
