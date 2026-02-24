using UnityEngine;

public class BlackHitter : IPlayerHitter
{
    private int _damage = 1;
    public void HitPlayer(PlayerController player)
    {
        player.Damage(_damage);
    }
}
