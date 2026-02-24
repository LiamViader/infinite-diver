using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class EnemiesManager : MonoBehaviour
{
    private float margin = 2f;
    private bool _canSpawn=false;

    public GameObject whiteBird;
    public GameObject blackBird;

    private float startingSecondsBetweenSpawn = 2.5f;
    private float currentSecondsBetweenSpawn = 2.5f;

    private Coroutine spawnCoroutine;

    void Start()
    {
        GameManager.Instance.OnGameStart += HandleGameStarted;
        GameManager.Instance.OnGameOver += HandleGameOver;
    }

    void FixedUpdate()
    {
        if (_canSpawn)
        {
            int numberOfSpawns = Random.Range(1, 5);
            for (int i = 0; i < numberOfSpawns; i++)
            {
                Vector3 bottomEdgeWorld = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.height, 0, Camera.main.nearClipPlane));
                float ySpawn = bottomEdgeWorld.y - margin;
                Vector3 rightEdgeWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane));
                float xSpawn = Random.Range(-rightEdgeWorld.x, rightEdgeWorld.x);
                Vector3 spawnPosition = new Vector3(xSpawn, ySpawn, 0);
                Debug.Log(spawnPosition);
                float angle = Random.Range(-15f, 15f);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                float speed = Random.Range(1f, 2f);

                float randomNumber = Random.Range(0f, 100f);

                if (randomNumber <= 90f) {
                    FlyingBird spawnedBird=Instantiate(blackBird).GetComponent<FlyingBird>();
                    spawnedBird.Initialize(new BlackHitter(), new FlyingStraight(rotation * Vector3.up * speed), spawnPosition);
                }
                else
                {
                    FlyingBird spawnedBird = Instantiate(whiteBird).GetComponent<FlyingBird>();
                    spawnedBird.Initialize(new WhiteHitter(), new FlyingStraight(rotation * Vector3.up * speed), spawnPosition);
                }
            }

            _canSpawn = false;
            float secondsToSpawn= Random.Range(currentSecondsBetweenSpawn/5, currentSecondsBetweenSpawn);
            spawnCoroutine=StartCoroutine(EnableSpawnAfterDelay(secondsToSpawn));

        }
    }

    private void HandleGameStarted()
    {
        StartCoroutine(EnableSpawnAfterDelay(5f));
        currentSecondsBetweenSpawn = startingSecondsBetweenSpawn;
    }

    private IEnumerator EnableSpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _canSpawn = true;
    }

    private void HandleGameOver()
    {
        _canSpawn = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private void HandleDifficultyIncreased()
    {
        currentSecondsBetweenSpawn -= 0.1f;
        currentSecondsBetweenSpawn = Mathf.Max(currentSecondsBetweenSpawn, 0.05f);
    }
}
