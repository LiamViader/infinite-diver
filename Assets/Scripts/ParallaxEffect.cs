using Unity.VisualScripting;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]
    private float _parallaxMultiplier;

    private Transform _cameraTransform;
    private Vector3 _previousCameraPos;
    private float _spriteHeigh, _startPosition;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _previousCameraPos = _cameraTransform.position;
        _spriteHeigh = GetComponent<SpriteRenderer>().bounds.size.y;
        _startPosition = transform.position.y;
    }


    void LateUpdate()
    {
        float deltaY = (_cameraTransform.position.y - _previousCameraPos.y) * _parallaxMultiplier;
        float moveAmount = _cameraTransform.position.y * (1 - _parallaxMultiplier);
        transform.Translate(new Vector3(0,deltaY,0));
        _previousCameraPos = _cameraTransform.position;

        if (moveAmount > _startPosition + _spriteHeigh)
        {
            transform.Translate(new Vector3(0,_spriteHeigh,0));
            _startPosition += _spriteHeigh;
        }
        else if (moveAmount< _startPosition - _spriteHeigh)
        {
            transform.Translate(new Vector3(0,-_spriteHeigh,0));
            _startPosition -= _spriteHeigh;
        }
    }
}
