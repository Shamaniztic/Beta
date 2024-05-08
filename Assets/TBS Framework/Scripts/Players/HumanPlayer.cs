using System.Collections;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Players
{
    /// <summary>
    /// Class representing a human player.
    /// </summary>
    public class HumanPlayer : Player
    {
        // VARIABLES
        [SerializeField] private Unit[] units;

        public int UnitsUsedThisTurn { get; private set; } = 0;

        public bool HasUnitsLeftInTurn => UnitsUsedThisTurn < units.Length;

        // METHODS
        public override void Play(CellGrid cellGrid)
        {
            cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid);
        }

        public void SetUnitsUsedValue(int index)
        {
            UnitsUsedThisTurn = index;
        }

        public void IncrementCurrentUnitIndex()
        {
            UnitsUsedThisTurn++;
        }
    }
}
