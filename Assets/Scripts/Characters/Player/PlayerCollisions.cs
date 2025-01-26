using Characters.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCollisions : MonoBehaviour
    {
        private PlayerBody _body;

        private void Awake()
        {
            _body = GetComponent<PlayerBody>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}