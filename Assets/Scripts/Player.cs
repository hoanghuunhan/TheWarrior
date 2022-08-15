using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform attackPoint;
    public float speed = 5f;
    public float jumpForce = 300f;
    public int health = 100;
    public int maxHealth = 100;

    //Immunity
    float immunityTime = 1f;
    float x;

    public bool _isAlive = true;
    bool m_facingRight = true;

    Rigidbody2D rb;
    Animator anim;
    SensorGroundPlayer sensorGroundPlayer;

    private void Start()
    {
        //Time.timeScale = 0f;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sensorGroundPlayer = GetComponentInChildren<SensorGroundPlayer>();

    }

    private void Update()
    {
        immunityTime += Time.deltaTime;

        //Jump
        if (rb != null && _isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && sensorGroundPlayer._isGround == true)
            {
                anim.SetTrigger("Jump");
                rb.AddForce(Vector2.up * jumpForce);
                sensorGroundPlayer._isGround = false;
                anim.SetBool("IsGround", sensorGroundPlayer._isGround);
            }
        }

    }
    private void FixedUpdate()
    {

        x = Input.GetAxisRaw("Horizontal");

        //Flip
        if (x > 0 && !m_facingRight)
        {
            Flip();
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (x < 0 && m_facingRight)
        {
            Flip();
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        //Move
        if (_isAlive)
            transform.Translate(new Vector3(x * speed * Time.fixedDeltaTime, 0, 0));

        //Anim Running
        anim.SetFloat("Speed", Mathf.Abs(x));

        if (sensorGroundPlayer._isGround)
        {
            anim.SetBool("IsGround", sensorGroundPlayer._isGround);
        }
    }



    //Receive Damage
    public void ReceiveDamage(int dmg)
    {
        if (immunityTime <= 1f)
            return;

        anim.SetTrigger("Hurt");
        health -= dmg;
        GameManager.instance.OnHealthChange();

        if(SoundManager.Ins.hurtSound)
            SoundManager.Ins.PlaySound(SoundManager.Ins.hurtSound);

        if (health <= 0)
        {
            health = 0;
            StartCoroutine(Death());
        }
        immunityTime = 0;
    }


    void Flip()
    {
        m_facingRight = !m_facingRight;
        attackPoint.Rotate(0f, 180f, 0f);
        GetComponentInChildren<PlayerTakeDame>().lineRenderer.transform.Rotate(0f, 180f, 0f);
    }

    IEnumerator Death()
    {
        _isAlive = false;
        anim.SetBool("Dead", true);
        SoundManager.Ins.PlaySound(SoundManager.Ins.deadSound);
        SoundManager.Ins.PlaySound(SoundManager.Ins.failure);
        
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0f;
        GameManager.instance.GameOverPanel.SetActive(true);
    }

    //Healing Player
    public void Heal(int healPoint)
    {
        if (health == maxHealth)
            return;

        health += healPoint;
        if(health > maxHealth)
            health = maxHealth;

        GameManager.instance.ShowText("+" + healPoint.ToString() + "hp", 50, Color.green,
                transform.position, Vector3.up * 25, 0.5f);
        GameManager.instance.OnHealthChange();
    }
}
