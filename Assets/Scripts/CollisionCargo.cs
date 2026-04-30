using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CollisionCargo : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] spawnPrefabs;
    public float spawnOffset = 0.5f;
    public float spawnedObjectLifetime = 5f;

    public GameObject objectToDestroy;
    public GameObject destroyVFX;

    [Tooltip("Максимальная прочность груза")]
    public float maxHealth = 100f;

    [Tooltip("Минимальная скорость, при которой начинается урон")]
    public float minDamageSpeed = 5f;

    [Tooltip("Множитель урона от скорости")]
    public float damageMultiplier = 2f;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    [Header("Scene Reload")]
    public float reloadDelay = 0.5f;

    [Header("Destruction")]
    public FakeBreak fakeBreak;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obsticle"))
        {
            float speed = collision.relativeVelocity.magnitude;

            if (speed > minDamageSpeed)
            {
                float damage = (speed - minDamageSpeed) * damageMultiplier;

                currentHealth -= damage;
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

                UpdateHealthUI();

                if (spawnPrefabs != null && spawnPrefabs.Length > 0)
                {
                    int count = Random.Range(2, 5);

                    for (int i = 0; i < count; i++)
                    {
                        GameObject prefabToSpawn =
                            spawnPrefabs[Random.Range(0, spawnPrefabs.Length)];

                        Vector3 randomOffset = Random.insideUnitSphere * spawnOffset;
                        Vector3 spawnPos =
                            collision.GetContact(0).point + randomOffset;

                        GameObject obj = Instantiate(prefabToSpawn, spawnPos, Random.rotation);

                        Destroy(obj, spawnedObjectLifetime);
                    }
                }

                if (currentHealth <= 0f)
                {
                    DestroyCargo();
                }
            }
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            float percent = (currentHealth / maxHealth) * 100f;
            healthText.text = $"Запас прочности: {percent:F0}%";

            if (percent > 75f) healthText.color = Color.green;
            else if (percent > 50f) healthText.color = Color.yellow;
            else if (percent > 25f) healthText.color = new Color(1f, 0.5f, 0f);
            else healthText.color = Color.red;
        }
    }

    void DestroyCargo()
    {
        Debug.Log("Груз уничтожен!");

        if (fakeBreak != null)
        {
            fakeBreak.Break();
        }

        if (destroyVFX != null)
        {
            GameObject vfx = Instantiate(
                destroyVFX,
                transform.position + Vector3.up * 1f,
                Quaternion.identity
            );

            Destroy(vfx, 3f);
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        StartCoroutine(ReloadSceneWithDelay());
    }

    IEnumerator ReloadSceneWithDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}