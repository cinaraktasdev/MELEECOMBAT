using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;

    int attackIndex = 0; // 0 = ilk saldırı
    float lastClickTime;
    public float comboResetTime = 1f; // 1 saniye içinde devam edilmezse resetlensin


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // sol tık saldırı
        {
            // Eğer combo reset süresi geçtiyse başa dön
            if (Time.time - lastClickTime > comboResetTime)
            {
                attackIndex = 0;
            }

            lastClickTime = Time.time; //burda 

            // Animator'a indexi gönder
            animator.SetInteger("AttackIndex", attackIndex);
            animator.SetTrigger("AttackTrigger");

            // Combo ilerlet
            attackIndex++;
            if (attackIndex > 2) // 0,1,2 → toplam 3 saldırı
            {
                attackIndex = 0;
            }
        }
    }
}
