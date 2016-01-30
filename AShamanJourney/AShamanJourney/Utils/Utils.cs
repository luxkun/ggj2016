using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Engine;
using Aiv.Fast2D;
using OpenTK;

namespace AShamanJourney
{
    public static class Utils
    {
        public static string RandomString(int length, string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[GameManager.Random.Next(s.Length)]).ToArray());
        }
        public static List<KeyCode> RandomKeys(int length, KeyCode[] keyCodes)
        {
            var result = new List<KeyCode>(length);
            for (int i = 0; i < length; i++)
            {
                result.Add(keyCodes[GameManager.Random.Next(keyCodes.Length)]);
            }
            return result;
        }

        public static List<string> GetAssetName(string baseName, int sX, int sY, int lenX = 1, int lenY = 1)
        {
            var result = new List<string>();
            for (var y = 0; y < lenY; y++)
            {
                for (var x = 0; x < lenX; x++)
                {
                    result.Add($"{baseName}_{y + sY}_{x + sX}");
                }
            }
            return result;
        }

        public static void LoadAnimation(Engine engine, string name, string fileName, int xLen, int yLen)
        {
            var spriteAsset = new SpriteAsset(fileName);
            var blockSizeOnWall = new Vector2(spriteAsset.Width / (float)xLen, spriteAsset.Height / (float)yLen);
            for (var posX = 0; posX < xLen; posX++)
                for (var posY = 0; posY < yLen; posY++)
                {
                    var animName = $"{name}_{posY}_{posX}";
                    Debug.WriteLine("Loaded animations: " + animName);
                    engine.LoadAsset(animName,
                        new SpriteAsset(fileName, (int)(posX * blockSizeOnWall.X), (int)(posY * blockSizeOnWall.Y),
                            (int)blockSizeOnWall.X, (int)blockSizeOnWall.Y));
                }
        }
    }
}
