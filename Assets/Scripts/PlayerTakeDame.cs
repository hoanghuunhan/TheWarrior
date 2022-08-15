using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDame : MonoBehaviour
{
    public LayerMask enemyLayer;

    [SerializeField] int damePointMin = 25;
    [SerializeField] int damePointMax = 50;
    [SerializeField] int dameUlti = 200;
    [SerializeField] float timeLimitUlti = 10f;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject deathEffect;
    [SerializeField] GameObject attackEffect;
    [SerializeField] ParticleSystem dust;
    
    public AudioClip[] kickSounds;
    public Animator anim;
    public LineRenderer lineRenderer;
    public bool isUnlockSkill;
    public bool isUnlockUlti;

    //cooldown time attack
    float timeLimitAttack;
    float timeCooldownUlti;
    int randomDame;
    int randomKickSound;


    private void Start()
    {
        anim = GetComponent<Animator>();
        timeLimitAttack = 0.3f;
        timeCooldownUlti = 10f;
    }

    // Update is called once per frame
    private void Update()
    {
        randomKickSound = Random.Range(0, kickSounds.Length);

        if (anim == null || !GameManager.instance.player._isAlive)
            return;

        //time cooldown attack
        timeLimitAttack += Time.deltaTime;

        //Update Energy Ultimate
        UPdateEnergy();

        //Attack 1
        if (Input.GetKeyDown(KeyCode.J) && timeLimitAttack > 0.3f)
        {
            randomDame = Random.Range(damePointMin, damePointMax);
            anim.SetTrigger("Attack1");
            timeLimitAttack = 0;
        }

        //Attack 2
        else if (Input.GetKeyDown(KeyCode.K) && timeLimitAttack > 0.3f)
        {
            randomDame = Random.Range(damePointMin, damePointMax);
            anim.SetTrigger("Attack2");
            timeLimitAttack = 0;
        }

        //Attack 3
        else if (Input.GetKeyDown(KeyCode.L) && anim.GetBool("IsGround") && timeLimitAttack > 0.3f)
        {
            randomDame = Random.Range(damePointMin, damePointMax);
            anim.SetTrigger("Attack3");
            timeLimitAttack = -0.25f;
        }

        //Attack 4
        else if (Input.GetKeyDown(KeyCode.I) && timeLimitAttack > 0.3f && isUnlockSkill)
        {
            randomDame = Random.Range(damePointMin, damePointMax);
            anim.SetTrigger("Attack4");
            timeLimitAttack = 0;
        }

        //Roll
        else if (Input.GetKeyDown(KeyCode.LeftShift) && anim.GetBool("IsGround") && timeLimitAttack > 0.3f)
        {
            randomDame = Random.Range(damePointMin, damePointMax);
            anim.SetTrigger("Roll");
            timeLimitAttack = -0.25f;
        }

        //Ultimate
        if (Input.GetKeyDown(KeyCode.O) && timeCooldownUlti >= timeLimitUlti && isUnlockUlti)
        {
            anim.SetTrigger("Ultimate");
            StartCoroutine(Ultimate());
            timeCooldownUlti = 0;
            SoundManager.Ins.PlaySound(SoundManager.Ins.ultiSound);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(attackEffect, collision.transform.position, Quaternion.identity);
            collision.SendMessage("ReceiveDamage", randomDame);

            GameManager.instance.ShowText(randomDame.ToString(), 70, Color.red,
                collision.gameObject.transform.position, Vector3.up * 50, 0.5f);

            SoundManager.Ins.PlaySound(kickSounds[randomKickSound]);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(attackEffect, collision.transform.position, Quaternion.identity);
            collision.SendMessage("ReceiveDamage", randomDame);
            SoundManager.Ins.PlaySound(SoundManager.Ins.breakCrateSound);

        }
    }

    //Ultimate Effect
    public IEnumerator Ultimate()
    {
        yield return new WaitForSeconds(0.3f);
        RaycastHit2D[] hitInfo = Physics2D.RaycastAll(attackPoint.position, attackPoint.right, 10f, enemyLayer);
        if (hitInfo != null && hitInfo.Length >0)
        {
            //Take dame 
            for (int i = 0; i < hitInfo.Length; i++)
            {
                if (hitInfo[i].collider.gameObject.tag == "Obstacle")
                {
                    Crate crate = hitInfo[i].transform.GetComponent<Crate>();
                    Instantiate(deathEffect, crate.transform.position, Quaternion.identity);
                    crate.ReceiveDamage(dameUlti);

                } 

                else if (hitInfo[i].collider.gameObject.tag == "Enemy")
                {
                    hitInfo[i].collider.gameObject.SendMessage("ReceiveDamage", dameUlti);
                    GameManager.instance.ShowText(dameUlti.ToString(), 100, Color.red,
                        hitInfo[i].transform.position, Vector3.up * 70, 0.5f);
                }
                Instantiate(impactEffect, hitInfo[i].point, Quaternion.identity);
            }
        }

        //Effect
        Color color1 = new Color(255f, 255f, 255f, 100f);
        Color color2 = new Color(0, 100, 255f, 0f);
        lineRenderer.startColor = color1;
        lineRenderer.endColor = color2;
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, lineRenderer.transform.position + lineRenderer.transform.right * 10);

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }

    public void CreateDust()
    {
        dust.Play();
    }

    //Update Energy Ultimate
    public void UPdateEnergy()
    {
        timeCooldownUlti += Time.deltaTime;

        if (timeCooldownUlti >= timeLimitUlti)
            timeCooldownUlti = timeLimitUlti;

        float ratio = timeCooldownUlti / timeLimitUlti;
        GameManager.instance.energyBar.localScale = new Vector3(ratio, 1, 1);
    }
}
