using System.Collections;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    private Enemy _enemy;
    protected override void Start()
    {
        _enemy = GetComponent<Enemy>();
        base.Start();
    }
    
    public override void TakeDamage(float damage, GameObject attacker)
    {
        _enemy.target = attacker;
        base.TakeDamage(damage);
    }
    
    /// <summary>
    /// Take damage and stun the enemy.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    /// <param name="attacker">Attacker.</param>
    /// <param name="stunDuration">Stun duration (sec).</param>
    public void TakeDamageAndStun(float damage, GameObject attacker, float stunDuration)
    {
        StartCoroutine(Stun(stunDuration));
        TakeDamage(damage,attacker);
    }

    /// <summary>
    /// Dead and add point to the player score.
    /// </summary>
    protected override void Dead()
    {
        if(Random.Range(0,101) >= 90)
            GameManager.instance.AddPoint(10);
        
        base.Dead();
    }

    /// <summary>
    /// stun the enemy.
    /// </summary>
    /// <param name="stunDuration">Stun duration (sec).</param>
    /// <returns></returns>
    IEnumerator Stun(float stunDuration)
    {
        _enemy.isStun = true;
        yield return new WaitForSeconds(stunDuration);
        _enemy.isStun = false;
    }
}
