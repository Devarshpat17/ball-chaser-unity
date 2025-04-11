using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Game State Tracking")]
    public int playerResetCount = 0;
    public int domeBounceCount = 0;
    public int enemyCollisionCount = 0;
    public DateTime gameStartTime;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        gameStartTime = DateTime.Now;
        LogGameStart();
    }

    public void LogGameStart()
    {
        Debug.Log($"[GameManager] Game Started at: {gameStartTime}");
        LogGameConfiguration();
    }

    private void LogGameConfiguration()
    {
        Debug.Log("[GameManager] Initial Game Configuration:");
        Debug.Log($"- Resolution: {Screen.width}x{Screen.height}");
        Debug.Log($"- Quality Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
        Debug.Log($"- Target Frame Rate: {Application.targetFrameRate}");
    }

    public void LogPlayerReset()
    {
        playerResetCount++;
        Debug.Log($"[GameManager] Player Reset | Total Resets: {playerResetCount}");
        
        if (playerResetCount >= 3)
        {
            Debug.LogWarning("[GameManager] Player has reset multiple times. Check for potential gameplay issues.");
        }
    }

    public void LogDomeBounce()
    {
        domeBounceCount++;
        Debug.Log($"[GameManager] Dome Bounce | Total Bounces: {domeBounceCount}");
    }

    public void LogEnemyCollision()
    {
        enemyCollisionCount++;
        Debug.Log($"[GameManager] Enemy Collision | Total Collisions: {enemyCollisionCount}");
        
        if (enemyCollisionCount >= 2)
        {
            Debug.LogWarning("[GameManager] Multiple enemy collisions detected. Adjust difficulty or player mechanics.");
        }
    }

    private void Update()
    {
        TrackGameTime();
    }

    private void TrackGameTime()
    {
        TimeSpan elapsedTime = DateTime.Now - gameStartTime;
        
        // Log game time every minute
        if (elapsedTime.Minutes > 0 && elapsedTime.Seconds == 0)
        {
            Debug.Log($"[GameManager] Game Time: {elapsedTime.Minutes} minutes");
        }
    }

    private void OnApplicationQuit()
    {
        LogGameSummary();
    }

    private void LogGameSummary()
    {
        TimeSpan totalGameTime = DateTime.Now - gameStartTime;
        
        Debug.Log("[GameManager] Game Summary:");
        Debug.Log($"- Total Game Time: {totalGameTime.Minutes} minutes {totalGameTime.Seconds} seconds");
        Debug.Log($"- Player Resets: {playerResetCount}");
        Debug.Log($"- Dome Bounces: {domeBounceCount}");
        Debug.Log($"- Enemy Collisions: {enemyCollisionCount}");
    }
}