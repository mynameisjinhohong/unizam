using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DragonBuffIcon : MonoBehaviour
{
    public TMP_Text buffText;

    // Update is called once per frame
    void Update()
    {
        buffText.text = (8 - BattleManager.instance.turn).ToString() + "턴 후 천 년의 응어리가 폭발합니다.";
    }
}
