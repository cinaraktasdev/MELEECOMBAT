using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InMapMovement : MonoBehaviour
{
    public float speed = 10f;

    Rigidbody2D rb;

    Vector2 lastClickedPos;

    bool moving;

    public Transform arrow; // Karakterin üzerindeki oku buraya ekleyin
    private Camera mainCamera;

    private CameraZoom cmz;

    public bool stopscene;

    public GameObject WarSwords;
    public GameObject cross;
    public GameObject BeuclairUI;
    public GameObject ElriyaUI;
    public GameObject MantarysUI;
    public GameObject OpeningScreen;

    public GameObject[] BanditFights;

    void Start()
    {
        mainCamera = Camera.main;
        cross.SetActive(false);
        stopscene = false;
        cmz = FindObjectOfType<CameraZoom>();
        rb = GetComponent<Rigidbody2D>();
        OpeningScreen.SetActive(true);

        lastClickedPos = transform.position;
        moving = false;

        if (SceneManager.GetActiveScene().name == "Main")
        {
            if (LoadBool("LeavingMantarys"))
            {
                gameObject.transform.position = new Vector3(-19.4f, 18.8f, 0f);
                PlayerPrefs.SetInt("LeavingMantarys", 0); // Pozisyonlandıktan sonra sıfırla
            }
            else if (LoadBool("LeavingBeuclair"))
            {
                gameObject.transform.position = new Vector3(6.5f, -5.8f, 0f);
                PlayerPrefs.SetInt("LeavingBeuclair", 0); // Pozisyonlandıktan sonra sıfırla
            }
            else if (LoadBool("LeavingElriya"))
            {
                gameObject.transform.position = new Vector3(-26f, -19.4f, 0f);
                PlayerPrefs.SetInt("LeavingElriya", 0); // Pozisyonlandıktan sonra sıfırla
            }
            PlayerPrefs.Save(); // Değişiklikleri kaydet
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && stopscene == false)
        {
            Vector2 clickPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            // Raycast ile tıklanan pozisyonda bir nesne var mı diye kontrol et
            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("GroundTag") || hit.collider.CompareTag("bandits"))
            {
                lastClickedPos = clickPos;
                moving = true;

                cross.transform.position = lastClickedPos;
                cross.SetActive(true); // Görünür yap
            }
        }

        if(moving && (Vector2)transform.position != lastClickedPos && stopscene == false)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);

        }
        else
        {
            moving = false;
        }

        RotateArrowToMouse();
        StartCoroutine(opening());

        if (LoadBool("LeavingMantarys"))
        {
            gameObject.transform.position = new Vector3(-19.4f, 18.8f, 0f);

        }
        if (LoadBool("LeavingBeuclair"))
        {
            gameObject.transform.position = new Vector3(6.5f, -5.8f, 0f);
        }
        if (LoadBool("LeavingElriya"))
        {
            gameObject.transform.position = new Vector3(-26f, -19.4f, 0f);
        }
    }

    void RotateArrowToMouse()
    {
        Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bandits" && stopscene == false && cmz.zoomTarget <= 12)
        {
            foreach(GameObject BW in BanditFights)
            {
                BW.SetActive(false);
            }
            BanditFights[Random.Range(0, BanditFights.Length)].SetActive(true);

            stopscene = true;
            WarSwords.SetActive(true);
        }
        else if (collision.gameObject.tag == "SeaTag")
        {
            moving = false; // SeaTile ile çarpıştığında hareketi durdur
        }


        if(collision.gameObject.tag == "CityBeuclair")
        {
            stopscene = true;
            this.gameObject.SetActive(false);
            BeuclairUI.SetActive(true);
            moving = false;
        }
        if (collision.gameObject.tag == "CityElriya")
        {
            stopscene = true;
            this.gameObject.SetActive(false);
            ElriyaUI.SetActive(true);
            moving = false;
        }
        if (collision.gameObject.tag == "CityMantarys")
        {
            stopscene = true;
            this.gameObject.SetActive(false);
            MantarysUI.SetActive(true);
            moving = false;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SeaTag")
        {
            moving = false; // SeaTile ile üst üste dururken hareketi durdur
        }
    }

    private IEnumerator opening()
    {
        yield return new WaitForSeconds(0.5f);
        OpeningScreen.SetActive(false);
    }

    public bool LoadBool(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key) == 1;
        }
        return false;
    }

}
