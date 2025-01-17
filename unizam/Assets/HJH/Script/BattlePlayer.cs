using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : MonoBehaviour
{
    public GameObject[] mp;
    public Slider hpBar;
    public Transform BuffParent;
    public Transform HPCan;
    public GameObject HpCanImage;
    // Start is called before the first frame update
    void Start()
    {
        HPCan = hpBar.transform.GetChild(3);
        for (int i = 0; i < GameManager.Instance.player.hp / 3; i++)
        {
            Instantiate(HpCanImage, HPCan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = ((float)GameManager.Instance.player.hp / (float)GameManager.Instance.maxHp);
        for(int i =0; i < mp.Length; i++)
        {
            if (i < GameManager.Instance.player.mp)
            {
                mp[i].SetActive(true);
            }
            else
            {
                mp[i].SetActive(false);
            }
        }
    }
    public void MakeBuff()
    {
        for (int i = BuffParent.childCount - 1; i >= 0; i--)
        {
            Destroy(BuffParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < GameManager.Instance.player.buffs.Count; i++)
        {
            GameObject buff = Instantiate(GameManager.Instance.player.buffs[i].buffIcon, BuffParent);
        }
    }
}
