using UnityEngine;

public class StoreCamera : MonoBehaviour
{
    public float moveSpeed = 10.0f; // ī�޶� �̵� �ӵ�
    private Vector3 targetPosition; // ī�޶� �̵��� ��ǥ ��ġ

    private void Start()
    {
        // �ʱ� ��ǥ ��ġ�� ���� ī�޶��� ��ġ�� ����
        targetPosition = transform.position;
    }

    private void Update()
    {
        // ���� ī�޶��� ��ġ�� ��ǥ ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // ��ư 1 Ŭ�� �� ȣ��� �Լ�
    public void MoveToPosition1()
    {
        Debug.Log("���尣 �̵�");
        targetPosition = new Vector3(0, transform.position.y, transform.position.z);
    }

    // ��ư 2 Ŭ�� �� ȣ��� �Լ�
    public void MoveToPosition2()
    {
        Debug.Log("���°� �̵�");
        targetPosition = new Vector3(17, transform.position.y, transform.position.z);
    }

    // ��ư 3 Ŭ�� �� ȣ��� �Լ�
    public void MoveToPosition3()
    {
        Debug.Log("���� �̵�");
        targetPosition = new Vector3(33, transform.position.y, transform.position.z);
    }
}