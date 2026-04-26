using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int levelNum = 1;
        if (int.TryParse(levelName[5].ToString(), out levelNum))
        {
            SceneManager.LoadScene($"Level{levelNum}Cinematic");
        }
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
