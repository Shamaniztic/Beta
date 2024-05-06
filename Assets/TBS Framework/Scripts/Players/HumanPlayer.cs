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

        private int currentUnitIndex = 0;

        public bool HasUnitsLeftInTurn => currentUnitIndex < units.Length;

        public Unit CurrentUnit => units[currentUnitIndex];

        // EXECUTION FUNCTIONS
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.2f);
            (CurrentUnit.Cell).MarkAsHighlighted();
        }

        // METHODS
        public override void Play(CellGrid cellGrid)
        {
            cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid);
        }

        public void IncrementCurrentUnitIndex()
        {
            currentUnitIndex++;

            
            (CurrentUnit.Cell).MarkAsHighlighted();
        }
    }
}
