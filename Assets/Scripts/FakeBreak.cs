using UnityEngine;

public class FakeBreak : MonoBehaviour
{
    public GameObject[] debrisPrefabs; // несколько префабов

    public int debrisCount = 8;
    public float explosionForce = 4f;
    public float radius = 2f;

    public void Break()
    {
        for (int i = 0; i < debrisCount; i++)
        {
            // выбираем 
            GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];

            GameObject piece = Instantiate(
                prefab,
                transform.position + Random.insideUnitSphere * 0.2f,
                Random.rotation
            );

            Rigidbody rb = piece.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    radius
                );
            }

            Destroy(piece, 3f);
        }

        Destroy(gameObject);
    }
}