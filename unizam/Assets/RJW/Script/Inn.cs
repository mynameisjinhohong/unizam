using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inn : MonoBehaviour
{
    bool spendNight = false;
    bool enter = false;
    public int heal;

    public GameObject innUI;
    public GameObject popupUI;
    public TMP_Text popupText;

    float temp = 0; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enter) {
            enter = false;
            innUI.SetActive(true);
            temp = GameObject.FindWithTag("Player").GetComponent<Player>().speed;
            GameObject.FindWithTag("Player").GetComponent<Player>().speed = 0;
        }
    }

    public void closeInnUI()
    {
        innUI.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<Player>().speed = temp;
    }

    public void randomEvent() {
        if (spendNight == false)
        {
            int num = Random.Range(0, 3);

            if (GameManager.Instance.player.mp >= 1)
            {
                GameManager.Instance.player.mp -= 1;
                GameManager.Instance.player.hp += heal;

                StartCoroutine(popUp("�� 1�� �Ҹ��Ͽ� ���� �� ���� ü�� " + heal + "�� ȹ���ߴ�."));
                spendNight = true;
            }
            else
            {
                Debug.Log("�Ⱑ �����մϴ�.");
                StartCoroutine(popUp("�Ⱑ ������ �� �� �����١�"));
            }
        
        }
        else {
            Debug.Log("�� �� ���� ����");
            StartCoroutine(popUp("�� �� ���� ����..."));
        }
    }

    IEnumerator popUp(string text) {
        popupText.text = text;
        popupUI.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        popupUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spendNight == false)
        {
            enter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (innUI != null) {
            innUI.SetActive(false);
            enter = false;
        }
    }
}
