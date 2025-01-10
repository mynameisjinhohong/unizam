using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : MonoBehaviour
{
    bool isVisit = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Ŭ���� ó�� �߻��� �������� ����
        if (Input.GetKeyDown(KeyCode.Space) && isVisit == true)
        {
            int num = Random.Range(0, 3);

            if (num == 0)
            {
                Debug.Log("ü�� ȸ��");
            }
            else if (num == 1)
            {
                Debug.Log("ü�� ����");
            }
            else if (num == 2)
            {
                Debug.Log("���� ȸ��");
            }
            this.GetComponent<Inn>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�湮");
            isVisit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("����");
        isVisit = false;
    }
}
