using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class Spawner : MonoBehaviour
{
    [SerializeField] private SpawnerData spawnerData;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private bool infinity = true;
    [SerializeField] Vector2Int numberOfSpawns = Vector2Int.one;
    [SerializeField] Vector2 delayÂetweenSpawns = Vector2.one;

    public UnityEvent startSpawnEvent;
    public UnityEvent stopSpawnEvent;
    public UnityEvent spawnPrefabEvent;

    private Coroutine currentCoroutine;

    private void Start()
    {
        if (playOnAwake) StartSpawn();
    }

    private IEnumerator StartSpawnCor()
    {
        while (infinity)
        {
            SpawnRandomPrefab();
            yield return new WaitForSeconds(Random.Range(delayÂetweenSpawns.x, delayÂetweenSpawns.y));
        }

        for (int i = 0; i < Random.Range(numberOfSpawns.x, numberOfSpawns.y); i++)
        {
            SpawnRandomPrefab();
            yield return new WaitForSeconds(Random.Range(delayÂetweenSpawns.x, delayÂetweenSpawns.y));
        }

        stopSpawnEvent?.Invoke();
        yield break;
    }


    public void StartSpawn()
    {
        startSpawnEvent?.Invoke();
        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(StartSpawnCor());
    }


    public void StopSpawn()
    {
        stopSpawnEvent?.Invoke();
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
    }

    public void SpawnRandomPrefab()
    {
        spawnPrefabEvent?.Invoke();
        Instantiate(spawnerData.Prefabs[Random.Range(0, spawnerData.Prefabs.Count)], spawnPosition).transform.localPosition = Vector3.zero;
    }

    // â ðàíäîìíîì ðàäèóñå 
    // ðàíäîìíîå êîë-âî çà ðàç
    // èâåíòû çàñïàâíèë íà÷àë ñïàâíèòü çàêîí÷èë ñïàâíèòü
    // âðåìÿ æèçíè 
    // áåññêîíå÷íàÿ ãåíåðàöèÿ
    // øàíñ âûïàäåíèÿ êàæäîãî èòåìà îòäåëüíî
}
