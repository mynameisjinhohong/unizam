using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public bool on = false;
    public Button skillButton;
    public GameObject[] skills;
    public GameObject[] skillDescribe;
    public Sprite question;
    public void Start()
    {
        OffSkillButton();
        on = false;
        for(int j = 0; j<skills.Length; j++)
        {
            bool find = false;
            for (int i = 0; i < GameManager.Instance.player.behaviours.Count; i++)
            {
                if(GameManager.Instance.player.behaviours[i].mpIdx == j)
                {
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                skills[j].transform.GetChild(0).GetComponent<Image>().sprite = question;
            }
        }
        
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

    public void MouseHover(GameObject bto)
    {
        for (int i = 0; i < skillDescribe.Length; i++)
        {
            skillDescribe[i].SetActive(false);
        }
        if (bto.GetComponent<Button>().interactable)
        {
            bto.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
    public void MosetHoverOut()
    {
        for (int i = 0; i < skillDescribe.Length; i++)
        {
            skillDescribe[i].SetActive(false);
        }
    }
}
