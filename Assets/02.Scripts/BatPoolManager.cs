using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BatPoolManager : MonoBehaviour
{
    [System.Serializable]
    private class ObjectInfo
    {
        public string objectName;  // 오브젝트 이름
        public GameObject prefab;   // 프리팹
        public int count;          // 미리 생성할 개수
    }

    public static BatPoolManager instance; // 싱글톤 인스턴스

    [SerializeField]
    private ObjectInfo[] objectInfos = null; // 오브젝트 정보 배열

    private Dictionary<string, IObjectPool<GameObject>> objectPoolDic = new Dictionary<string, IObjectPool<GameObject>>(); // 오브젝트 풀을 관리할 딕셔너리

    private void Awake()
    {
        // 싱글톤 구현
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        Init(); // 초기화
    }

    private void Init()
    {
        // 각 오브젝트 정보를 기반으로 풀 생성
        foreach (var objectInfo in objectInfos)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                () => Instantiate(objectInfo.prefab), // 오브젝트 생성
                obj => obj.SetActive(true),           // 오브젝트 활성화
                obj => obj.SetActive(false),          // 오브젝트 비활성화
                obj => Destroy(obj),                  // 오브젝트 삭제
                true,                                 // 자동 확장 여부
                objectInfo.count,                     // 최소 크기
                objectInfo.count                      // 최대 크기
            );

            objectPoolDic.Add(objectInfo.objectName, pool); // 딕셔너리에 추가

            // 미리 오브젝트 생성 해놓기
            for (int i = 0; i < objectInfo.count; i++)
            {
                pool.Get(); // 오브젝트 풀에서 가져오기
            }
        }
    }

    // 지정된 개수의 배트를 가져오는 메서드
    public GameObject[] GetBats(int count)
    {
        GameObject[] bats = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            bats[i] = objectPoolDic["Bat"].Get(); // "Bat" 오브젝트 풀에서 가져오기
        }
        return bats; // 가져온 배트 배열 반환
    }
}
