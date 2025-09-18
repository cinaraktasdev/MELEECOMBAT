using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditRandomMove : MonoBehaviour
{
    public float moveSpeed = 2f; // Hareket hızı
    public Vector2 wanderCenter; //gezilecek bölge
    public float wanderRadius = 5f; // Dolaşılacak alanın yarıçapı
    public float waitTime = 2f; // Her pozisyonda ne kadar bekleyeceği

    private Vector2 targetPosition; // Hedef pozisyon
    private bool isMoving; // Hareket edip etmediğini kontrol etmek için
    bool GoOn;

    void Start()
    {
        StartCoroutine(Wander());
        GoOn = true;
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            targetPosition = wanderCenter + Random.insideUnitCircle * wanderRadius;

            if (GoOn)
            {

                isMoving = true; // Hareket etmeye başla

            }

            while (isMoving)
            {
                if (GoOn)
                {
                    float step = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
                }

                // Hedef pozisyona ulaştıysa
                if ((Vector2)transform.position == targetPosition)
                {
                    isMoving = false; // Hareketi durdur
                }

                yield return null; // Bir sonraki frame'e geç
            }

            // Belirli bir süre bekle
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GoOn = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Renk seçimi
        Gizmos.DrawWireSphere(wanderCenter, wanderRadius); // Dolaşılacak alanı çizin
    }

}
