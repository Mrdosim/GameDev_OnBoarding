using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterInfoPopup : MonoBehaviour
{
    public GameObject popup;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI healthText;

    private MonsterData currentMonsterData;
    private Coroutine autoCloseCoroutine;

    void Start()
    {
        popup.SetActive(false);
    }

    public void ShowMonsterInfo(MonsterData data)
    {
        currentMonsterData = data;
        nameText.text = "이름: " + data.Name;
        gradeText.text = "등급: " + data.Grade;
        speedText.text = "속도: " + data.Speed;
        healthText.text = "체력: " + data.Health;
        popup.SetActive(true);

        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
        }
        autoCloseCoroutine = StartCoroutine(AutoClosePopup(2f));
    }

    IEnumerator AutoClosePopup(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClosePopup();
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}
