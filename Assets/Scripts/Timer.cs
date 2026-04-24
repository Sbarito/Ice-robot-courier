using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeLeft = 10f;

    public GameObject player;
    public GameObject[] worldObjects;

    bool gameOver = false;

    void Update()
    {
        if (gameOver) return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Время: " + Mathf.Ceil(timeLeft);
        }
        else
        {
            ExplodeWorld();
        }
    }

    void ExplodeWorld()
    {
        gameOver = true;

        if (player != null)
            player.SetActive(false);

        foreach (GameObject obj in worldObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        timerText.text = "GAME OVER";

        Invoke("RestartLevel", 2f);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}