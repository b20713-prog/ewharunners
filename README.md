# ewharunners
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentLane = 1; // 0: 왼쪽, 1: 가운데, 2: 오른쪽
    private float laneDistance = 2.0f; // 레인 간격

    [Header("이동 속도 및 점프력")]
    public float forwardSpeed = 7.0f;
    public float jumpForce = 6.0f;
    private float verticalVelocity;
    private float gravity = 14.0f;

    private CharacterController controller;
    private Vector3 targetPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        // 1. 앞으로 자동 전진 (묶은 머리가 휘날리며 질주)
        targetPosition.z += forwardSpeed * Time.deltaTime;

        // 2. 좌우 라인 변경 입력 (A, D 및 방향키)
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0) currentLane--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 2) currentLane++;
        }

        // 3. 3D 점프 및 중력 계산 (Space키)
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 목표 X 좌표 계산 (자석 기능 없이 조작한 라인으로만 이동)
        float targetX = (currentLane - 1) * laneDistance;
        Vector3 moveVector = new Vector3(targetX - transform.position.x, verticalVelocity * Time.deltaTime, forwardSpeed * Time.deltaTime);
        
        controller.Move(moveVector);
    }
}
