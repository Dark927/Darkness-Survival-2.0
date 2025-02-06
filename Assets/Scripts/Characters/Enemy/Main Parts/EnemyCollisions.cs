using Characters.Enemy;
using Characters.Interfaces;
using Characters.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    private DefaultEnemyLogic _mainLogic;

    private void Awake()
    {
        _mainLogic = GetComponent<DefaultEnemyLogic>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();

        if(target is PlayerCharacterBody)
        {

        }
    }
}
