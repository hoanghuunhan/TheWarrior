using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    public GameObject wallBlock;
    public Boss boss;
    bool isTriggerBoss = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isTriggerBoss == false)
        {
            GameManager.instance.isTeleport = false;
            wallBlock.SetActive(true);
            isTriggerBoss = true;
            SoundManager.Ins.PlaySound(SoundManager.Ins.introBoss);
            boss.GetComponent<Animator>().SetTrigger("action");
            boss.GetComponent<Collider2D>().enabled = true;
        }
    }
}
