namespace UnityEngine.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using System.Collections.Generic;

    public class ButtonExtended : Button
    {

        /// <summary>
        /// Список объектов которые будут менять цвет при изменении состояния
        /// </summary>
        public List<Graphic> colorizeTargets = new List<Graphic>();

        /// <summary>
        /// Список объектов которые будут менять альфу при изменении состояния
        /// </summary>
        public List<Graphic> alphaOnlyTargets = new List<Graphic>();

        void Update()
        {
            if (interactable)
            {
                BringTargetsAlphaToValueVia(currentSelectionState);
                BringTargetsColorToValueVia(currentSelectionState);
            }
            else
            {
                BringTargetsAlphaToValueVia(SelectionState.Disabled);
                BringTargetsColorToValueVia(SelectionState.Disabled);
            }
        }

        /// <summary>
        /// Изменить альфу у объектов
        /// </summary>
        /// <param name="state"></param>
        void BringTargetsAlphaToValueVia(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    foreach (Graphic target in alphaOnlyTargets)
                        target.color = new Color(target.color.r, target.color.g, target.color.b, colors.normalColor.a);
                    break;
                case SelectionState.Pressed:
                    foreach (Graphic target in alphaOnlyTargets)
                        target.color = new Color(target.color.r, target.color.g, target.color.b, colors.pressedColor.a);
                    break;
                case SelectionState.Highlighted:
                    foreach (Graphic target in alphaOnlyTargets)
                        target.color = new Color(target.color.r, target.color.g, target.color.b, colors.highlightedColor.a);
                    break;
                case SelectionState.Disabled:
                    foreach (Graphic target in alphaOnlyTargets)
                        target.color = new Color(target.color.r, target.color.g, target.color.b, colors.disabledColor.a);
                    break;
            }
        }

        /// <summary>
        /// Изменить цвет у объектов
        /// </summary>
        /// <param name="state"></param>
        void BringTargetsColorToValueVia(SelectionState state)
        {
            switch (state)
            {
                case SelectionState.Normal:
                    foreach (Graphic target in colorizeTargets)
                        target.color = colors.normalColor;
                    break;
                case SelectionState.Pressed:
                    foreach (Graphic target in colorizeTargets)
                        target.color = colors.pressedColor;
                    break;
                case SelectionState.Highlighted:
                    foreach (Graphic target in colorizeTargets)
                        target.color = colors.highlightedColor;
                    break;
                case SelectionState.Disabled:
                    foreach (Graphic target in colorizeTargets)
                        target.color = colors.disabledColor;
                    break;
            }
        }
    }

}