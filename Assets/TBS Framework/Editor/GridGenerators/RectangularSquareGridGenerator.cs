using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using TbsFramework.Cells;

namespace TbsFramework.EditorUtils.GridGenerators
{
    [ExecuteInEditMode()]
    public class RectangularSquareGridGenerator : ICellGridGenerator
    {
        public string prefabFolderPath = "Assets/Prefabs/Tiles"; // Path to the prefabs
        public int Width;
        public int Height;

        public override GridInfo GenerateGrid()
        {
            GameObject[] prefabs = LoadPrefabsFromFolder(prefabFolderPath);
            var cells = new List<Cell>();

            if (prefabs.Length == 0)
            {
                Debug.LogError("No prefabs found in the specified folder.");
                return null;
            }

            int prefabIndex = 0;
            // Loop for visual top-left to bottom-right generation
            for (int j = Height - 1; j >= 0; j--) // Visual loop from top to bottom
            {
                for (int i = 0; i < Width; i++)
                {
                    if (prefabIndex >= prefabs.Length) prefabIndex = 0; // Loop back or break, depending on your needs
                    GameObject squarePrefab = prefabs[prefabIndex++];
                    var square = PrefabUtility.InstantiatePrefab(squarePrefab) as GameObject;

                    // Position cells starting from top-left for visual consistency
                    var position = Is2D ? new Vector3(i * square.GetComponent<Cell>().GetCellDimensions().x, j * square.GetComponent<Cell>().GetCellDimensions().y, 0) :
                        new Vector3(i * square.GetComponent<Cell>().GetCellDimensions().x, 0, j * square.GetComponent<Cell>().GetCellDimensions().z);

                    square.transform.position = position;
                    square.transform.parent = CellsParent;

                    var cellComponent = square.GetComponent<Cell>();
                    if (cellComponent != null)
                    {
                        // Assign logical offsets starting from bottom-left
                        cellComponent.OffsetCoord = new Vector2(i, Height - j - 1);
                    }
                    cells.Add(cellComponent);
                }
            }

            return GetGridInfo(cells);
        }

        GameObject[] LoadPrefabsFromFolder(string path)
        {
            List<GameObject> prefabs = new List<GameObject>();
            string[] fileEntries = Directory.GetFiles(path, "*.prefab");

            // Natural sort the file entries
            Array.Sort(fileEntries, new AlphanumComparatorFast());

            foreach (string fileName in fileEntries)
            {
                string assetPath = fileName.Replace(Application.dataPath, "Assets");
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null)
                {
                    prefabs.Add(prefab);
                }
            }
            return prefabs.ToArray();
        }
    }

    public class AlphanumComparatorFast : IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = x as string;
            string s2 = y as string;
            if (s1 == null || s2 == null)
                return 0;

            string pattern = @"\d+";
            var num1 = Regex.Match(s1, pattern).Value;
            var num2 = Regex.Match(s2, pattern).Value;

            int num1Int, num2Int;
            bool isNumeric1 = int.TryParse(num1, out num1Int);
            bool isNumeric2 = int.TryParse(num2, out num2Int);

            if (isNumeric1 && isNumeric2)
                return num1Int.CompareTo(num2Int);

            return s1.CompareTo(s2); // If no numbers or equal, compare as usual strings
        }
    }
}
