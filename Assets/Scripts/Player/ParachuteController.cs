using UnityEngine;

public class ParachuteController
{
    private bool _isOpen;
    private float _cooldownTotal;
    private float _cooldownElapsed;
    private float _maxDuration;
    private float _durationLeft;
    private CloneManager _cloneManager;
    private AudioSource _audioSource;
    private AudioClip _openClip;
    private bool _initialized = false;

    public ParachuteController(float cooldown, float maxDuration, CloneManager cloneManager, AudioSource audioSource, AudioClip parachuteOpenClip)
    {
        _isOpen = false;
        _cooldownTotal = cooldown;
        _maxDuration = maxDuration;
        _durationLeft = maxDuration;
        _cooldownElapsed = cooldown;
        _cloneManager = cloneManager;

        _audioSource = audioSource;
        _openClip = parachuteOpenClip;
    }

    public bool IsOpen => _isOpen;
    public bool IsOnCooldown => _cooldownElapsed < _cooldownTotal;

    public void InitParachute()
    {
        _initialized = true;
        FullyRecharge();
    }

    public void Open()
    {
        if (!IsOnCooldown)
        {
            _audioSource.PlayOneShot(_openClip);
            _cloneManager.OpenClonesParachutes();
            _isOpen = true;
        }
    }

    public void Close()
    {
        if (_isOpen)
        {
            _cloneManager.CloseClonesParachutes();
            _isOpen = false;
            _cooldownElapsed = 0f;
        }

    }

    public void UpdateCooldown(float deltaTime)
    {
        if (!IsOpen && IsOnCooldown)
        {
            _cooldownElapsed += deltaTime;
        }
    }

    public void UpdateDuration(float deltaTime)
    {
        if (_isOpen)
        {
            _durationLeft -= deltaTime;
            if (_durationLeft <= 0f)
            {
                _durationLeft = 0f;
                Close();
            }
        }
        else
        {
            _durationLeft = Mathf.Min(_maxDuration, _durationLeft + deltaTime);
        }
        _cloneManager.SetProgressBars(_durationLeft/_maxDuration, _isOpen);
    }

    public void FullyRecharge()
    {
        if (_initialized)
        {
            _durationLeft = _maxDuration;
            _cloneManager.SetProgressBars(_durationLeft / _maxDuration, _isOpen);
        }

    }

}
