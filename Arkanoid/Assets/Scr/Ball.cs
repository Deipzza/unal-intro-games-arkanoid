using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    [SerializeField] private float _initSpeed = 5;

    public Rigidbody2D _rb;
    private Collider2D _collider;
    private const float BALL_VELOCITY_MIN_AXIS_VALUE = 0.5f;
    public float _minSpeed = 4;
    public float _maxSpeed = 7;
    public bool _isSuper = false;
    public int _forceHit = 1;
    [SerializeField] private float _timeRemainingSuper = 10f;
    public int _isSped = 0;
    [SerializeField] private float _timeRemainingSpedUp = 5f;

    public void Init() {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _isSuper = false;
        _collider.enabled = true;
        _rb.velocity = Random.insideUnitCircle.normalized * _initSpeed;
    }

    void FixedUpdate() {
        CheckVelocity();

        if (_isSuper) {
            if (_timeRemainingSuper > 0) {
                _timeRemainingSuper -= Time.deltaTime;
            }
            else {
                _isSuper = false;
                _forceHit = 1;
                transform.localScale = Vector3.one;
                _timeRemainingSuper = 10f;
            }
        }

        if (_isSped != 0) {
            if (_timeRemainingSpedUp > 0) {
                _timeRemainingSpedUp -= Time.deltaTime;
            }
            else {
                if (_isSped == 1) {
                    _minSpeed -= 2f;
                    _maxSpeed -= 2f;
                    _rb.velocity -= Vector2.one * 2;
                    _timeRemainingSpedUp = 5f;
                }
                else if (_isSped == -1) {
                    _minSpeed += 2f;
                    _maxSpeed += 2f;
                    _rb.velocity += Vector2.one * 2;
                    _timeRemainingSpedUp = 5f;
                }
                _isSped = 0;
            }
        }
    }

    private void CheckVelocity() {
        Vector2 velocity = _rb.velocity;
        float currentSpeed = velocity.magnitude;

        if (currentSpeed < _minSpeed) {
            velocity = velocity.normalized * _minSpeed;
        }
        else if (currentSpeed > _maxSpeed) {
            velocity = velocity.normalized * _maxSpeed;
        }
        if(Mathf.Abs(velocity.x) < BALL_VELOCITY_MIN_AXIS_VALUE) {
            velocity.x += Mathf.Sign(velocity.x) * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
        else if (Mathf.Abs(velocity.y) < BALL_VELOCITY_MIN_AXIS_VALUE) {   
            velocity.y += Mathf.Sign(velocity.y) * BALL_VELOCITY_MIN_AXIS_VALUE * Time.deltaTime;
        }
        _rb.velocity = velocity;
    }

    internal void Hide() {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        // Verifico si el objeto impactado no tiene un componente de tipo BlockTile
        BlockTile blockTileHit;
        if (!other.collider.TryGetComponent(out blockTileHit)) {
            return; // Si no lo tiene, no hace nada
        }
        // Si sí lo tiene, invoca OnHitCOllision con el punto de contacto entre bola y bloque
        ContactPoint2D contactPoint = other.contacts[0];
        blockTileHit.OnHitCollision(contactPoint, _forceHit);
    }
}