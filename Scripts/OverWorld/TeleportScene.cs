using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScene : MonoBehaviour
{
    [Tooltip("Tag required for the object to trigger teleport (e.g., 'Player')")]
    public string requiredTag = "Player";

    [Tooltip("Name of the scene to load (case-sensitive).")]
    public string sceneToLoad = "OverWorld";

    private void Reset()
    {
        int israelLayer = LayerMask.NameToLayer("Israel");
        if (israelLayer != -1)
        {
            gameObject.layer = israelLayer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}