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
                LoadNextLevel();
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;

        }
    }

    void StartCrashSequence()
    {

        // play crash sound

        // play crash particle effect

        // disable player movement
        var playerMovement = GetComponent<Movement>();
        playerMovement.stopMovement = true;

        // reload level after delay
        Invoke("ReloadLevel", 2f);
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
