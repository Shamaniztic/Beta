using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public UnitData unitData;

    // If the unit has moved this turn.
    private bool hasMoved;
    // If the unit has acted this turn (e.g., attacked, used an item).
    private bool hasActed;

    private void Start()
    {
        // Initialize data here.
        hasMoved = false;
        hasActed = false;
    }

    public bool HasActed()
    {
        return hasActed;
    }


    public void Move(Vector2 targetPosition)
    {
        // Check if the unit has already moved this turn.
        if (hasMoved)
        {
            Debug.LogError("This unit has already moved this turn.");
            return;
        }

        // Here, you would need to implement pathfinding to check if the target position is within the unit's movement range,
        // and to find the shortest path to the target position. The specifics of this will depend on your game's grid system.
        // For example, you could use A* pathfinding.

        // If the target position is within range and reachable, move the unit and set hasMoved to true.
        // Otherwise, show an error message or provide some other form of feedback.
        // Note that you will probably want to animate the movement in some way, rather than just instantly changing the position.

        transform.position = targetPosition;
        hasMoved = true;
    }

    public void Attack(UnitManager targetUnit)
    {
        // Check if the unit has already acted this turn.
        if (hasActed)
        {
            Debug.LogError("This unit has already acted this turn.");
            return;
        }

        // Check if the target unit is within attack range.
        // You will need to replace this with your own method of measuring distance, depending on your grid system.
        if (Vector2.Distance(transform.position, targetUnit.transform.position) > unitData.attackRange)
        {
            Debug.LogError("The target unit is out of range.");
            return;
        }

        // Calculate damage and apply it to the target unit.
        // This is a very simple damage calculation and you will likely want to replace it with your own.
        int damage = unitData.baseStats.strength - targetUnit.unitData.baseStats.defense;
        targetUnit.unitData.baseStats.health -= damage;

        // Check if the target unit has been defeated.
        if (targetUnit.unitData.baseStats.health <= 0)
        {
            // Here, you would need to implement what happens when a unit is defeated.
            // For example, you could remove it from the battlefield, play a death animation, etc.
            Destroy(targetUnit.gameObject);
        }

        hasActed = true;
    }


    public void UseItem(Item item)
    {
        // Check if the unit has already acted this turn.
        if (hasActed)
        {
            Debug.LogError("This unit has already acted this turn.");
            return;
        }

        // Use the item.
        // You will need to implement the effects of the item.
        // For example, a healing item might increase the unit's health.

        hasActed = true;
    }

    public void EndTurn()
    {
        // Reset the unit's actions for this turn.
        hasMoved = false;
        hasActed = false;
    }
}
