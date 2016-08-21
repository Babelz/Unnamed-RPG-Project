using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD
{
    public struct TextLine
    {
        public Vector2 Position;
        public Vector2 Size;
        public string Contents;
    }

    public static class TextHelper
    {
        public static IEnumerable<TextLine> GenerateLines(Vector2 size, SpriteFont font, string text, float columnOffset = 8.0f, float rowOffset = 0.0f)
        {
            var maxWidth    = size.X;
            var textSize    = font.MeasureString(text);
            var linesCount  = (int)Math.Round(textSize.X / maxWidth, 0);
            var lineWidth   = maxWidth / linesCount;
            var buffer      = new StringBuilder();
            var row         = 0.0f;
            
            for (int i = 0; i < text.Length; i++)
            {
                var ch      = text[i];
                var str     = buffer.ToString();
                var strSize = font.MeasureString(str);

                if (strSize.X + columnOffset >= maxWidth)
                {
                    if (char.IsLetter(ch)) { buffer.Append('-'); i--; }
                    else                   { i--; }

                    TextLine line;

                    line.Position = new Vector2(0.0f, row + rowOffset);
                    line.Contents = buffer.ToString();
                    line.Size     = font.MeasureString(line.Contents);

                    row += line.Size.Y;

                    buffer.Clear();

                    yield return line;

                    continue;
                }

                buffer.Append(ch);
            }

            TextLine lastLine;
            lastLine.Position = new Vector2(0.0f, row + rowOffset);
            lastLine.Contents = buffer.ToString();
            lastLine.Size     = font.MeasureString(lastLine.Contents);

            if (!string.IsNullOrEmpty(lastLine.Contents)) yield return lastLine;
        }
    }
}
