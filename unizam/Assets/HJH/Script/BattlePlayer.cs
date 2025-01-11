using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : MonoBehaviour
{
    public Slider hpBar;
    public Transform BuffParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = ((float)GameManager.Instance.player.hp / (float)GameManager.Instance.maxHp);
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
