using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InstructionUI : MonoBehaviour
{
    [SerializeField] string nextScene;

    [SerializeField] bool animateContinueText = false;
    [SerializeField] float animateContinueDelay = 0.5f;
    [SerializeField] TextMeshProUGUI continueText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animateContinueText)
        {
            if (continueText == null)
            {
                Debug.LogError("Animate continue text is enabled but no text assigned!");
                return;
            }

            StartCoroutine(AnimateContinue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(InputValue value)
    {
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator AnimateContinue()
    {
        while (true)
        {
            const int numDots = 3;
            for (int i = 0; i <= numDots; i++)
            {
                continueText.text = "Click anywhere to continue" + new string('.', i);
                yield return new WaitForSeconds(animateContinueDelay);
            }
        }
    }
}
