using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BatPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName;  // ������Ʈ �̸�
        public GameObject prefab;   // ������
        public int count;          // �̸� ������ ����
    }

    public static BatPoolManager instance; // �̱��� �ν��Ͻ�

    [SerializeField]
    private ObjectInfo[] objectInfos = null; // ������Ʈ ���� �迭

    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>(); // ������Ʈ Ǯ�� ������ ��ųʸ�

    private void Awake()
    {
        // �̱��� ����
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init(); // �ʱ�ȭ
    }

    private void Init()
    {
        // �� ������Ʈ ������ ������� Ǯ ����
        foreach (var objectInfo in objectInfos)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => Instantiate(objectInfo.prefab), // ������Ʈ ����
                obj => obj.SetActive(true),           // ������Ʈ Ȱ��ȭ
                obj => obj.SetActive(false),          // ������Ʈ ��Ȱ��ȭ
                obj => Destroy(obj),                  // ������Ʈ ����
                true,                                 // �ڵ� Ȯ�� ����
                objectInfo.count,                     // �ּ� ũ��
                objectInfo.count                      // �ִ� ũ��
            );

            objectPoolDic.Add(objectInfo.objectName, pool); // ��ųʸ��� �߰�

            // �̸� ������Ʈ ���� �س���
            for (int i = 0; i < objectInfo.count; i++)
            {
                pool.Get(); // ������Ʈ Ǯ���� ��������
            }
        }
    }

    // ������ ������ ��Ʈ�� �������� �޼���
    public GameObject[] GetBats(int count)
    {
        GameObject[] bats = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            bats[i] = objectPoolDic["Bat"].Get(); // "Bat" ������Ʈ Ǯ���� ��������
        }
        return bats; // ������ ��Ʈ �迭 ��ȯ
    }
}
