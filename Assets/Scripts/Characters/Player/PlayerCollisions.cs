using Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private PlayerCharacterBody _body;

        private void Awake()
        {
            _body = GetComponent<PlayerCharacterBody>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}