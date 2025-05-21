using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeleeFighter meleeFighter;

    public void Awake()
    {
        meleeFighter = GetComponent<MeleeFighter>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            meleeFighter.TryToAttack();
        }
    }
}
