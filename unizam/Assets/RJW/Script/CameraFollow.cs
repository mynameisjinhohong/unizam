using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public List<RectTransform> backgroundUI; // ��� UI ����Ʈ

    private Vector3 offset; // �÷��̾�� ī�޶��� �ʱ� ����
    private Vector3 previousPlayerPosition; // �÷��̾��� ���� ��ġ

    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.position;
            previousPlayerPosition = player.position;
        }
    }

    void LateUpdate()
    {
        //if (player != null)
        //{
        //    // ī�޶� ��ġ ����
        //    transform.position = player.position + offset;

        //    // �÷��̾� �̵��� ���
        //    Vector3 playerDelta = player.position - previousPlayerPosition;

        //    // ��� UI �̵�
        //    foreach (RectTransform bg in backgroundUI)
        //    {
        //        if (bg != null)
        //        {
        //            bg.position -= playerDelta; // ��� UI�� �÷��̾� �̵�����ŭ �̵�
        //        }
        //    }

        //    // �÷��̾��� ���� ��ġ ����
        //    previousPlayerPosition = player.position;
        //}
    }
}
