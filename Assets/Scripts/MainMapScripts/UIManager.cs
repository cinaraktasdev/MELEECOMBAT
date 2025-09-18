using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InMapMovement mapCode;

    private int n;
    public GameObject closingScreen;

    private void Start()
    {
        mapCode = FindObjectOfType<InMapMovement>();
    }

    public void LeaveBeuclair()
    {
        mapCode.gameObject.transform.position = new Vector3(6.5f, -5.8f, 0f);

        mapCode.stopscene = false;

        mapCode.BeuclairUI.SetActive(false);

        mapCode.gameObject.SetActive(true);
    }
    public void LeaveElriya()
    {
        mapCode.gameObject.transform.position = new Vector3(-26f, -19.4f, 0f);

        mapCode.stopscene = false;

        mapCode.ElriyaUI.SetActive(false);

        mapCode.gameObject.SetActive(true);
    }

    public void LeaveMantarys()
    {
        mapCode.gameObject.transform.position = new Vector3(-19.4f, 18.8f, 0f);

        mapCode.stopscene = false;

        mapCode.MantarysUI.SetActive(false);

        mapCode.gameObject.SetActive(true);
    }

    public void MantarysKeep()
    {
        n = 1;
        StartCoroutine(changingScenes());
    }
    public void BeuclairKeep()
    {
        n = 2;
        StartCoroutine(changingScenes());
    }
    public void ElriyaKeep()
    {
        n = 3;
        StartCoroutine(changingScenes());
    }
    public void MantarysSmith()
    {
        n = 4;
        StartCoroutine(changingScenes());
    }
    public void BeuclairTavern()
    {
        n = 5;
        StartCoroutine(changingScenes());
    }
    public void ElriyaLibrary()
    {
        n = 6;
        StartCoroutine(changingScenes());
    }

    private IEnumerator changingScenes()
    {
        closingScreen.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(n);
    }


}
