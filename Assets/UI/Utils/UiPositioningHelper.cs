


using UnityEngine;

public static class UiPositioningHelper
{
    public static Vector3 GetIconButtonSlotPosition(int index, GameObject iconButtonPrefab, int itemsPerRow)
    {
        float iconButtonWidth = iconButtonPrefab.GetComponent<RectTransform>().rect.width;
        float iconButtonHeight = -iconButtonPrefab.GetComponent<RectTransform>().rect.height;
        int row = (index) / itemsPerRow;
        int column = ((index) % itemsPerRow);
        return new Vector3((-iconButtonWidth / 2) + (iconButtonWidth * column), (iconButtonHeight / 2) + (iconButtonHeight * row), 0);
    }
}