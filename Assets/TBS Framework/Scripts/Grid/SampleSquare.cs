﻿using TbsFramework.Cells;
using UnityEngine;

namespace TbsFramework.Tutorial
{
    public class SampleSquare : Square
    {
        public GameObject highlightedPrefab;
        public GameObject pathPrefab;
        public GameObject reachablePrefab;
        public GameObject attackRangePrefab;
        public GameObject attackReachablePrefab;

        private GameObject currentMarkingPrefab;

        public override Vector3 GetCellDimensions()
        {
            return GetComponent<Renderer>().bounds.size;
        }

        public override void MarkAsHighlighted()
        {
            ShowMarkingPrefab(highlightedPrefab);
        }

        public override void MarkAsPath()
        {
            ShowMarkingPrefab(pathPrefab);
        }

        public override void MarkAsReachable()
        {
            ShowMarkingPrefab(reachablePrefab);
        }

        public void MarkAsAttackReachable()
        {
            ShowMarkingPrefab(attackReachablePrefab);
        }

        public void MarkAsAttackRange()
        {
            ShowMarkingPrefab(attackRangePrefab);
        }

        public override void UnMark()
        {
            if (currentMarkingPrefab != null)
            {
                //Debug.Log(currentMarkingPrefab.name);
                if (currentMarkingPrefab.name == "Attack(Clone)")
                {
                    // Debug.Log("Unmarking");
                }

                Destroy(currentMarkingPrefab);
                currentMarkingPrefab = null;

            }
        }

        private void ShowMarkingPrefab(GameObject prefab)
        {
            UnMark();
            currentMarkingPrefab = Instantiate(prefab, transform);

            if (prefab.name == "Attack")
            {
                // Debug.Log("Show: " + prefab.name);
            }
        }
    }
}