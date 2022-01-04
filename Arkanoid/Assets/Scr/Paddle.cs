using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _movementLimit = 7;
    private Vector3 _targetPosition;
    private Camera _cam;
    private Camera Camera {
        get {
            if (_cam == null) {
                _cam = Camera.main;
            }
            return _cam;
        }
    }
    private SpriteRenderer _paddleRenderer;
    private Animator _paddleAnimator;

    void Update() {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit);
        _targetPosition.y = this.transform.position.y;
   
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Verifico si el objeto impactado no tiene un componente de tipo PowerUp
        PowerUp powerUpHit;
        // Si no lo tiene, no hace nada
        if (!other.collider.TryGetComponent(out powerUpHit)) {
            return;
        }
        // Si sí lo tiene, invoca OnHitCOllision con el punto de contacto entre bola y bloque
        ContactPoint2D contactPoint = other.contacts[0];
        powerUpHit.OnHitCollision(contactPoint);
    }

    public void ResetPaddle() {
        _paddleRenderer = GetComponentInChildren<SpriteRenderer>();
        _paddleAnimator = GetComponentInChildren<Animator>();

        _paddleAnimator.enabled = true;
        _paddleRenderer.sprite = Resources.Load<Sprite>("Sprites/Paddles/Normal_0");
    }
}