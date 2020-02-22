using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public static int totalWidth = 350;
    public static int surfaceHeight = 125;
    public static int horizontalBorderLength = 140;
    public static int thickness = 40;
    public static float pixelsToUnitsFactor = 1f/350f;

    public static float yFactor = 1 + (70f/175f);
    public static float xOffset = (totalWidth/2f) * yFactor * pixelsToUnitsFactor;
    public static float yOffset = (surfaceHeight / 2f) * pixelsToUnitsFactor;
}
