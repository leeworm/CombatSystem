using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;
    private void Awake()
    {
        enemy.visionSensor = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
            EnemyManager.i.AddEnemyInRange(enemy);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeleeFighter>();

        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
            EnemyManager.i.RemoveEnemyInRange(enemy);
        }
    }
}