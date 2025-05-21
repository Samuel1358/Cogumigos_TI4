using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    [SerializeField] private GameObject shadow;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float baseScale = 1f;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float shadowHeight = 0.1f;

    private void Start()
    {
        if (shadow != null)
        {
            shadow.transform.localScale = new Vector3(baseScale, baseScale, 1);
            // Rotaciona o quad para ficar paralelo ao chão
            shadow.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }

    private void LateUpdate()
    {
        if (shadow == null) return;

        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        
        if (Physics.Raycast(rayStart, Vector3.down, out hit, maxDistance, groundLayer))
        {
            // Posiciona a sombra exatamente no ponto de impacto
            shadow.transform.position = hit.point + Vector3.up * shadowHeight;
            
            // Calcula a escala baseada na distância
            float distanceRatio = hit.distance / maxDistance;
            float scale = Mathf.Lerp(baseScale, minScale, distanceRatio);
            
            // Aplica a escala
            shadow.transform.localScale = new Vector3(scale, scale, 1);
            shadow.SetActive(true);
        }
        else
        {
            shadow.SetActive(false);
        }
    }
}