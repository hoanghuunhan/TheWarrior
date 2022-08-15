using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorGroundPlayer : MonoBehaviour
{
    public bool _isGround;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")|| collision.gameObject.CompareTag("Obstacle")
            || collision.gameObject.CompareTag("Enemy"))
        {
            _isGround = true;
        }
    }
}
