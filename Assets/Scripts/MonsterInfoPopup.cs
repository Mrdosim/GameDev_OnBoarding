using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPopup : MonoBehaviour
{
    public GameObject popup;
    public Text nameText;
    public Text gradeText;
    public Text speedText;
    public Text healthText;

    private MonsterData currentMonsterData;

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
    }

    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}

