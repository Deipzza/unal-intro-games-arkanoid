using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUp : MonoBehaviour {

    private GameObject _paddle;
    private const string PADDLE_PATH = "Sprites/Paddles/{0}";
    private GameObject _ball = null;
    private SpriteRenderer _paddleRenderer;
    private Animator _paddleAnimator;
    private CapsuleCollider2D _paddleCollider;
    private TextMeshProUGUI _scoreText;
    private ArkanoidController _arkanoidController;
    
    void Start() {
        _paddle =  GameObject.Find("Paddle");
        _ball = GameObject.Find("Ball(Clone)");
        _arkanoidController = GameObject.Find("ArkanoidController").GetComponent<ArkanoidController>();
        _paddleRenderer = _paddle.GetComponentInChildren<SpriteRenderer>();
        _paddleAnimator = _paddle.GetComponentInChildren<Animator>();
        _paddleCollider = _paddle.GetComponentInChildren<CapsuleCollider2D>();

        ArkanoidEvent.OnPowerUpReachDeadZoneEvent += OnPowerUpReachDeadZone;
    }

    void OnDestroy() {
        ArkanoidEvent.OnPowerUpReachDeadZoneEvent -= OnPowerUpReachDeadZone;
    }

    private void OnPowerUpReachDeadZone(PowerUp powerUp) {
        powerUp.gameObject.SetActive(false);
    }

    // Retornar el tipo de powerup que será
    public void Type(float randomValue) {
        float anotherRandom = UnityEngine.Random.value;

        // Podemos tener tres tipos de powerup: scale, superball y speed
        // Spawnear scale
        if (anotherRandom < 0.33f) {
            if (anotherRandom < 0.11f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Down");
            }
            else if (anotherRandom >= 0.11f && anotherRandom < 0.22f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Up");
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Scale_Normal");
            }
        }
        // Spawnear superball
        // else if (anotherRandom >= 0.25f && anotherRandom < 0.5f) {
        //     // Debug.Log("Superball " + anotherRandom);
        //     gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Superball");
        // }

        // Spawnear speed
        else if (anotherRandom >= 0.33f && anotherRandom < 0.66f) {
            if (anotherRandom < 0.63f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Speed_Fast");
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Speed_Slow");
            }
        }
        // Spawnear score
        else {
            if (anotherRandom < 0.76f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Score_50");
            }
            else if (anotherRandom >= 0.76f && anotherRandom < 0.86f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Score_100");
            }
            else if (anotherRandom >= 0.86f && anotherRandom < 0.95f) {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Score_250");
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/PowerUps/Score_500");
            }
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
            // Debug.Log("DOWN");
            _paddleAnimator.enabled = false;
            path = string.Format(PADDLE_PATH, "Small");
            _paddleCollider.size = new Vector2(0.74f, 0.4f);
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Scale_Up") {
            // Debug.Log("UP");
            _paddleAnimator.enabled = false;
            path = string.Format(PADDLE_PATH, "Large");
            _paddleCollider.size = new Vector2(2.2f, 0.4f);
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Scale_Normal") {
            // Debug.Log("NORMAL");
            _paddleAnimator.enabled = true;
            path = string.Format(PADDLE_PATH, "Normal_0");
            _paddleCollider.size = new Vector2(1.56f, 0.4f);
            _paddleRenderer.sprite = Resources.Load<Sprite>(path);
        }
        else if (spriteName == "Superball") {
            // Debug.Log("SUPAAAA");
            scriptBall._isSuper = true;
            scriptBall._forceHit = 2;
            _ball.transform.localScale = Vector3.one * 2;
            _ball.GetComponent<CircleCollider2D>().isTrigger = true;
        }
        else if (spriteName == "Speed_Fast") {
            // Debug.Log("FAST");
            if (scriptBall._isSped != 1) {
                scriptBall._isSped = 1;
                scriptBall._minSpeed *= 1.5f;
                scriptBall._maxSpeed *= 1.5f;
                scriptBall._rb.velocity *= 1.5f * Time.deltaTime;
            }
        }
        else if (spriteName == "Speed_Slow") {
            // Debug.Log("SLOW");
            if (scriptBall._isSped != -1) {
                scriptBall._isSped = -1;
                scriptBall._minSpeed *= 0.5f;
                scriptBall._maxSpeed *= 0.5f;
                scriptBall._rb.velocity *= 0.5f * Time.deltaTime;
            }
        }
        else if (spriteName == "Score_50") {
            // Debug.Log("50");
            _arkanoidController._totalScore += 50;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(50, _arkanoidController._totalScore);
        }
        else if (spriteName == "Score_100") {
            // Debug.Log("100");
            _arkanoidController._totalScore += 100;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(100, _arkanoidController._totalScore);
        }
        else if (spriteName == "Score_250") {
            // Debug.Log("250");
            _arkanoidController._totalScore += 250;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(250, _arkanoidController._totalScore);
        }
        else if (spriteName == "Score_500") {
            // Debug.Log("500");
            _arkanoidController._totalScore += 500;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(500, _arkanoidController._totalScore);
        }
    }
}