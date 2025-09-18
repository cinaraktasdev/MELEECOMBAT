using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAI : MonoBehaviour
{
    public Bandit bandit;
    
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject player;
    public float speed;
    public float distanceBetween;
    float distance;

    private bool isAttacking = false;

    public float attackCooldown = 1.0f; // Saldırıların arası 1 saniye
    private float lastAttackTime = 0f;

    public int EnemieAttackDamage = 20;

    public int EnemiesmaxHealth = 100;
    int EnemiescurrentHealth;

    bool isDead = false;

    public Transform EnemyAttackPoint;
    public float EnemyAttackRange = 0.5f;
    public LayerMask playerLayers;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        EnemiescurrentHealth = EnemiesmaxHealth;
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float directionX = player.transform.position.x - transform.position.x;
        float directionY = player.transform.position.y - transform.position.y;


        if (Mathf.Abs(directionX) > Mathf.Abs(directionY) && isDead == false)
        {
            // X ekseninde hareket ediyorsa
            if (directionX > 0 && EnemiescurrentHealth > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Oyuncunun sağında, düşman sağa dönsün
            else
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Oyuncunun solunda, düşman sola dönsün
        }
        else
        {
            // Y ekseninde hareket ediyorsa
            if (directionY > 0 && EnemiescurrentHealth > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Oyuncu yukarıdaysa, düşman yukarıya baksın
            else
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Oyuncu aşağıdaysa, düşman aşağıya baksın
        }




        if (distance > 0.8 && isDead == false)
        {
            if (distance < distanceBetween)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                
                int newAnimState = 2; // Varsayılan animasyon durumu, örneğin Idle

                if (distance <= 0.8)
                {
                    newAnimState = 0; // Koşma animasyonu durumu
                }

                // Eğer animasyon durumu değiştiyse güncelle
                if (animator.GetInteger("AnimState") != newAnimState)
                {
                    animator.SetInteger("AnimState", newAnimState);
                }
            }
        }
        else
        {
            // Hedef 0.8 birimden küçük mesafedeyse, animasyonu durdur ve hareketi sıfırla
            animator.SetInteger("AnimState", 0); // Idle animasyon durumu
            transform.position = this.transform.position; // Hareketi durdur
        }


        if (distance <= 0.8f && isDead == false && bandit.CurrentHealth > 0)
        {
            // Eğer saldırı yapmıyorsa veya bekleme süresi dolmuşsa
            if (!isAttacking || Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetTrigger("Attack"); // Saldırı animasyonunu başlat
                lastAttackTime = Time.time; // Son saldırı zamanını güncelle
                isAttacking = true; // Saldırı moduna geç

                Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(EnemyAttackPoint.position, EnemyAttackRange, playerLayers);
                
                foreach (Collider2D player in hitPlayer)
                {
                    player.GetComponent<Bandit>().PlayerTakeDamage(EnemieAttackDamage);
                }

            }
        }
        else
        {
            isAttacking = false; // Düşman uzaklaştığında tekrar saldırı yapılabilir
        }
    }

    public void TakeDamage(int damage)
    {
        EnemiescurrentHealth -= damage;
        StartCoroutine(damaged());
    }

    void enemieDies()
    {

        StartCoroutine(deadass());
    }

    private IEnumerator damaged()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("Hurt");

        if (EnemiescurrentHealth <= 0 && isDead == false)
        {
            enemieDies();
        }
        
    }

    private IEnumerator deadass()
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("Death");
        isDead = true;

        // Death animasyonu tamamlandıktan sonra Animator bileşenini devre dışı bırak
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
        GetComponent<Collider2D>().enabled = false;

    }

    private void OnDrawGizmosSelected()
    {
        if (EnemyAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(EnemyAttackPoint.position, EnemyAttackRange);
    }
}
