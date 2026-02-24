using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]
    private float _parallaxMultiplier;

    private Transform _cameraTransform;
    private Vector3 _previousCameraPos;
    private float _spriteHeight, _startPosition;

    void Start()
    {
        _cameraTransform = Camera.main.transform;

        AdaptWidthToScreen();

        _previousCameraPos = _cameraTransform.position;
        _spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        _startPosition = transform.position.y;
    }

    void LateUpdate()
    {
        float deltaY = (_cameraTransform.position.y - _previousCameraPos.y) * _parallaxMultiplier;
        float moveAmount = _cameraTransform.position.y * (1 - _parallaxMultiplier);

        transform.Translate(new Vector3(0, deltaY, 0));
        _previousCameraPos = _cameraTransform.position;

        if (moveAmount > _startPosition + _spriteHeight)
        {
            transform.Translate(new Vector3(0, _spriteHeight, 0));
            _startPosition += _spriteHeight;
        }
        else if (moveAmount < _startPosition - _spriteHeight)
        {
            transform.Translate(new Vector3(0, -_spriteHeight, 0));
            _startPosition -= _spriteHeight;
        }
    }

    private void AdaptWidthToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float spriteWidth = sr.sprite.bounds.size.x;

        float scaleFactor = cameraWidth / spriteWidth;

        scaleFactor = Mathf.Max(1f, scaleFactor);

        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }
}