using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite emptyChest;
    bool unlockSkill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !unlockSkill)
        {
            if (emptyChest != null)
            {
                unlockSkill = true;
                GetComponent<SpriteRenderer>().sprite = emptyChest;

                GameManager.instance.playerTakeDame.isUnlockSkill = true;
                GameManager.instance.notifiSkill4.SetActive(true);
                SoundManager.Ins.PlaySound(SoundManager.Ins.tadaSound);

                Time.timeScale = 0f;
                
            }
        }
    }
}
