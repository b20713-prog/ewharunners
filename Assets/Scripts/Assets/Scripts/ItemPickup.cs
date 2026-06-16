using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // 이 아이템이 진짜 교복인지 가짜 사복인지 인스펙터에서 고를 수 있게 매핑
    public UniformManager.ItemType itemType;

    [Header("공중에 둥둥 뜨는 회전 이펙트")]
    public float rotateSpeed = 90.0f;
    public float floatSpeed = 2.0f;
    public float floatHeight = 0.2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // 3D 아이템이 공중에서 스스로 회전하고 둥둥 떠다니는 연출 (시인성 확보)
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = tempPos;
    }

    // 플레이어(묶은 머리 학생)가 자석 없이 몸으로 직접 부딪혔을 때 발동
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UniformManager manager = other.GetComponent<UniformManager>();
            if (manager != null)
            {
                manager.ProcessPickup(itemType); // 매니저에게 어떤 옷을 먹었는지 전달
            }
            
            Destroy(gameObject); // 먹은 아이템은 3D 맵에서 삭제
        }
    }
}
