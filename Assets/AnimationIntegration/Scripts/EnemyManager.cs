using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    [SerializeField]
    private float _respawnTime = 5f;
    [SerializeField]
    private Rect _respawnBounds;

    private void Awake()
    {
        Instance = this;
    }

    public void Respawn(Enemy e)
    {
        StartCoroutine(QueueRespawn(e));
    }

    private IEnumerator QueueRespawn(Enemy e)
    {
        yield return new WaitForSeconds(_respawnTime);
        var newX = Random.Range(_respawnBounds.xMin, _respawnBounds.xMax);
        var newZ = Random.Range(_respawnBounds.yMin, _respawnBounds.yMax);
        e.transform.position = new Vector3(newX, 0, newZ);
        e.Respawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var p1 = new Vector3(_respawnBounds.xMin, 0, _respawnBounds.yMin);
        var p2 = new Vector3(_respawnBounds.xMax, 0, _respawnBounds.yMin);
        Gizmos.DrawLine(p1, p2);
        p1.z = _respawnBounds.yMax;
        p2.z = _respawnBounds.yMax;
        Gizmos.DrawLine(p1, p2);
        p1.x = _respawnBounds.xMax;
        p1.z = _respawnBounds.yMin;
        Gizmos.DrawLine(p1, p2);
        p1.x = _respawnBounds.xMin;
        p2.x = _respawnBounds.xMin;
        Gizmos.DrawLine(p1, p2);
    }
}
