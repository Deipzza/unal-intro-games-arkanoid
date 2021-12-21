using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType {
    Small,
    Big
}

public enum BlockColor {
    Green,
    Blue,
    Orange,
    Red,
    Purple
}

public class BlockTile : MonoBehaviour {

    [SerializeField] BlockType _type = BlockType.Big;
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private int _totalHits = 1; // Máxima cantidad de golpes que puede recibir el bloque
    private int _currentHits = 0; // Cantidad actual de golpes que ha recibido el bloque
    private const string BLOCK_BIG_PATH = "Sprites/BlockTiles/Big/Big_{0}_{1}";
    [SerializeField] private BlockColor _color = BlockColor.Blue;
    private int _id;
    [SerializeField] private int _score = 10;
    public int Score => _score; //

    public void SetData(int id, BlockColor color) {
        _id = id;
        _color = color;
    }

    public void Init() {
        _currentHits = 0;
        _totalHits = _type == BlockType.Big ? 2 : 1; // Si es grande requiere dos golpes, sino requiere uno

        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;

        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = GetBlockSprite(_type, _color, 0);
    }

    public void OnHitCollision(ContactPoint2D contactPoint) { // Se ejecuta cuando la bola colisiona con el bloque
        _currentHits++; // Aumento la cantidad de golpes que ha recibidoel bloque

        // Si la cantidad de golpes es igual o superior que la máxima soportada se desactiva el bloque
        if (_currentHits >= _totalHits) {
            _collider.enabled = false;
            gameObject.SetActive(false);
            ArkanoidEvent.OnBlockDestroyedEvent?.Invoke(_id); // Se invoca el evento cuando el bloque es destruido
        }
        else {
            _renderer.sprite = GetBlockSprite(_type, _color, _currentHits);
        }
    }

    static Sprite GetBlockSprite(BlockType type, BlockColor color, int state) {
        string path = string.Empty;
        if (type == BlockType.Big) {
            path = string.Format(BLOCK_BIG_PATH, color, state);
        }

        if (string.IsNullOrEmpty(path)) {
            return null;
        }

        return Resources.Load<Sprite>(path);
    }
}
