using System;
using UnityEngine;

namespace RFRocketLibrary.Helpers
{
    // Original from: https://github.com/RocketMod/Rocket.Unturned/blob/legacy/Rocket.Unturned/Chat/UnturnedChat.cs
    // Original Author: fr34kyn01535
    public static class ColorHelper
    {
        #region Methods

        public static Color? GetColorFromHex(string hexString)
        {
            hexString = hexString.Replace("#", "");
            if (hexString.Length == 3)
            {
                // #99f
                hexString = hexString.Insert(1, Convert.ToString(hexString[0])); // #999f
                hexString = hexString.Insert(3, Convert.ToString(hexString[2])); // #9999f
                hexString = hexString.Insert(5, Convert.ToString(hexString[4])); // #9999ff
            }

            if (hexString.Length != 6 ||
                !int.TryParse(hexString, System.Globalization.NumberStyles.HexNumber, null, out var argb))
            {
                return null;
            }

            var r = (byte) ((argb >> 16) & 0xff);
            var g = (byte) ((argb >> 8) & 0xff);
            var b = (byte) (argb & 0xff);
            return GetColorFromRGB(r, g, b);
        }

        public static Color GetColorFromName(string colorName, Color fallback)
        {
            if (string.IsNullOrWhiteSpace(colorName))
                return fallback;
            
            switch (colorName.Trim().ToLower())
            {
                case "black": return Color.black;
                case "blue": return Color.blue;
                case "clear": return Color.clear;
                case "cyan": return Color.cyan;
                case "gray": return Color.gray;
                case "green": return Color.green;
                case "grey": return Color.grey;
                case "magenta": return Color.magenta;
                case "red": return Color.red;
                case "white": return Color.white;
                case "yellow": return Color.yellow;
                case "rocket": return GetColorFromRGB(90, 206, 205);
            }

            var color = GetColorFromHex(colorName);
            return color ?? fallback;
        }

        public static Color GetColorFromRGB(byte r, byte g, byte b, short a = 100)
        {
            return new Color((1f / 255f) * r, (1f / 255f) * g, (1f / 255f) * b, (1f / 100f) * a);
        }

        #endregion
    }
}