using UnityEngine;
using System.Collections;

public class SpawnerCenouras: MonoBehaviour
{
    public GameObject prefabCenoura;
    public GameObject prefabPedra;
    public Transform[] pontosSpawn;
    public float intervalo = 2f;
    [Range(0f, 1f)]
    public float chancePedra = 0.3f;

    void Start()
    {
        StartCoroutine(SpawnObjetos());
    }

    IEnumerator SpawnObjetos()
    {
        while (true)
        {
            if (pontosSpawn.Length > 0)
            {
                Transform spawn = pontosSpawn[Random.Range(0, pontosSpawn.Length)];

                GameObject prefabEscolhido = Random.value < chancePedra ? prefabPedra : prefabCenoura;
                Instantiate(prefabEscolhido, spawn.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(intervalo);
        }
    }
}
