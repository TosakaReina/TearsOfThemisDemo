using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public Transform player1; // ���1��Transform
    public Transform player2; // ���2��Transform
    public CameraFollow cameraFollow; // ���������ű�

    private Transform currentPlayer; // ��ǰ��ҵ�Transform
    private int currentPlayerIndex = 1; // ��ǰ��ҵ�����



    void Start()
    {
        // ��ʼ����ǰ���Ϊ���1
        currentPlayer = player1;
        cameraFollow.SetTarget(currentPlayer); // �����������ʼĿ��
        EnablePlayerComponents(currentPlayer); // ���õ�ǰ��ҵ����
        DisablePlayerComponents(player2); // ���÷ǵ�ǰ��ҵ����
    }

    void Update()
    {
        // ��ⰴ���л����
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchPlayer();
        }

        // toggle current player's movement script
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            TogglePlayerMovement();
        }
    }

    private void TogglePlayerMovement()
    {
        PlayerMovement playerMovementScript = currentPlayer.GetComponent<PlayerMovement>();
        playerMovementScript.enabled = !playerMovementScript.enabled;
    }

    private void SwitchPlayer()
    {
        if (currentPlayerIndex == 1)
        {
            DisablePlayerComponents(currentPlayer); // ���õ�ǰ��ҵ����
            currentPlayer = player2;
            currentPlayerIndex = 2;
        }
        else
        {
            DisablePlayerComponents(currentPlayer); // ���õ�ǰ��ҵ����
            currentPlayer = player1;
            currentPlayerIndex = 1;
        }

        // ��������ҵ����
        EnablePlayerComponents(currentPlayer);
        // ����������ĸ���Ŀ��
        cameraFollow.SetTarget(currentPlayer);
    }

    private void EnablePlayerComponents(Transform player)
    {
        // ������ҵ��ƶ������
        var movementComponent = player.GetComponent<PlayerMovement>(); // ������ҵ��ƶ������ΪPlayerMovement
        if (movementComponent != null)
        {
            movementComponent.enabled = true;
        }
        // ��������������Ҫ���õ����
    }

    private void DisablePlayerComponents(Transform player)
    {
        // ������ҵ��ƶ������
        var movementComponent = player.GetComponent<PlayerMovement>(); // ������ҵ��ƶ������ΪPlayerMovement
        if (movementComponent != null)
        {
            movementComponent.enabled = false;
        }
        // ���Խ���������Ҫ���õ����
    }
}
