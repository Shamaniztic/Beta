using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

public static class BattleData
{
    // VARIABLES
    public static int CurrentPlayerFighterID;
    public static int CurrentEnemyFighterID;
    public static Dictionary<int, UnitData> UnitDataDictionary = new Dictionary<int, UnitData>();
    public static bool? PlayerWon { get; set; }

    public static UnitData CurrentPlayerFighterData => UnitDataDictionary[CurrentPlayerFighterID];
    public static UnitData CurrentEnemyFighterData => UnitDataDictionary[CurrentEnemyFighterID];

    // METHODS
    public static void AddUnitDataToDictionary(Unit unit)
    {
        if (!UnitDataDictionary.ContainsKey(unit.UnitID))
        {
            var newUnitData = new UnitData.Builder(unit.UnitID)
                                .WithName(unit.UName)
                                .WithPlayerNumber(unit.PlayerNumber)
                                .WithHealth(unit.HitPoints)
                                .WithData(unit.Data)
                                .WithPosition(unit.transform.position)
                                .WithCell(unit.Cell)
                                .Build();

            UnitDataDictionary.Add(unit.UnitID, newUnitData);
        }
    }



    // HELPER CLASSES
    public class UnitData
    {
        public int UnitID { get; private set; }
        public string UnitName { get; private set; }
        public int UnitPlayerNumber { get; private set; }
        public int UnitHealth { get; private set; }
        public Vector3 Position { get; private set; }
        public Cell Cell { get; private set; }
        public UnitSO Data { get; private set; }

        private UnitData() { }


        public void LoseHealth(int amount)
        {
            UnitHealth -= amount;
        }

        public class Builder
        {
            readonly int id;
            string name;
            int playerNumber;
            int health;
            Vector3 position;
            Cell cell;
            UnitSO data;

            public Builder(int id)
            {
                this.id = id;
            }

            public Builder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public Builder WithPlayerNumber(int playerNumber)
            {
                this.playerNumber = playerNumber;
                return this;
            }

            public Builder WithHealth(int health)
            {
                this.health = health;
                return this;
            }

            public Builder WithData(UnitSO data)
            {
                this.data = data;
                return this;
            }

            public Builder WithPosition(Vector3 position)
            {
                this.position = position;
                return this;
            }

            public Builder WithCell(Cell cell)
            {
                this.cell = cell; 
                return this;
            }

            public UnitData Build()
            {
                UnitData newData = new()
                {
                    UnitID = id,
                    UnitName = name,
                    UnitPlayerNumber = playerNumber,
                    UnitHealth = health,
                    Data = data,
                    Position = position,
                    Cell = cell
                };

                return newData;
            }
        }
    }

}