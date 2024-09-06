using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleBtns:MonoBehaviour
{
    public Button startBtn;
    public Button exitBtn;

    public void ExitGame()
    {
        Application.Quit();
    }
}