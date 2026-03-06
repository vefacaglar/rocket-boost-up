using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;

        }
    }

    void StartSuccessSequence()
    {
        DisablePlayerMovement();
        Invoke("LoadNextLevel", 2f);
    }

    void StartCrashSequence()
    {

        // play crash sound

        // play crash particle effect

        // disable player movement
        DisablePlayerMovement();

        // reload level after delay
        Invoke("ReloadLevel", 2f);
    }

    void DisablePlayerMovement()
    {
        var playerMovement = GetComponent<Movement>();
        playerMovement.stopMovement = true;
    }

    void ReloadLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        var nextScene = currentScene + 1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }
}
