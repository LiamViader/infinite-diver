using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _controls;
    private PlayerMovement _movement;
    private ParachuteController _parachute;
    private Rigidbody2D _rb;
    private CloneManager _cloneManager;

    private float _currentSpeedX;
    [SerializeField] private float maxHorizontalSpeed = 5f;

    [SerializeField]  private float startFallSpeed = -4f;
    private float _currentFallSpeed;
    private float _accelY = -0.01f;

    private InputAction _movementAction;
    private InputAction _parachuteAction;

    private float _initialGameplayPosition;

    private bool _inmune = false;
    private float _invencibilitySeconds = 1f;
    private float _invencibilityElapsed;

    public AudioSource audioSource;
    public AudioClip parachuteOpenClip;
    public AudioClip takeDamageClip;
    public AudioClip healClip;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _movement = new PlayerMovement(_rb);
        _cloneManager = GetComponent<CloneManager>();



        InitializeInput();
        GameManager.Instance.OnGameStart += HandleGameStarted;
        GameManager.Instance.OnGameOver += HandleGameOver;
        _parachute = new ParachuteController(0.4f, 4f, _cloneManager, audioSource, parachuteOpenClip);
        HandleGameOver();

    }

    private void Start()
    {
        _parachute.InitParachute();
    }

    private void InitializeInput()
    {
        _controls = new PlayerControls();
        _parachuteAction = _controls.Player.Parachute;
        _movementAction = _controls.Player.Movement;
        if (UnityEngine.InputSystem.Accelerometer.current != null)
        {
            InputSystem.EnableDevice(UnityEngine.InputSystem.Accelerometer.current);
        }
        else
        {
            UIManager.Instance.ShowError("No Accelerometer available on your device. You need gyroscope to play the game");
        }

        _parachuteAction.started += _ => _parachute.Open();
        _parachuteAction.canceled += _ => _parachute.Close();

    }



    private void FixedUpdate()
    {
        HandleInmmunity();
        if (!GameManager.Instance.IsGameActive()){
            _movement.ApplyMovement();
        }
        else
        {
            _parachute.UpdateCooldown(Time.fixedDeltaTime);
            _parachute.UpdateDuration(Time.fixedDeltaTime);

            UpdateHorizontalSpeed();
            ComputeFallSpeed(Time.fixedDeltaTime);

            _movement.SetHorizontalSpeed(_currentSpeedX);
            _movement.SetFallSpeed(_parachute.IsOpen ? _currentFallSpeed * 0.1f : _currentFallSpeed);
            _movement.ApplyMovement();
            GameManager.Instance.UpdateDistanceTraveled(_initialGameplayPosition - transform.position.y);
        }
    }

    private void HandleInmmunity()
    {
        if (_inmune)
        {
            _invencibilityElapsed +=Time.fixedDeltaTime;
            if (_invencibilityElapsed >= _invencibilitySeconds) _inmune = false;
        }
    }

    public void Damage(int damage)
    {
        if (GameManager.Instance.IsGameActive() && !_inmune)
        {
            audioSource.PlayOneShot(takeDamageClip);
            float newScale = _cloneManager.GetClonesScale().x - 0.1f * damage;
            _cloneManager.SetClonesScale(new Vector3(newScale, newScale, newScale));
            if (ShouldDie()) GameManager.Instance.EndGame();
            else
            {
                _inmune = true;
                _invencibilityElapsed = 0;
                _cloneManager.DamageFlashClones();
            }

        }

    }

    public void Heal(int heal)
    {
        if (GameManager.Instance.IsGameActive())
        {
            audioSource.PlayOneShot(healClip);
            float newScale = _cloneManager.GetClonesScale().x + 0.1f * heal;
            _cloneManager.SetClonesScale(new Vector3(newScale, newScale, newScale));
        }
    }

    private bool ShouldDie()
    {
        float clonesScale=_cloneManager.GetClonesScale().x;
        return clonesScale <= 0.2;

    }


    private void UpdateHorizontalSpeed()
    {
        Vector3 accelValue = _movementAction.ReadValue<Vector3>(); 
        float tiltX = accelValue.x;
        tiltX = Mathf.Clamp(tiltX * 4, -1, 1);
        _currentSpeedX = tiltX * maxHorizontalSpeed;

    }

    private void ComputeFallSpeed(float delta)
    {
        _currentFallSpeed += delta * _accelY;
    }

    private void HandleGameStarted()
    {
        _currentFallSpeed = startFallSpeed;
        _movement.SetFallSpeed(_currentFallSpeed);
        _movement.SetHorizontalSpeed(0);
        _controls.Player.Enable();
        _initialGameplayPosition = transform.position.y;
    }

    private void HandleGameOver()
    {
        _currentFallSpeed = startFallSpeed;
        _movement.SetFallSpeed(_currentFallSpeed);
        transform.position =new Vector3(0,transform.position.y,transform.position.z);
        _currentSpeedX = 0;
        _movement.SetHorizontalSpeed(_currentSpeedX);
        _parachute?.FullyRecharge();
        _controls.Player.Disable();
    }

    private void OnEnable()
    {
        if (GameManager.Instance.IsGameActive()) _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }


}
