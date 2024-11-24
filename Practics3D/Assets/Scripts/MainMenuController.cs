using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void GoToObjectCreatorScene()
    {
        SceneManager.LoadScene("ObjectCreatorScene");
    }

    public void GoToObjectViewerScene()
    {
        SceneManager.LoadScene("ObjectViewerScene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }
}
