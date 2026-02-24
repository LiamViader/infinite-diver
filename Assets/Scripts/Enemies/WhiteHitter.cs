using UnityEngine;

public class WhiteHitter : IPlayerHitter
{
    private int _heal = 1;
    public void HitPlayer(PlayerController player)
    {
        player.Heal(_heal);
    }
}
