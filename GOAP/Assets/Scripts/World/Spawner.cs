using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public int numPatients;

    void Start()
    {
        Invoke("SpawnObject", 5.0f);
    }

    void SpawnObject()
    {
        Instantiate(prefab, this.transform.position, Quaternion.identity);

        Invoke("SpawnObject", Random.Range(2.0f, 10.0f));
    }
}