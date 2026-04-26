using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCargo : MonoBehaviour
{
    public GameObject objectToDestroy;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obsticle"))
        {
            float speed = collision.relativeVelocity.magnitude;

            if (speed > 20f)
            {
                Debug.Log("Сильное столкновение с препятствием!");

                Destroy(objectToDestroy);

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}