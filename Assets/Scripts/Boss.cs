using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    public Image healthWrap, healthBar;
    public Vector3 offset;
    public bool isImmune;
    public int maxHealth = 2000;
    public bool isAngry;
    public AudioClip[] swingAttacks;

    int attackDameMax = 15;
    protected override void Start()
    {
        base.Start();
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void Update()
    {
        healthWrap.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
        OnHealthChange();
    }

    void OnHealthChange()
    {

        float ratio = (float)health / (float)maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
    }

    public override void Attack()
    {
        base.Attack();
        int rand = Random.Range(0, swingAttacks.Length);
        SoundManager.Ins.PlaySound(swingAttacks[rand]);

    }

    public override void ReceiveDamage(int dmg)
    {
        if (isImmune)
            return;

        health -= dmg;
        GetComponent<Animator>().SetTrigger("hurt");

        //Angry
        if (health <= maxHealth / 2)
        {
            isAngry = true;
            GetComponent<Animator>().SetBool("isAngry", true);
            attackDamage = attackDameMax;
        }

        if (health <= 0)
        {
            health = 0;
            StartCoroutine(DeathTime());
        }
    }

    public override void Death()
    {
        SoundManager.Ins.PlaySound(SoundManager.Ins.tadaSound);
        GameManager.instance.winPanel.SetActive(true);
    }
    public void Trigger()
    {
        GetComponent<Animator>().SetTrigger("action");
    }

    IEnumerator DeathTime()
    {
        SoundManager.Ins.PlaySound(SoundManager.Ins.bossDie);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("death");
        yield return new WaitForSeconds(1.5f);
        Death();
    }
}
