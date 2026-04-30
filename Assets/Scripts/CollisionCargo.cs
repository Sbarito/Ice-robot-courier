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
    [Tooltip("TMP текст для отображения процента прочности")]
    public TextMeshProUGUI healthText;

    [Header("Scene Reload")]
    public float reloadDelay = 0.5f;

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

            // Проверяем минимальную скорость урона
            if (speed > minDamageSpeed)
            {
                // Вычисляем урон
                float damage = (speed - minDamageSpeed) * damageMultiplier;

                // Уменьшаем здоровье
                currentHealth -= damage;

                // Ограничиваем диапазон
                currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

                Debug.Log(
                    $"Столкновение! " +
                    $"Скорость: {speed:F1} | " +
                    $"Урон: {damage:F1} | " +
                    $"Прочность: {currentHealth:F1}"
                );

                // Обновляем UI
                UpdateHealthUI();

                // Проверяем уничтожение
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

            // Обновляем текст
            healthText.text = $"Запас прочности: {percent:F0}%";

            // Меняем цвет в зависимости от процента
            if (percent > 75f)
            {
                healthText.color = Color.green;
            }
            else if (percent > 50f)
            {
                healthText.color = Color.yellow;
            }
            else if (percent > 25f)
            {
                // Оранжевый цвет
                healthText.color = new Color(1f, 0.5f, 0f);
            }
            else
            {
                healthText.color = Color.red;
            }
        }
    }

    void DestroyCargo()
    {
        Debug.Log("Груз уничтожен!");

        // Эффект разрушения
        if (destroyVFX != null)
        {
            Instantiate(
                destroyVFX,
                transform.TransformPoint(Vector3.up * 2f),
                transform.rotation
            );
        }

        // Уничтожение объекта
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }

        // Перезагрузка сцены
        StartCoroutine(ReloadSceneWithDelay());
    }

    IEnumerator ReloadSceneWithDelay()
    {
        yield return new WaitForSeconds(reloadDelay);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}