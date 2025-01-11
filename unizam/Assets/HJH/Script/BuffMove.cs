using UnityEngine;

public class BuffMove : MonoBehaviour
{
    void Start()
    {
        // UI ����� ���� ��ǥ�� ȭ�� ����Ʈ ��ǥ�� ��ȯ
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(gameObject.GetComponent<RectTransform>().position);

        // ȭ�� ���ʿ� �ִ� ��� (����Ʈ ��ǥ�� x ���� 0.5���� ����)
        if (viewportPos.x > 0.5f)
        {
            // �ڽ� ������Ʈ�� ��ġ ����
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-240, 0);
        }
    }
}
