using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CollisionCargo : MonoBehaviour
{
    [Header("Cargo Settings")]
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
    public FakeBreak fakeBreak; // 👈 ВАЖНО ДОБАВИЛИ

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

        // разрушение
        if (fakeBreak != null)
        {
            fakeBreak.Break();
        }

        // 🔥 VFX (улучшенный)
        if (destroyVFX != null)
        {
            GameObject vfx = Instantiate(
                destroyVFX,
                transform.position + Vector3.up * 1f,
                Quaternion.identity
            );

            // если это не ParticleSystem — удаляем через время
            Destroy(vfx, 3f);
        }

        // отключаем коллайдер
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // перезагрузка сцены
        StartCoroutine(ReloadSceneWithDelay());
    }

    IEnumerator ReloadSceneWithDelay()
    {
        yield return new WaitForSeconds(reloadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}