using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScripts : MonoBehaviour
{
    public void goToMainGame()
    {
        SceneManager.LoadScene("Assets/Scenes/Protoype.unity");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public static void goToDeathScreen()
    {
        SceneManager.LoadScene("Assets/Scenes/FailMenu.unity");
    }

    public static void goToSuccesScree()
    {
        SceneManager.LoadScene("Assets/Scenes/SuccessMenu.unity");
    }
}
