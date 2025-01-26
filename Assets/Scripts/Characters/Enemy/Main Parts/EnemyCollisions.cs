using Characters.Enemy;
using Characters.Interfaces;
using Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private DefaultEnemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<DefaultEnemy>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if(target is PlayerBody)
        {
            target.TakeDamage(_enemy.Stats.Damage);
        }
    }
}
