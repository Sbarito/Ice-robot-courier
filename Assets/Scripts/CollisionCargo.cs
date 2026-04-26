using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CollisionCargo : MonoBehaviour
{
    public GameObject objectToDestroy;
    public GameObject destroyVFX;
    public float reloadDelay = 0.5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obsticle"))
        {
            float speed = collision.relativeVelocity.magnitude;

            if (speed > 20f)
            {
                Debug.Log("Сильное столкновение с препятствием!");

                Instantiate(destroyVFX, transform.TransformPoint(Vector3.up * 2f), transform.rotation);
                Destroy(objectToDestroy);

                StartCoroutine(ReloadSceneWithDelay());
            }
        }
    }

    IEnumerator ReloadSceneWithDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}