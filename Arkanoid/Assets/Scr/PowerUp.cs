using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    [SerializeField] private GameObject _paddle;
    private const string PADDLE_PATH = "Sprites/Paddles/{0}";
    private GameObject _ball = null;
    // private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private SpriteRenderer _paddleRenderer;
    private Animator _paddleAnimator;
    // private bool _isSuper = false;
    // private float _timeRemainingSuper = 10f;
    // private PowerUp _powerUpPrefab = null;
    // private const string POWERUP_PREFAB_PATH = "Prefabs/PowerUp";

    
    void Start() {
        _paddle =  GameObject.Find("Paddle");
        _ball = GameObject.Find("Ball(Clone)");
        // _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        _paddleRenderer = _paddle.GetComponentInChildren<SpriteRenderer>();
        _paddleAnimator = _paddle.GetComponentInChildren<Animator>();

        // ArkanoidEvent.OnPowerUpSpawnedEvent += OnPowerUpSpawned;
        ArkanoidEvent.OnPowerUpReachDeadZoneEvent += OnPowerUpReachDeadZone;
    }

    void OnDestroy() {
        // ArkanoidEvent.OnPowerUpSpawnedEvent -= OnPowerUpSpawned;
        ArkanoidEvent.OnPowerUpReachDeadZoneEvent -= OnPowerUpReachDeadZone;
    }

    private void OnPowerUpReachDeadZone(PowerUp powerUp) {
        Destroy(powerUp.gameObject);
    }

    // Retornar el tipo de powerup que será
    public void Type(float randomValue) {
        float anotherRandom = UnityEngine.Random.value;
        // Podemos tener tres tipos de powerup: scale, superball y speed
        // Spawnear scale
        if (anotherRandom < 0.25f) {
            Debug.Log("Scale " + anotherRandom);
            if (anotherRandom < 0.08f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Down");
                // return Resources.Load<Sprite>("Sprites/PowerUps/Scale_Down");
            }
            else if (anotherRandom >= 0.08f && anotherRandom < 0.17f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Up");
                // return Resources.Load<Sprite>("Sprites/PowerUps/Scale_Up");
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Normal");
                // return Resources.Load<Sprite>("Sprites/PowerUps/Scale_Normal");
            }
            // _paddleRenderer.sprite = ScalePowerUp(anotherRandom, blockPosition);
        }
        // Spawnear superball
        else if (anotherRandom >= 0.25f && anotherRandom < 0.5f) {
            Debug.Log("Superball " + anotherRandom);
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Superball");
        }
        // Spawnear speed
        else if (anotherRandom >= 0.5f && anotherRandom < 0.75f) {
            Debug.Log("Speed " + anotherRandom);
            if (anotherRandom < 0.63f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Speed_Fast");
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Speed_Slow");
            }
        }
        // Spawnear score
        else {
            Debug.Log("Score " + anotherRandom);

        }
    }

    // El paddle interactuó con el powerup
    public void OnHitCollision(ContactPoint2D contactPoint) {
        gameObject.SetActive(false);
        string path = string.Empty;
        string spriteName = gameObject.GetComponentInChildren<SpriteRenderer>().sprite.name;
        Ball scriptBall = _ball.GetComponent<Ball>();
        
        // Activar el powerUp
        if (spriteName == "Scale_Down") {
            Debug.Log("DOWN");
            _paddleAnimator.enabled = false;
            path = string.Format(PADDLE_PATH, "Small");
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Scale_Up") {
            Debug.Log("UP");
            _paddleAnimator.enabled = false;
            path = string.Format(PADDLE_PATH, "Large");
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Scale_Normal") {
            Debug.Log("NORMAL");
            _paddleAnimator.enabled = true;
            path = string.Format(PADDLE_PATH, "Normal_0");
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Superball") {
            Debug.Log("SUPAAAA");
            scriptBall._isSuper = true;
            scriptBall._forceHit = 2;
            _ball.transform.localScale = Vector3.one * 2;
        }
        else if (spriteName == "Speed_Fast") {
            Debug.Log("FAST");
            scriptBall._isSped = 1;
            scriptBall._minSpeed += 2f;
            scriptBall._maxSpeed += 2f;
            scriptBall._rb.velocity += (Vector2.one * 2);
        }
        else if (spriteName == "Speed_Slow") {
            Debug.Log("SLOW");
            scriptBall._isSped = -1;
            scriptBall._minSpeed -= 2f;
            scriptBall._maxSpeed -= 2f;
            scriptBall._rb.velocity -= (Vector2.one * 2);
        }
        else if (spriteName == "Score_50") {
            Debug.Log("50");

        }
    }
}