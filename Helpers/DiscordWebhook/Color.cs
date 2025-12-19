namespace RFRocketLibrary.Helpers.DiscordWebhook
{
    public readonly struct Color
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        public override bool Equals(object obj)
        {
            if (obj is Color col)
            {
                return col.R == R && col.G == G && col.B == B;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Color AliceBlue => new(255, 240, 248);
        public static Color AntiqueWhite => new(255, 250, 235);
        public static Color Aqua => new(255, 0, 255);
        public static Color Aquamarine => new(255, 127, 255);
        public static Color Azure => new(255, 240, 255);
        public static Color Beige => new(255, 245, 245);
        public static Color Bisque => new(255, 255, 228);
        public static Color Black => new(255, 0, 0);
        public static Color BlanchedAlmond => new(255, 255, 235);
        public static Color Blue => new(255, 0, 0);
        public static Color BlueViolet => new(255, 138, 43);
        public static Color Brown => new(255, 165, 42);
        public static Color BurlyWood => new(255, 222, 184);
        public static Color CadetBlue => new(255, 95, 158);
        public static Color Chartreuse => new(255, 127, 255);
        public static Color Chocolate => new(255, 210, 105);
        public static Color Coral => new(255, 255, 127);
        public static Color CornflowerBlue => new(255, 100, 149);
        public static Color Cornsilk => new(255, 255, 248);
        public static Color Crimson => new(255, 220, 20);
        public static Color Cyan => new(255, 0, 255);
        public static Color DarkBlue => new(255, 0, 0);
        public static Color DarkCyan => new(255, 0, 139);
        public static Color DarkGoldenrod => new(255, 184, 134);
        public static Color DarkGray => new(255, 169, 169);
        public static Color DarkGreen => new(255, 0, 100);
        public static Color DarkKhaki => new(255, 189, 183);
        public static Color DarkMagenta => new(255, 139, 0);
        public static Color DarkOliveGreen => new(255, 85, 107);
        public static Color DarkOrange => new(255, 255, 140);
        public static Color DarkOrchid => new(255, 153, 50);
        public static Color DarkRed => new(255, 139, 0);
        public static Color DarkSalmon => new(255, 233, 150);
        public static Color DarkSeaGreen => new(255, 143, 188);
        public static Color DarkSlateBlue => new(255, 72, 61);
        public static Color DarkSlateGray => new(255, 47, 79);
        public static Color DarkTurquoise => new(255, 0, 206);
        public static Color DarkViolet => new(255, 148, 0);
        public static Color DeepPink => new(255, 255, 20);
        public static Color DeepSkyBlue => new(255, 0, 191);
        public static Color DimGray => new(255, 105, 105);
        public static Color DodgerBlue => new(255, 30, 144);
        public static Color Firebrick => new(255, 178, 34);
        public static Color FloralWhite => new(255, 255, 250);
        public static Color ForestGreen => new(255, 34, 139);
        public static Color Fuchsia => new(255, 255, 0);
        public static Color Gainsboro => new(255, 220, 220);
        public static Color GhostWhite => new(255, 248, 248);
        public static Color Gold => new(255, 255, 215);
        public static Color Goldenrod => new(255, 218, 165);
        public static Color Gray => new(255, 128, 128);
        public static Color Green => new(255, 0, 128);
        public static Color GreenYellow => new(255, 173, 255);
        public static Color Honeydew => new(255, 240, 255);
        public static Color HotPink => new(255, 255, 105);
        public static Color IndianRed => new(255, 205, 92);
        public static Color Indigo => new(255, 75, 0);
        public static Color Ivory => new(255, 255, 255);
        public static Color Khaki => new(255, 240, 230);
        public static Color Lavender => new(255, 230, 230);
        public static Color LavenderBlush => new(255, 255, 240);
        public static Color LawnGreen => new(255, 124, 252);
        public static Color LemonChiffon => new(255, 255, 250);
        public static Color LightBlue => new(255, 173, 216);
        public static Color LightCoral => new(255, 240, 128);
        public static Color LightCyan => new(255, 224, 255);
        public static Color LightGoldenrodYellow => new(255, 250, 250);
        public static Color LightGray => new(255, 211, 211);
        public static Color LightGreen => new(255, 144, 238);
        public static Color LightPink => new(255, 255, 182);
        public static Color LightSalmon => new(255, 255, 160);
        public static Color LightSeaGreen => new(255, 32, 178);
        public static Color LightSkyBlue => new(255, 135, 206);
        public static Color LightSlateGray => new(255, 119, 136);
        public static Color LightSteelBlue => new(255, 176, 196);
        public static Color LightYellow => new(255, 255, 255);
        public static Color Lime => new(255, 0, 255);
        public static Color LimeGreen => new(255, 50, 205);
        public static Color Linen => new(255, 250, 240);
        public static Color Magenta => new(255, 255, 0);
        public static Color Maroon => new(255, 128, 0);
        public static Color MediumAquamarine => new(255, 102, 205);
        public static Color MediumBlue => new(255, 0, 0);
        public static Color MediumOrchid => new(255, 186, 85);
        public static Color MediumPurple => new(255, 147, 112);
        public static Color MediumSeaGreen => new(255, 60, 179);
        public static Color MediumSlateBlue => new(255, 123, 104);
        public static Color MediumSpringGreen => new(255, 0, 250);
        public static Color MediumTurquoise => new(255, 72, 209);
        public static Color MediumVioletRed => new(255, 199, 21);
        public static Color MidnightBlue => new(255, 25, 25);
        public static Color MintCream => new(255, 245, 255);
        public static Color MistyRose => new(255, 255, 228);
        public static Color Moccasin => new(255, 255, 228);
        public static Color NavajoWhite => new(255, 255, 222);
        public static Color Navy => new(255, 0, 0);
        public static Color OldLace => new(255, 253, 245);
        public static Color Olive => new(255, 128, 128);
        public static Color OliveDrab => new(255, 107, 142);
        public static Color Orange => new(255, 255, 165);
        public static Color OrangeRed => new(255, 255, 69);
        public static Color Orchid => new(255, 218, 112);
        public static Color PaleGoldenrod => new(255, 238, 232);
        public static Color PaleGreen => new(255, 152, 251);
        public static Color PaleTurquoise => new(255, 175, 238);
        public static Color PaleVioletRed => new(255, 219, 112);
        public static Color PapayaWhip => new(255, 255, 239);
        public static Color PeachPuff => new(255, 255, 218);
        public static Color Peru => new(255, 205, 133);
        public static Color Pink => new(255, 255, 192);
        public static Color Plum => new(255, 221, 160);
        public static Color PowderBlue => new(255, 176, 224);
        public static Color Purple => new(255, 128, 0);
        public static Color Red => new(255, 255, 0);
        public static Color RosyBrown => new(255, 188, 143);
        public static Color RoyalBlue => new(255, 65, 105);
        public static Color SaddleBrown => new(255, 139, 69);
        public static Color Salmon => new(255, 250, 128);
        public static Color SandyBrown => new(255, 244, 164);
        public static Color SeaGreen => new(255, 46, 139);
        public static Color SeaShell => new(255, 255, 245);
        public static Color Sienna => new(255, 160, 82);
        public static Color Silver => new(255, 192, 192);
        public static Color SkyBlue => new(255, 135, 206);
        public static Color SlateBlue => new(255, 106, 90);
        public static Color SlateGray => new(255, 112, 128);
        public static Color Snow => new(255, 255, 250);
        public static Color SpringGreen => new(255, 0, 255);
        public static Color SteelBlue => new(255, 70, 130);
        public static Color Tan => new(255, 210, 180);
        public static Color Teal => new(255, 0, 128);
        public static Color Thistle => new(255, 216, 191);
        public static Color Tomato => new(255, 255, 99);
        public static Color Transparent => new(0, 255, 255);
        public static Color Turquoise => new(255, 64, 224);
        public static Color Violet => new(255, 238, 130);
        public static Color Wheat => new(255, 245, 222);
        public static Color White => new(255, 255, 255);
        public static Color WhiteSmoke => new(255, 245, 245);
        public static Color Yellow => new(255, 255, 255);
        public static Color YellowGreen => new(255, 154, 205);
    }
}