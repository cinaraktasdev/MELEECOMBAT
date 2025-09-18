using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Bandit : MonoBehaviour {

    [SerializeField] float speed = 4.0f;
    [SerializeField] private NPC npcCode;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private bool attacking = false;
    public bool stopscenes;

    public GameObject blackScreen;
    public GameObject OpeningScreen;

    public float attackCooldown = 1.0f; // Saldırı arasındaki bekleme süresi
    private float lastAttackTime = -1f; // İlk saldırının anında yapılabilmesi için -1 ile başlatıyoruz
    public float holdDuration = 1f;
    public Image fillSlider;
    public GameObject ExitFiller;
    private float holdTimer = 0;
    public float decreaseSpeed = 1.0f; // Geri azalma hızı
    private bool isDecreasing = false;


    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private Vector2 previousVelocity = Vector2.zero;

    public int attackDamage = 20;

    public int maxHealth = 100;
    int currentHealth;

    [HideInInspector] public NPC currentNPC;
    public GameObject interactTutorial;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        npcCode = FindObjectOfType<NPC>();

        currentHealth = maxHealth;

        OpeningScreen.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

        if(stopscenes != true)
        {
            float inputX = Input.GetAxis("Vertical");
            float inputY = Input.GetAxis("Horizontal");


            if (currentHealth > 0)
            {
                //Attack
                if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                }
            }


            if (previousVelocity == Vector2.zero) // Hızı bir kez kaydetmek için
            {
                previousVelocity = rb.velocity;
            }
            rb.velocity = Vector2.zero;


            if (!attacking && !isDead)
            {

                if (previousVelocity != Vector2.zero)
                {
                    rb.velocity = previousVelocity;
                    previousVelocity = Vector2.zero; // Önceki hız geri yüklendiği için sıfırla
                }

                int newAnimState = 0; // Varsayılan animasyon durumu, örneğin Idle

                if (Mathf.Abs(inputX) > Mathf.Epsilon || Mathf.Abs(inputY) > Mathf.Epsilon)
                {
                    newAnimState = 2; // Koşma animasyonu durumu
                }

                // Eğer animasyon durumu değiştiyse güncelle
                if (animator.GetInteger("AnimState") != newAnimState)
                {
                    animator.SetInteger("AnimState", newAnimState);
                }

                // Swap direction of sprite depending on walk direction
                if (inputX > 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (inputX < 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                if (inputY > 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (inputY < 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                // Move
                rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
                rb.velocity = new Vector2(inputY * speed, rb.velocity.x);
            }

            if (Input.GetKey(KeyCode.Tab))
            {
                isDecreasing = false;
                ExitFiller.SetActive(true);
                holdTimer += Time.deltaTime;
                fillSlider.fillAmount = holdTimer / holdDuration;

                if (holdTimer >= holdDuration)
                {
                    if (SceneManager.GetActiveScene().name == "MantarysKeep" || SceneManager.GetActiveScene().name == "MantarysSmith")
                    {
                        saveBool("LeavingMantarys", true);
                    }
                    if (SceneManager.GetActiveScene().name == "BeuclairKeep" || SceneManager.GetActiveScene().name == "BeuclairTavern")
                    {
                        saveBool("LeavingBeuclair", true);
                    }
                    if (SceneManager.GetActiveScene().name == "ElriyaKeep" || SceneManager.GetActiveScene().name == "ElriyaLibrary")
                    {
                        saveBool("LeavingElriya", true);
                    }

                    //iki sahne arası sayısal veri saklamak için PlayerPrefs.get / .set komutalrını kullan
                    //

                    StartCoroutine(Leaving());
                    blackScreen.SetActive(true);
                }
            }
            // Tuş bırakıldığında
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                isDecreasing = true;
            }

            if (isDecreasing)
            {
                holdTimer -= decreaseSpeed * Time.deltaTime;
                holdTimer = Mathf.Clamp(holdTimer, 0, holdDuration); // holdTimer'ı 0 ile holdDuration arasında sınırla
                fillSlider.fillAmount = holdTimer / holdDuration;

                // Tamamen boşaldığında geri azalmayı durdur
                if (holdTimer <= 0)
                {
                    isDecreasing = false;
                    ExitFiller.SetActive(false);
                }
            }

            StartCoroutine(opening());
        }

        if(stopscenes == true)
        {
            animator.SetInteger("AnimState", 0);
            rb.constraints |= RigidbodyConstraints2D.FreezePosition;
            /*
             
            |= Operatörü (OR):
            rb.constraints |= RigidbodyConstraints2D.FreezePosition ifadesi, mevcut kısıtlamalara pozisyon dondurmayı ekler.
            Örneğin, FreezeRotation zaten etkinse, FreezePosition eklenecek ama FreezeRotation korunacak.
             
             */
        }
        else
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;

            /*
            &= ~ Operatörü (AND NOT):
            rb.constraints &= ~RigidbodyConstraints2D.FreezePosition ifadesi, yalnızca FreezePosition kısıtlamasını kaldırır. Diğer kısıtlamalar etkilenmez.
            */
        }

    }

    void Attack()
    {

        animator.SetTrigger("Attack");
        attacking = true;
        StartCoroutine(Attacking());
        lastAttackTime = Time.time; // Son saldırı zamanını güncelle

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BanditAI>().TakeDamage(attackDamage);
        }
    }
    private IEnumerator Attacking()
    {
        attacking = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.7f);
        attacking = false;

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void PlayerTakeDamage(int pdamage)
    {
        
        if (isDead == false)
        {
            currentHealth -= pdamage;
            StartCoroutine(damagedP());
        }
    }

    private IEnumerator damagedP()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0 && isDead == false)
        {
            PlayerDies();
        }


    }

    void PlayerDies()
    {
        
        StartCoroutine(Pdeadass());
    }

    private IEnumerator Pdeadass()
    {
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("Death");
        isDead = true;

        // Death animasyonu tamamlandıktan sonra Animator bileşenini devre dışı bırak
        yield return new WaitForSeconds(0.1f);
        animator.enabled = false;
        GetComponent<Collider2D>().enabled = false;

    }
    public int CurrentHealth
    {
        get { return currentHealth; }
    }
    private IEnumerator opening()
    {
        yield return new WaitForSeconds(0.5f);
        OpeningScreen.SetActive(false);
    }

    private IEnumerator Leaving()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    public void saveBool(string key, bool LeavingBool)
    {
        PlayerPrefs.SetInt(key, LeavingBool ? 1 : 0);
        PlayerPrefs.Save();
    }


}
