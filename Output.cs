using System.Drawing;

namespace Ghpp
{
    internal sealed class Output
    {
        internal string? Path { get; set; } = null;
        internal Color? BackgroundColor { get; set; } = null;
        internal Color? ForegroundColor { get; set; } = null;
        internal Map? Map { get; set; } = null;

        internal bool Save()
        {
            if (Path is null)
                return false;
            else
            {
                if (BackgroundColor is null)
                {
                    var rgb = new byte[3];
                    Randomizer.Generator.NextBytes(rgb);
                    BackgroundColor = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
                }

                if (ForegroundColor is null)
                {
                    var rgb = new byte[3];
                    Randomizer.Generator.NextBytes(rgb);
                    ForegroundColor = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
                }

                if (Map is null)
                    Map = new Map();

                var size = 420;
                using var image = new Bitmap(size, size);
                var contentStart = 35;
                var contentEnd = size - contentStart + 1;
                var blockSize = 71;
                var blockOffset = -1;

                #region Draw

                for (var x = 0; x < image.Width; x++)
                {
                    for (var y = 0; y < image.Height; y++)
                        image.SetPixel(x, y, (Color)BackgroundColor);
                }

                for (int blockX = contentStart, blockY = contentStart, i = 0; i < Map.Bits.Length; i++)
                {
                    int x, y;

                    for (x = blockX; x < blockX + blockSize; x++)
                    {
                        for (y = blockY; y < blockY + blockSize; y++)
                        {
                            if (Map.Bits[i])
                                image.SetPixel(x, y, (Color)ForegroundColor);
                        }
                    }

                    blockX += blockSize + blockOffset;

                    if (blockX == contentEnd - 1)
                    {
                        blockX = contentStart;
                        blockY += blockSize + blockOffset;
                    }
                }

                #endregion

                image.Save(Path);
                return true;
            }
        }
    }
}
