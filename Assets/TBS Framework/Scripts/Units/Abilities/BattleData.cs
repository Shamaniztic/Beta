using System;
using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

public static class BattleData
{
    // VARIABLES
    public static int CurrentPlayerFighterID;
    public static int CurrentEnemyFighterID;
    public static int UsedPlayerUnits = -1;
    public static bool PlayerIsAttacker = false;
    public static Dictionary<int, UnitData> UnitDataDictionary = new Dictionary<int, UnitData>();
    public static bool? PlayerWon { get; set; }

    public static UnitData CurrentPlayerFighterData => UnitDataDictionary.ContainsKey(CurrentPlayerFighterID) ? UnitDataDictionary[CurrentPlayerFighterID] : null;
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
                                .WithAttack(unit.AttackFactor)
                                .WithDefence(unit.DefenceFactor)
                                .WithData(unit.Data)
                                .WithPosition(unit.transform.position)
                                .WithCell(unit.Cell.OffsetCoord)
                                .WithHasDoneTurn(unit.HasDoneTurn)
                                .Build();

            UnitDataDictionary.Add(unit.UnitID, newUnitData);
        }
        else
        {
            UnitDataDictionary[unit.UnitID].SetHitPoints(unit.HitPoints);
            UnitDataDictionary[unit.UnitID].SetPosition(unit.transform.position);
            UnitDataDictionary[unit.UnitID].SetCoords(unit.Cell.OffsetCoord);
            UnitDataDictionary[unit.UnitID].SetHasDoneTurn(unit.HasDoneTurn);
        }
    }



    // HELPER CLASSES
    public class UnitData
    {
        public int UnitID { get; private set; }
        public string UnitName { get; private set; }
        public int UnitPlayerNumber { get; private set; }
        public int UnitHealth { get; private set; }
        public int UnitAttack { get; private set; }
        public int UnitDefence { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector2 CellOffset { get; private set; }
        public UnitSO Data { get; private set; }
        public bool HasDoneTurn { get; private set; }

        private UnitData() { }


        public void LoseHealth(int amount)
        {
            UnitHealth -= amount;
        }

        public void SetHitPoints(int hitPoints)
        {
            UnitHealth = hitPoints;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetCoords(Vector2 offsetCoord)
        {
            CellOffset = offsetCoord;
        }

        public void SetHasDoneTurn(bool hasDoneTurn)
        {
            HasDoneTurn = hasDoneTurn;
        }

        public class Builder
        {
            readonly int id;
            string name;
            int playerNumber;
            int health;
            int attack;
            int defence;
            Vector3 position;
            Vector2 cellOffset;
            UnitSO data;
            bool hasDoneTurn;

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

            public Builder WithAttack(int attack)
            {
                this.attack = attack;
                return this;
            }

            public Builder WithDefence(int defence)
            {
                this.defence = defence;
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

            public Builder WithCell(Vector2 cellOffset)
            {
                this.cellOffset = cellOffset; 
                return this;
            }

            public Builder WithHasDoneTurn(bool hasDoneTurn)
            {
                this.hasDoneTurn = hasDoneTurn;
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
                    UnitAttack = attack,
                    UnitDefence = defence,
                    Data = data,
                    Position = position,
                    CellOffset = cellOffset,
                    HasDoneTurn = hasDoneTurn
                };

                return newData;
            }
        }
    }

}