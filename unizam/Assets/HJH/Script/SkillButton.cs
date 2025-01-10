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
        for (int i = 0; i < skills.Length; i++)
        {
            if (!(i < GameManager.Instance.player.behaviours.Count && i < GameManager.Instance.player.mp))
            {
                skills[i].GetComponent<Button>().interactable = false;
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
            skills[i].GetComponent<Button>().interactable = true;
        }
        BattleManager.instance.State = BattleState.StartDice;
    }
}
