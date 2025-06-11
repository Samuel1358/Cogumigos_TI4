using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private bool hasHit = false;
    private Vector3 direction;
    private float destroyTime = 5f;
    private float spawnTime;

    public void Initialize(float arrowSpeed, Vector3 shootDirection)
    {
        speed = arrowSpeed;
        direction = shootDirection.normalized;
        spawnTime = Time.time;
        
        // Rotaciona a flecha para apontar na direção do tiro
        if (direction != Vector3.zero)
        {
            // Primeiro rotaciona para a direção do tiro
            // transform.rotation = Quaternion.LookRotation(direction);
            // Depois aplica a rotação de 90 graus no Z
            // transform.rotation *= Quaternion.Euler(0, 0, 90);
        }
    }

    void Update()
    {
        if (!hasHit)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // Destrói a flecha após 12 segundos
            if (Time.time - spawnTime >= destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;


        hasHit = true;
        Destroy(gameObject, 0.1f); // Destrói a flecha após a colisão
    }
} 