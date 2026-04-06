using System.Collections.Generic;
using UnityEngine;


public static class DirectionsHelper
{
    public enum Count : byte
    {
        Eight = 8,
        Twelve = 12,
        Sixteen = 16
    }

    public static void DrawGizmo(Vector3 position, float magnitude, Count count)
    {
        if (count == DirectionsHelper.Count.Eight)
        {
            foreach (Vector3 direction in DirectionsHelper.EightDirections)
            {
                Gizmos.DrawLine(position, position + direction * magnitude);
            }
        }
        else if (count == DirectionsHelper.Count.Twelve)
        {
            foreach (Vector3 direction in DirectionsHelper.TwelveDirections)
            {
                Gizmos.DrawLine(position, position + direction * magnitude);
            }
        }
        else if (count == DirectionsHelper.Count.Sixteen)
        {
            foreach (Vector3 direction in DirectionsHelper.SixteenDirections)
            {
                Gizmos.DrawLine(position, position + direction * magnitude);
            }
        }
    }

    public static List<Vector3> EightDirections = new List<Vector3>{
            new Vector3(Mathf.Cos(0.0f),0.0f, Mathf.Sin(0.0f)),
            new Vector3(Mathf.Cos(45.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(45.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(90.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(90.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(135.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(135.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(180.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(180.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(225.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(225.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(270.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(270.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(315.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(315.0f * Mathf.Deg2Rad))
        };

    public static List<Vector3> TwelveDirections = new List<Vector3>{
            new Vector3(Mathf.Cos(0.0f),0.0f, Mathf.Sin(0.0f)),
            new Vector3(Mathf.Cos(30.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(30.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(60.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(60.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(90.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(90.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(120.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(120.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(150.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(150.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(180.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(180.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(210.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(210.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(240.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(240.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(270.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(270.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(300.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(300.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(330.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(330.0f * Mathf.Deg2Rad))
        };

    public static List<Vector3> SixteenDirections = new List<Vector3>{
            new Vector3(Mathf.Cos(0.0f),0.0f, Mathf.Sin(0.0f)),
            new Vector3(Mathf.Cos(22.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(22.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(45.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(45.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(67.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(67.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(90.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(90.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(112.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(112.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(135.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(135.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(157.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(157.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(180.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(180.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(202.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(202.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(225.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(225.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(247.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(247.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(270.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(270.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(292.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(292.5f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(315.0f * Mathf.Deg2Rad),0.0f, Mathf.Sin(315.0f * Mathf.Deg2Rad)),
            new Vector3(Mathf.Cos(337.5f * Mathf.Deg2Rad),0.0f, Mathf.Sin(337.5f * Mathf.Deg2Rad))
        };
}
