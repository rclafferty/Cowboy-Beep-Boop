using UnityEngine;

public interface ICombat
{
    public int GetTeam(); // 0 for player, 1 for enemy, -1 for neutral
    public void Heal(int amount);
    public void TakeDamage(int amount);
    public void Die();
}
