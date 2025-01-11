using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // �÷��̾� ��ü
    public Vector3 offset;    // ī�޶�� �÷��̾� ������ ������
    public List<Transform> backgroundImages;  // ��� �̹��� UI ����Ʈ

    private Vector3 lastPosition;  // ī�޶��� ������ ��ġ (��� �̵��� ����)

    void Start()
    {
        // ī�޶� �÷��̾��� �ڽ����� ����
        transform.SetParent(player);

        // ó�� ī�޶��� ��ġ ���
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        // ī�޶� ��ġ�� �÷��̾��� ��ġ + ���������� ����
        transform.localPosition = offset;

        // ��� �̹����� �Բ� �����̵��� ����
        MoveBackground();

        // ī�޶��� ������ ��ġ ������Ʈ
        lastPosition = transform.position;
    }

    // ��� �̹����� ī�޶�� �Բ� �����̰� �ϴ� �Լ�
    void MoveBackground()
    {
        foreach (Transform background in backgroundImages)
        {
            // ��� ��ġ�� ī�޶��� �̵�����ŭ �̵���Ŵ
            Vector3 backgroundMove = transform.position - lastPosition;
            background.localPosition += backgroundMove;
        }
    }
}
