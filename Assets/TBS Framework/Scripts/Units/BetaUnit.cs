using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Units
{
    public class BetaUnit : Unit
    {
        public string UnitName;
        private Transform Highlighter;

        public override void Initialize()
        {
            base.Initialize();
            Highlighter = transform.Find("Highlighter");
        }

        protected override void DefenceActionPerformed()
        {
            // No healthbar update needed
        }

        private void SetHighlighterColor(Color color)
        {
            Highlighter.GetComponent<Renderer>().material.color = color;
        }

        public override void SetColor(Color color)
        {
            SetHighlighterColor(color);
        }
    }
}