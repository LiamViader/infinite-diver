using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting.Antlr3.Runtime;

public class CloneManager : MonoBehaviour
{
    public GameObject clonePrefab;

    private VisualClonePlayer _leftClone;
    private VisualClonePlayer _centerClone;
    private VisualClonePlayer _rightClone;

    private Vector3 _startCloneScale;

    private float _distanceBetweenClones;


    private void Awake()
    {
        Vector3 rightEdgeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane));
        _distanceBetweenClones = rightEdgeWorld.x * 2;
        _leftClone = Instantiate(clonePrefab, new Vector3(-_distanceBetweenClones, 0, 0), Quaternion.identity, transform).GetComponent<VisualClonePlayer>();
        _centerClone = Instantiate(clonePrefab, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<VisualClonePlayer>();
        _rightClone = Instantiate(clonePrefab, new Vector3(_distanceBetweenClones, 0, 0), Quaternion.identity, transform).GetComponent<VisualClonePlayer>();
        _startCloneScale = _leftClone.transform.localScale;

    }
    void Start()
    {
        GameManager.Instance.OnGameOver += HandleGameOver;


    }

    
    void FixedUpdate()
    {
        float marginToNotLag = _distanceBetweenClones / 4;
        if (_centerClone.transform.position.x > _distanceBetweenClones + marginToNotLag) // if left clone is now at center + margin
        {
            Vector3 aux = _centerClone.transform.position;
            _centerClone.transform.position = _leftClone.transform.position;
            _rightClone.transform.position = new Vector3(_centerClone.transform.position.x - _distanceBetweenClones, _rightClone.transform.position.y, _rightClone.transform.position.z);
            _leftClone.transform.position = _rightClone.transform.position;
            _rightClone.transform.position = aux;
        }
        else if (_centerClone.transform.position.x < -_distanceBetweenClones - marginToNotLag) // if right clone is now at center - margin
        {
            Vector3 aux = _centerClone.transform.position;
            _centerClone.transform.position = _rightClone.transform.position;
            _leftClone.transform.position = new Vector3(_centerClone.transform.position.x+_distanceBetweenClones, _leftClone.transform.position.y, _leftClone.transform.position.z);
            _rightClone.transform.position = _leftClone.transform.position;
            _leftClone.transform.position = aux;
        }
    }

    public void SetProgressBars(float proportion, bool isOpen)
    {
        _leftClone.SetParachuteProgress(proportion, isOpen);
        _rightClone.SetParachuteProgress(proportion, isOpen);
        _centerClone.SetParachuteProgress(proportion, isOpen);
    }

    public Vector3 GetClonesScale()
    {
        return _leftClone.transform.localScale;
    }
    public float GetClonesScaleMagnitude()
    {
        return _leftClone.transform.localScale.magnitude;
    }

    public void SetClonesScale(Vector3 scale)
    {
        _leftClone.transform.localScale=scale;
        _rightClone.transform.localScale = scale;
        _centerClone.transform.localScale = scale;
    }

    public void OpenClonesParachutes()
    {
        _leftClone.OpenParachute();
        _rightClone.OpenParachute();
        _centerClone.OpenParachute();
    }

    public void CloseClonesParachutes()
    {
        _leftClone.CloseParachute();
        _rightClone.CloseParachute();
        _centerClone.CloseParachute();
    }

    public void DamageFlashClones()
    {
        _leftClone.DamageFlash();
        _rightClone.DamageFlash();
        _centerClone.DamageFlash();
    }

    private void HandleGameOver()
    {
        _leftClone.transform.localScale = _startCloneScale;
        _rightClone.transform.localScale = _startCloneScale;
        _centerClone.transform.localScale = _startCloneScale;
    }


}
