using UnityEngine;

public class FlyingBird : MonoBehaviour
{
    private IPlayerHitter _playerHitter;
    private IFlyable _flyable;

    private Rigidbody2D _rb;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(IPlayerHitter hittingWay, IFlyable flyable, Vector3 initialPosition)
    {
        _playerHitter = hittingWay;
        _flyable = flyable;
        transform.position = initialPosition;
    }



    void FixedUpdate()
    {
        CheckIfCanBeDestroyed();
        _flyable.Fly(_rb);
    }

    private void CheckIfCanBeDestroyed()
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPos.y > 1.2f)
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.attachedRigidbody.GetComponent<PlayerController>();
        if (player != null)
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position); 
            Vector3 screenPos = Camera.main.WorldToViewportPoint(collisionPoint); 
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
            {
                return; 
            }
            _playerHitter.HitPlayer(player);
        }
    }
}
