using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shockwave : MonoBehaviour
{
    [SerializeField] bossBearAI bossBearAI;
    [SerializeField] private float knockbackStrength;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider collision)
    {
        bossBearAI.swDamage();
    }
}
