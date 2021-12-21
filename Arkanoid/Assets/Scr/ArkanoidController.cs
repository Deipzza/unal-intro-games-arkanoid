using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class ArkanoidController : MonoBehaviour {
    [SerializeField] private GridController _gridController;
    [Space(20)] [SerializeField] private List<LevelData> _levels = new List<LevelData>();
    private int _currentLevel = 0;
    private const string BALL_PREFAB_PATH = "Prefabs/Ball"; // Como es un valor que no se va a cambiar se le pone const
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f); // Como es un objeto que no se va a cambiar se le pone readonly
    private Ball _ballPrefab = null;
     private List<Ball> _balls = new List<Ball>();
     private int _totalScore = 0;
    
    private void Start() {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { // El juego se iniciará cuando se oprima la barra espaciadora
            InitGame();
        }
    }

    private void OnDestroy() {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
    }

    private void InitGame() {
        _currentLevel = 0;
        _totalScore = 0;
        _gridController.BuildGrid(_levels[0]);

        SetInitialBall();

        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
    }

    private void OnBlockDestroyed(int blockID) {
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockID);

        if (blockDestroyed != null) {
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore); // Se invoca el evento para que se actualice el puntaje en pantalla
        }
        if (_gridController.GetBlocksActive() == 0) {
            _currentLevel++;

            if (_currentLevel >= _levels.Count) {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            else {
                SetInitialBall();
                _gridController.BuildGrid(_levels[_currentLevel]);
                ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
            }
        }
    }

    private void OnBallReachDeadZone(Ball ball) {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }

    private void CheckGameOver() {
        if (_balls.Count == 0) {
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }

    private void SetInitialBall() {
        ClearBalls();
        Ball ball = CreateBallAt(BALL_INIT_POSITION);
        ball.Init();
        _balls.Add(ball);
    }

    private Ball CreateBallAt(Vector2 position) {
        if (_ballPrefab == null) {
             _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }
        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }

    private void ClearBalls() {
        for (int i = _balls.Count - 1; i >= 0; i--) {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
        _balls.Clear();
    }
}