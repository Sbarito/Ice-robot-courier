using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCrash : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}