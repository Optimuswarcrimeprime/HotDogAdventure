using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI; 

    public void TriggerGameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Tutorial"); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}