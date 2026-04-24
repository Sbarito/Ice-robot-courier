using UnityEngine;
using UnityEngine.SceneManagement;

public class BreakOnHit : MonoBehaviour
{
    public float hitThreshold = 0.01f;

    private GameObject cargoObject;

    void Start()
    {
        Transform t = transform.Find("Cargo");

        if (t != null)
        {
            cargoObject = t.gameObject;
            Debug.Log("Cargo найден: " + cargoObject.name);
        }
        else
        {
            Debug.LogWarning("Cargo НЕ найден у Robot!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float speed = collision.relativeVelocity.magnitude;

        Debug.Log("Удар о: " + collision.gameObject.name);
        Debug.Log("Сила удара: " + speed);

        if (cargoObject == null)
        {
            Debug.LogWarning("Груз уже отсутствует или не найден");
            return;
        }

        if (speed >= hitThreshold)
        {
            Destroy(cargoObject);
            cargoObject = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}