using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    bool on = false;
    public Button skillButton;
    public GameObject[] skills;

    public void Start()
    {
        OffSkillButton();
        on = false;
    }

    public void SkillBto()
    {
        if (on)
        {
            OffSkillButton();
        }
        else
        {
            OnSkillButton();
        }
    }


    public void OnSkillButton()
    {
        on = true;
        for(int i =0; i< GameManager.Instance.player.behaviours.Count; i++)
        {
            if(GameManager.Instance.player.behaviours[i].mpIdx < GameManager.Instance.player.mp)
            {
                skills[GameManager.Instance.player.behaviours[i].mpIdx].GetComponent<Button>().interactable = true;
            }
        }
        for (int i = 0; i< skills.Length; i++)
        {
            skills[i].SetActive(true);
        }

    }

    public void OffSkillButton()
    {
        on = false;
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetActive(false);
            skills[i].GetComponent<Button>().interactable = false;
        }
        BattleManager.instance.State = BattleState.StartDice;
    }
}
