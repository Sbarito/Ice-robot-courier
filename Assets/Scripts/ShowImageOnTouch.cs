using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowImageOnTouch : MonoBehaviour
{
    public Image targetImage;
    public GameObject[] hideWhileActiveUI;
    public string sceneName = "level_1";

    private bool isLoading = false;

    private void Start()
    {
        if (targetImage != null)
            targetImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            StartCoroutine(LoadLevel(other));
        }
    }

    private IEnumerator LoadLevel(Collider player)
    {
        isLoading = true;

        SetUIActive(false);

        if (targetImage != null)
            targetImage.gameObject.SetActive(true);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

       yield return new WaitForSeconds(2f);


       SceneManager.LoadScene(sceneName);
    }

    private void SetUIActive(bool state)
    {
        if (hideWhileActiveUI == null) return;

        foreach (GameObject ui in hideWhileActiveUI)
        {
            if (ui != null)
                ui.SetActive(state);
        }
    }
}