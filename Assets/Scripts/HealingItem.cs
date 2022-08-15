using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int healingPoint = 30;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.player.Heal(healingPoint);
            SoundManager.Ins.PlaySound(SoundManager.Ins.slurp);
            Destroy(gameObject);
        }
    }
}
