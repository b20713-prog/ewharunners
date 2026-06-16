using UnityEngine;

public class UniformManager : MonoBehaviour
{
    // 아이템 종류 정의 (진짜 이화 교복 5종 vs 헷갈리는 사복 함정 5종)
    public enum ItemType
    {
        Blouse, Ribbon, KnitVest, Skirt, Jacket, // 진짜 교복
        CasualShirt, CasualTie, NoLogoVest, GreySweatpants, NavyOuter // 가짜 사복
    }

    [Header("현재 착용 단계 (0:사복 ~ 5:완벽정복)")]
    public int currentStage = 0; 

    [Header("3D 캐릭터 부위별 메쉬 오브젝트")]
    public GameObject casualOutfitMesh; // 최초 묶은 머리 사복 스타팅 룩
    public GameObject blouseMesh;       // 둥근 카라 + 소매 체크
    public GameObject ribbonMesh;       // 초록/남색 체크 리본
    public GameObject knitVestMesh;     // 꽃 모양 로고 + 넥 줄무늬 조끼
    public GameObject skirtMesh;        // 남색 주름치마
    public GameObject jacketMesh;       // 금장 단추 4개 마이

    void Start()
    {
        UpdateCharacterLook(); // 게임 시작 시 무조건 0단계 사복 차림으로 시작
    }

    // 아이템 충돌 시 호출되는 핵심 메커니즘 함수
    public void ProcessPickup(ItemType item)
    {
        // 1. 지정된 순서대로 정확하게 먹었을 때만 옷을 갈아입음
        if (currentStage == 0 && item == ItemType.Blouse) NextStage();
        else if (currentStage == 1 && item == ItemType.Ribbon) NextStage();
        else if (currentStage == 2 && item == ItemType.KnitVest) NextStage();
        else if (currentStage == 3 && item == ItemType.Skirt) NextStage();
        else if (currentStage == 4 && item == ItemType.Jacket) WinGame();

        // 2. 겉모습이 비슷해서 헷갈리는 사복 함정을 먹었을 때 (벌칙 적용)
        else if (CheckIfCasualTrap(item))
        {
            ApplyTrapPenalty();
        }
    }

    private bool CheckIfCasualTrap(ItemType item)
    {
        return item == ItemType.CasualShirt || item == ItemType.CasualTie || 
               item == ItemType.NoLogoVest || item == ItemType.GreySweatpants || item == ItemType.NavyOuter;
    }

    private void NextStage()
    {
        currentStage++;
        UpdateCharacterLook(); // 3D 그래픽 실시간 교복으로 교체
        Debug.Log($"교복 착용 전진! 현재 단계: {currentStage}/5");
    }

    private void ApplyTrapPenalty()
    {
        Debug.LogWarning("사복 적발! 선생님 추격 및 입고 있던 교복 한 단계 탈의!");
        
        // 입고 있던 교복 게이지 한 단계 하락 (사복 상태인 0 밑으로는 안 내려감)
        if (currentStage > 0) currentStage--;
        
        UpdateCharacterLook();

        // [복장 단속 선생님 기믹 트리거]
        // 뒤에서 쫓아오는 선생님의 거리를 좁히는 연출 코드를 여기에 연결합니다.
    }

    // 실시간 3D 스킨 체인지 시스템
    private void UpdateCharacterLook()
    {
        casualOutfitMesh.SetActive(currentStage == 0); // 0단계일 때만 추리닝 사복 활성화
        blouseMesh.SetActive(currentStage >= 1);
        ribbonMesh.SetActive(currentStage >= 2);
        knitVestMesh.SetActive(currentStage >= 3);
        skirtMesh.SetActive(currentStage >= 4);
        jacketMesh.SetActive(currentStage == 5);       // 5단계 최종 정복 완성
    }

    private void WinGame()
    {
        currentStage = 5;
        UpdateCharacterLook();
        Debug.Log("★ 축하합니다! 완벽한 이화여고 정복 완성! 복장 단속을 피해 교실 골인에 성공했습니다! ★");
    }
}
