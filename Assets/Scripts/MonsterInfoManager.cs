using UnityEngine;

public class MonsterInfoManager : MonoBehaviour
{
    public MonsterInfoPopup infoPopup;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Monster monster = hit.collider.GetComponent<Monster>();
                if (monster != null)
                {
                    ShowMonsterInfo(monster);
                }
            }
        }
    }

    void ShowMonsterInfo(Monster monster)
    {
        if (infoPopup != null)
        {
            infoPopup.ShowMonsterInfo(monster.monsterData);
        }
    }
}
