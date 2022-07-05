using UnityEngine;

public class AnimationEventProcessor : MonoBehaviour
{
    public Enemy CurrentEnemy;

    public void OnEnemyDeath(){
        CurrentEnemy.Die();
                EnemyManager.Instance.Respawn(CurrentEnemy);
    }
}
