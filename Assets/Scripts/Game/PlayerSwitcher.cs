using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public Transform player1; // 玩家1的Transform
    public Transform player2; // 玩家2的Transform
    public CameraFollow cameraFollow; // 摄像机跟随脚本

    private Transform currentPlayer; // 当前玩家的Transform
    private int currentPlayerIndex = 1; // 当前玩家的索引



    void Start()
    {
        // 初始化当前玩家为玩家1
        currentPlayer = player1;
        cameraFollow.SetTarget(currentPlayer); // 设置摄像机初始目标
        EnablePlayerComponents(currentPlayer); // 启用当前玩家的组件
        DisablePlayerComponents(player2); // 禁用非当前玩家的组件
    }

    void Update()
    {
        // 检测按键切换玩家
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
            DisablePlayerComponents(currentPlayer); // 禁用当前玩家的组件
            currentPlayer = player2;
            currentPlayerIndex = 2;
        }
        else
        {
            DisablePlayerComponents(currentPlayer); // 禁用当前玩家的组件
            currentPlayer = player1;
            currentPlayerIndex = 1;
        }

        // 启用新玩家的组件
        EnablePlayerComponents(currentPlayer);
        // 更新摄像机的跟随目标
        cameraFollow.SetTarget(currentPlayer);
    }

    private void EnablePlayerComponents(Transform player)
    {
        // 启用玩家的移动组件等
        var movementComponent = player.GetComponent<PlayerMovement>(); // 假设玩家的移动组件名为PlayerMovement
        if (movementComponent != null)
        {
            movementComponent.enabled = true;
        }
        // 可以启用其他需要启用的组件
    }

    private void DisablePlayerComponents(Transform player)
    {
        // 禁用玩家的移动组件等
        var movementComponent = player.GetComponent<PlayerMovement>(); // 假设玩家的移动组件名为PlayerMovement
        if (movementComponent != null)
        {
            movementComponent.enabled = false;
        }
        // 可以禁用其他需要禁用的组件
    }
}
