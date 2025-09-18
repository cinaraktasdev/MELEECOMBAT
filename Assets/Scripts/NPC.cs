using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField] Bandit banditCode;

    public Transform InterectionkPoint;
    public float InterectionRange = 0.5f;
    public LayerMask PlayerLayer;

    public bool isInteract;
    public GameObject Talk;
    public GameObject TalkScreen;
    public GameObject books;
    public GameObject booksQuit;
    public string [] GreatingsNPC = new string[1];
    public string [] Answer = new string[1];
    public TMP_Text nameOfBarMan;
    public TMP_Text GreatingsOfBarMan;
    public TMP_Text AnswerToBarMan;
    private int greatDecider = 0;
    private int answerDecider = 0;
    public bool randomName = false;


    void Start()
    {
        Talk.SetActive(false);
        isInteract = false;
        banditCode = FindObjectOfType<Bandit>();

        if (randomName == true)
        {

            greatDecider = Random.Range(0, GreatingsNPC.Length);
            answerDecider = Random.Range(0, Answer.Length);

            GreatingsOfBarMan.text = GreatingsNPC[greatDecider];
            AnswerToBarMan.text = Answer[answerDecider];
        }
    }
    void Update()
    {
        Collider2D interact = Physics2D.OverlapCircle(InterectionkPoint.position, InterectionRange, PlayerLayer); //sadece Player layerına sahip colliderleri algılar
        //Collider2D[] dizisi olarak kullansaydım yani alana birden çok şeyin aynı anda girmesine izin verseydim OverLapCircleAll kullanırdım

        if(interact != null) //eğer alanda bir obje varsa
        {
            Talk.SetActive(true);
            isInteract = true;
            banditCode.interactTutorial.SetActive(true);

            banditCode.currentNPC = this;

            if (Input.GetKeyDown(KeyCode.E) && banditCode.currentNPC == this)
            {
                Talk.SetActive(false);
                banditCode.interactTutorial.SetActive(false);
                TalkScreen.SetActive(true);
                banditCode.stopscenes = true;
            }
        }
        else
        {
            Talk.SetActive(false);
            isInteract = false;

            if (banditCode.currentNPC == this)
            {
                banditCode.currentNPC = null;
                banditCode.interactTutorial.SetActive(false);
            }
        }

        if (books && Input.GetKeyDown(KeyCode.Q))
        {
            books.SetActive(false);
            booksQuit.SetActive(false);
            TalkScreen.SetActive(false);
            Talk.SetActive(true);
            banditCode.interactTutorial.SetActive(true);
            banditCode.stopscenes = false;
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (InterectionkPoint == null)
            return;

        Gizmos.DrawWireSphere(InterectionkPoint.position, InterectionRange);
    }

    public void OpenBooks()
    {
        TalkScreen.SetActive(false);
        books.SetActive(true);
        booksQuit.SetActive(true);
    }
    public void EndTalk()
    {
        TalkScreen.SetActive(false);
        Talk.SetActive(true);
        banditCode.interactTutorial.SetActive(true);
        banditCode.stopscenes = false;

    }
}
