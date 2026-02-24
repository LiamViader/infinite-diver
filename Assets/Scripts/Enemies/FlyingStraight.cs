using UnityEngine;

public class FlyingStraight : IFlyable
{

    private Vector3 _speed;
    public FlyingStraight(Vector3 speed)
    {
        _speed = speed;
    }

    public void Fly(Rigidbody2D rb)
    {
        rb.linearVelocity = _speed;
    }
}
