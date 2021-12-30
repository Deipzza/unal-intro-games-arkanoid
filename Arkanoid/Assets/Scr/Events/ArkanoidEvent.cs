using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArkanoidEvent { // Solo será para guardar eventos por lo que puede ser static
    public delegate void GameStartAction(); // Evento de iniciar el juego
    public static GameStartAction OnGameStartEvent;
    public delegate void GameOverAction(); // Evento de terminar el juego
    public static GameOverAction OnGameOverEvent;
    public delegate void BallDeadZoneAction(Ball ball); // Evento de bola salida por debajo (perder)
    public static BallDeadZoneAction OnBallReachDeadZoneEvent;
    public delegate void BlockDestroyedAction(int blockID); // Evento de destruir bloques (ganar)
    public static BlockDestroyedAction OnBlockDestroyedEvent;
    public delegate void ScoreUpdatedAction(int score, int totalScore); // Evento del UI de puntaje
    public static ScoreUpdatedAction OnScoreUpdatedEvent;
    public delegate void LevelUpdatedAction(int level); // Evento del UI de puntaje
    public static LevelUpdatedAction OnLevelUpdatedEvent;
    // public delegate void PowerUpSpawnedAction(Vector2 blockPosition); // Evento del spawn de los powerups
    // public static PowerUpSpawnedAction OnPowerUpSpawnedEvent;
    public delegate void PowerUpDeadZoneAction(PowerUp powerUp); // Evento de bola salida por debajo (perder)
    public static PowerUpDeadZoneAction OnPowerUpReachDeadZoneEvent;
}