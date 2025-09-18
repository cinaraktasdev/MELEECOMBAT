using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UselessNPC : MonoBehaviour
{
    public Transform InterectionkPoint;
    public float InterectionRange = 0.5f;
    public LayerMask PlayerLayer;

    public string[] Sentences = new string[1];
    public TMP_Text Sentence;
    public GameObject Sentencebox;
    private int SentenceDecider = 0;

    private bool once;

    void Start()
    {
        once = true;
    }
    void Update()
    {
        Collider2D interact = Physics2D.OverlapCircle(InterectionkPoint.position, InterectionRange, PlayerLayer); 
                                                                                                                  

        if (once && interact != null) //eğer alanda bir obje varsa
        {
            SentenceDecider = Random.Range(0, Sentences.Length);
            Sentence.text = Sentences[SentenceDecider];

            StartCoroutine(SentenceUsed());

            once = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (InterectionkPoint == null)
            return;

        Gizmos.DrawWireSphere(InterectionkPoint.position, InterectionRange);
    }

    private IEnumerator SentenceUsed()
    {
        Sentencebox.SetActive(true);
        yield return new WaitForSeconds(3f);
        Sentencebox.SetActive(false);

    }
}
