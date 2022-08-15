using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUltiSkill : MonoBehaviour
{
    public Sprite emptyChest;
    bool unlockSkill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !unlockSkill)
        {
            if (emptyChest != null)
            {
                GetComponent<SpriteRenderer>().sprite = emptyChest;

                GameManager.instance.playerTakeDame.isUnlockUlti = true;
                GameManager.instance.energyPanel.SetActive(true);
                GameManager.instance.notifiSkill5.SetActive(true);
                SoundManager.Ins.PlaySound(SoundManager.Ins.tadaSound);
                Time.timeScale = 0f;
            }
        }
    }
}
