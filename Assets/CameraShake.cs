using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f; // The duration of the shake, in seconds
    public float shakeMagnitude = 0.01f; // The magnitude of the shake

    private float timer = 0f;

    public TextMeshProUGUI scoreText;
    
    void Update()
    {
        if (timer > 0f)
        {
            // Generate random rotation values
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            float z = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply the rotation to the camera's transform component
            transform.localRotation = Quaternion.Euler(x, y, z);

            // Decrement the timer
            timer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera's rotation to its default value
            transform.localRotation = Quaternion.identity;
        }
    }

    public void Shake()
    {
        timer = shakeDuration;
        scoreText.color = Color.red;
        scoreText.transform.localScale = new Vector3(scoreText.transform.localScale.x + 0.1f, scoreText.transform.localScale.y + 0.1f, scoreText.transform.localScale.z + 0.1f);
        Invoke(nameof(Return), 0.5f);
    }

    void Return()
    {
        scoreText.color = Color.black;
        scoreText.transform.localScale = new Vector3(scoreText.transform.localScale.x - 0.1f, scoreText.transform.localScale.y - 0.1f, scoreText.transform.localScale.z - 0.1f);
    }
    
}