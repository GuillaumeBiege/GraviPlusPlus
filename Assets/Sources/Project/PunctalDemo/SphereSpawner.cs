using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [SerializeField] GameObject spherePrefab = default;
    [SerializeField] float lifeTime = 5f;
    public float spawnRate = 1f;

    float clacSpawnRate = 0f;
    float timer = 1f;

    

    // Update is called once per frame
    void Update()
    {
        clacSpawnRate = 1f / spawnRate;


        timer -= Time.deltaTime / clacSpawnRate;
        if (timer <= 0f)
        {
            SpawnSphere();
            timer = 1f;
        }
        
    }

    void SpawnSphere()
    {
        GameObject go = Instantiate<GameObject>(spherePrefab,transform.position, Quaternion.identity);
        StartCoroutine(KillSphere(go, lifeTime));
    }

    IEnumerator KillSphere(GameObject _sphere, float _lifetime)
    {
        //To avoid lifetime consumption on the first call
        yield return 0;
        yield return new WaitForSeconds(_lifetime);

        Destroy(_sphere);
    }
}
