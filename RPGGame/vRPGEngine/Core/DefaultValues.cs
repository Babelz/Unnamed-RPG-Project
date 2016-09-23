﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Core
{
    public static class DefaultValues
    {
        #region Fields
        public static readonly Texture2D  EmptyTexture;
        public static readonly Texture2D  MissingTexture;
        public static readonly SpriteFont DefaultFont;
        #endregion

        static DefaultValues()
        {
            // Empty texture init.
            const int width  = 32;
            const int height = 32;

            EmptyTexture = new Texture2D(Engine.Instance.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            var pixels   = Enumerable.Range(0, width * height).Select(i => Color.White).ToArray();
            EmptyTexture.SetData(pixels);

            // Missing texture init.
            #region Missing texture bytes
            var MissingTextureData = new byte[]
            {
	            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 
	            13, 73, 72, 68, 82, 0, 0, 0, 128, 0, 0, 
	            0, 128, 8, 2, 0, 0, 0, 76, 92, 246, 156, 
	            0, 0, 0, 7, 116, 73, 77, 69, 7, 224, 8, 
	            3, 11, 59, 43, 100, 14, 172, 148, 0, 0, 0, 
	            23, 116, 69, 88, 116, 83, 111, 102, 116, 119, 97, 
	            114, 101, 0, 71, 76, 68, 80, 78, 71, 32, 118, 
	            101, 114, 32, 51, 46, 52, 113, 133, 164, 225, 0, 
	            0, 0, 8, 116, 112, 78, 71, 71, 76, 68, 51, 
	            0, 0, 0, 0, 74, 128, 41, 31, 0, 0, 0, 
	            4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 
	            97, 5, 0, 0, 0, 6, 98, 75, 71, 68, 0, 
	            255, 0, 255, 0, 255, 160, 189, 167, 147, 0, 0, 
	            2, 59, 73, 68, 65, 84, 120, 156, 237, 220, 75, 
	            110, 195, 32, 20, 70, 97, 54, 216, 253, 175, 32, 
	            203, 168, 212, 89, 21, 133, 55, 6, 140, 115, 142, 
	            245, 15, 220, 216, 224, 203, 253, 242, 232, 136, 240, 
	            250, 249, 53, 55, 38, 220, 94, 1, 60, 2, 8, 
	            192, 142, 0, 2, 176, 35, 128, 0, 236, 8, 32, 
	            0, 59, 2, 8, 192, 142, 0, 2, 176, 35, 128, 
	            0, 236, 8, 32, 0, 59, 2, 8, 192, 142, 0, 
	            2, 176, 35, 128, 0, 236, 8, 32, 0, 59, 2, 
	            124, 35, 64, 72, 29, 141, 87, 227, 27, 218, 7, 
	            246, 86, 120, 123, 247, 215, 2, 228, 94, 233, 186, 
	            90, 30, 56, 177, 66, 1, 66, 174, 47, 43, 0, 
	            206, 201, 248, 122, 186, 250, 120, 5, 160, 240, 196, 
	            143, 75, 201, 239, 165, 228, 165, 248, 188, 58, 86, 
	            128, 196, 111, 64, 117, 254, 21, 231, 167, 0, 84, 
	            203, 45, 55, 43, 121, 53, 55, 73, 181, 11, 235, 
	            154, 158, 92, 194, 51, 0, 90, 238, 44, 76, 56, 
	            60, 112, 197, 249, 186, 92, 125, 64, 242, 13, 34, 
	            192, 62, 128, 106, 215, 122, 23, 86, 176, 188, 17, 
	            96, 29, 198, 38, 128, 247, 23, 171, 139, 9, 111, 
	            199, 43, 250, 55, 244, 227, 168, 54, 171, 235, 254, 
	            194, 216, 21, 221, 159, 3, 240, 148, 44, 237, 163, 
	            0, 245, 166, 11, 112, 167, 193, 153, 221, 71, 0, 
	            28, 30, 1, 4, 96, 71, 0, 1, 216, 17, 64, 
	            0, 118, 4, 16, 128, 29, 1, 4, 96, 71, 0, 
	            1, 216, 17, 64, 0, 118, 4, 16, 128, 29, 1, 
	            4, 96, 71, 0, 1, 216, 17, 64, 0, 118, 4, 
	            16, 128, 29, 1, 4, 96, 71, 0, 1, 216, 17, 
	            64, 0, 118, 4, 16, 128, 29, 1, 4, 96, 71, 
	            0, 1, 216, 17, 64, 0, 118, 4, 16, 128, 29, 
	            1, 4, 96, 71, 0, 1, 216, 17, 64, 0, 118, 
	            4, 16, 128, 29, 1, 4, 96, 71, 0, 1, 216, 
	            17, 64, 0, 118, 4, 56, 27, 32, 183, 223, 233, 
	            172, 29, 105, 167, 236, 167, 154, 220, 83, 250, 204, 
	            109, 90, 191, 19, 96, 226, 108, 251, 229, 154, 0, 
	            66, 106, 115, 244, 3, 223, 98, 80, 128, 143, 27, 
	            114, 95, 2, 201, 215, 227, 121, 170, 99, 11, 13, 
	            74, 94, 141, 7, 86, 231, 15, 209, 54, 251, 185, 
	            58, 203, 75, 158, 9, 80, 125, 252, 254, 243, 22, 
	            128, 117, 53, 12, 148, 183, 27, 160, 220, 154, 157, 
	            0, 113, 205, 201, 133, 140, 213, 80, 120, 196, 52, 
	            128, 184, 220, 198, 130, 202, 150, 219, 0, 198, 238, 
	            239, 2, 232, 106, 250, 90, 128, 106, 101, 87, 154, 
	            62, 17, 96, 202, 27, 98, 31, 192, 129, 231, 189, 
	            0, 139, 106, 104, 47, 111, 7, 192, 255, 159, 185, 
	            94, 52, 86, 95, 152, 179, 11, 32, 249, 220, 94, 
	            167, 198, 225, 3, 221, 175, 3, 156, 150, 177, 69, 
	            158, 156, 7, 44, 230, 226, 103, 252, 240, 60, 99, 
	            49, 87, 62, 227, 135, 231, 219, 214, 243, 184, 8, 
	            32, 0, 59, 2, 8, 192, 142, 0, 2, 176, 35, 
	            128, 0, 236, 8, 32, 0, 59, 2, 8, 192, 142, 
	            0, 2, 176, 35, 128, 0, 236, 8, 32, 0, 59, 
	            127, 196, 137, 185, 114, 42, 177, 204, 33, 0, 0, 
	            0, 0, 73, 69, 78, 68, 174, 66, 96, 130, 
            };
            #endregion

            using (var ms = new MemoryStream(MissingTextureData))
            {
                MissingTexture = Texture2D.FromStream(Engine.Instance.GraphicsDevice, ms);
            }

            // Default font init.
            DefaultFont = Engine.Instance.Content.Load<SpriteFont>("default font");
        }
    }
}