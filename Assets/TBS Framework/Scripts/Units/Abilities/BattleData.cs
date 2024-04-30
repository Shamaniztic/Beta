using TbsFramework.Units;
using UnityEngine;

public static class BattleData
{
    public static int PlayerUnitID { get; set; }
    public static int EnemyUnitID { get; set; }
    public static string PlayerUnitName { get; set; }
    public static string EnemyUnitName { get; set; }
    public static int PlayerUnitPlayerNumber { get; set; }
    public static int EnemyUnitPlayerNumber { get; set; }
    public static bool? PlayerWon { get; set; }
    public static int PlayerUnitHealth { get; set; }
    public static int EnemyUnitHealth { get; set; }
    public static Unit PlayerUnit { get; set; }
    public static Unit EnemyUnit { get; set; }

}