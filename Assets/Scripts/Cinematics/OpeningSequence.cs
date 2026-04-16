using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSequence : MonoBehaviour
{
    [SerializeField] string nextSceneName = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
