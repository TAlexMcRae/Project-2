using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockwave : MonoBehaviour
{
    public int points;
    public float maxRadius;
    public float speed;
    public float startWidth;
    public float knockBack;

    private LineRenderer lineRenderer;

    [SerializeField] bossBearAI bossBearAI;
    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = points + 1;
    }
    private void Update()
    {
        
    }
    private IEnumerator Blast()
    {
        float currentRadius = 0f;

        while(currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / points;

        for (int i = 0; i <= points; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }
        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }
    private void Damage(float currentRadius)
    {
        Collider[] hittingObjects = Physics.OverlapSphere(transform.position, currentRadius);

        for (int i = 0; i < hittingObjects.Length; i++)
        {
            Rigidbody rb = hittingObjects[i].GetComponent<Rigidbody>();
            if (!rb)
                continue;
            Vector3 direction = (hittingObjects[i].transform.position - transform.position).normalized;
            rb.AddForce(direction * knockBack, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        bossBearAI.swDamage();
    }
}
