using UnityEngine;

public class BuffMove : MonoBehaviour
{
    void Start()
    {

        // ȭ�� �ȼ� ��ǥ�� ����Ʈ ��ǥ(0~1)�� ��ȯ
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(transform.position);
        // ȭ���� ���ʿ� �ִ� ��� (����Ʈ ��ǥ�� x ���� 0.5���� ����)
        if (viewportPos.x > 0.5f)
        {
            // �ڽ� ������Ʈ�� ��ġ ����
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, 0);
        }
        else
        {

            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
    }
}
