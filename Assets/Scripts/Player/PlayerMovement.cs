using UnityEngine;

public class PlayerMovement
{
    private Rigidbody2D _rb;
    private float _currentSpeedX;
    private float _fallSpeed;

    public PlayerMovement(Rigidbody2D rb)
    {
        _rb = rb;
    }

    public void SetHorizontalSpeed(float speed)
    {
        _currentSpeedX = speed;
    }

    public void SetFallSpeed(float speed)
    {
        _fallSpeed = speed;
    }

    public void ApplyMovement()
    {
        _rb.linearVelocity = new Vector2(_currentSpeedX, _fallSpeed);
    }
}
