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
        buffText.text = (8 - BattleManager.instance.turn).ToString() + "�� �� õ ���� ����� �����մϴ�.";
    }
}
