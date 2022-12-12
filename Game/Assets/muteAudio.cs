using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muteAudio : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.waveCount == waveManager.instance.waves.Length)
        {
            Destroy(gameObject);
        }
    }
}
