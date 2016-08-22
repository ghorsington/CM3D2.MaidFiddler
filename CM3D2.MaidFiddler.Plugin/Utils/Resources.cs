using System;
using System.Drawing;
using System.IO;

namespace CM3D2.MaidFiddler.Plugin.Utils
{
    public static class Resources
    {
        public static readonly string ThumbnailBase64 =
            "iVBORw0KGgoAAAANSUhEUgAAAFoAAABaCAYAAAA4qEECAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAA"
            + "OwQAADsEBuJFr7QAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0IDQuMC45bDN+TgAAB8JJREFUeF7tnM"
            + "uLHFUUhzPOmIcyiaA7N0oMEk1GEERQo0t3ghuzEHyNio/EjbjLQgQDQU2M+ScENyKKGwmK+seIiooG8"
            + "ZXxfGWd4fTNrztdPffWrS5z4aNqTld1n/OrU/dVt2bXPScvXKMHpPEa+ZHGa+RHGgfOmrANHmkcKAh8"
            + "u7FpvGs81NrUsYNDGgcAAj5sPGsg6kHjH2Mr8HtrQ/AzBheArfq+6khjZQ4YfxhRVAR1Wyo4f0cbF4D"
            + "MV99dDWnsGbLXqwEy9yvjVyOK+ZfhgrqobP82Lrf4sdi+MY4ZXt1Ur2KksWfIYBeObERUshdc1Hlxwf"
            + "804rnVM1wae4RMi1VCxEValPgd3CH7DeVDL0hjj9B4RXFKQHZzp9BYKh96QRoLc5NBPeyZ6/WvEinWv"
            + "YvCd8Q6nt/uvSqRxsI8ZbgIZFoUZZrgOYjfTeOrfCuGNBbmS4NgyTIlQkn4HX6XOlv5VgxpLEhs/Grh"
            + "PZJeu3zSWIhHjGmZ21dGO951ZPSpfM2ONBbCq4xU1FiF9IX7QMOofM2ONBaCrpyLSqBQsxrht98xlK/"
            + "ZkcZCvG3EQC+121pi0+M5ayhfsyONBaDhOW94kGQ2AhMscxNRgD75xVD+ZkcaM4PI3hDG+hmRETvHoG"
            + "RR8KeXqVVpLACNTjo4gZrZ7Be9lwZRGgvAkJfbNA22ZjYDF5+pWeVzVqSxEJ8YKthakNGnDeVrdqSxA"
            + "NTT3xs1+syz+NHoZYQojQXg2Z83firgGnjjzCSX8jkr0lgAhE4fT9UGkenL45vyOSvSWIifDBVwTT4z"
            + "RlV1EIzqddTmN0P5mx1pzAyPkAhqaA2hQxXyhaF8z4Y0ZuacQUBDFNp9YnpA+Z4NaczMHUYa4JAgo4s"
            + "/2pLGzPCYn2BUkEOAnkfxpQjSmBkawiH2OBwGUsV7HtKYGYJQE0pDgd7QKIRmydeQqw6SAB+V79mQxs"
            + "wQxJAzGt+KL6iRxszQog9t+B1B6OJTpdKYmaELTV96FEITxJBm7SK0Hfg2GqE9qDTQWrgvbKH4Qhppz"
            + "AxBENQQG0QXfBQZzTJdghmK0IjrVZnPdYxKaGbIYrC+3zfxt3njiy0+Kt+zIY2ZYdRFMFQhHmTNxtF/"
            + "G1+8WhvFyBAIhkdGLMHyINnWwH8bX5gr/9ZQPmdFGgtAMM8ZtxlpwH3D73o2U3WMSujnDR4AxHmPGkL"
            + "7gh2qD7/Dik/6gzQWgMl/H+p+ZxBsLaH5XYT+0GA5WC/vs0hjIfx2ZW7aReY1h1SM0vhvcsEvGr28fy"
            + "iNheA2ZY00me0PAmoI7QsrmX/ppdoAaSwEfdX4pMUzu9ZCx94Wz4A0FsKftFA/xn50jbraGWVG0/Mgi"
            + "33YS7cq1td984PBhS8+KgRpXACcndWokM2s+v/Y8HdXaIhqz39QT6fVB746NN5Zsl4aO+LToN5VwnGc"
            + "jMe442Tv5wbO0+XzOrtGPY0vXGgGUe4fkDT0+YnH77YdTzpJY0d8vsCrBJzjnUKcpafBlhEYw12OIQA"
            + "mmBi88PoZ59QSOrYV0e4CO8SoYp8baeyIr61z51zweahddUwjjWVQQrtTs4TuchFqkgq9426gNHaki9"
            + "DpLTlU3E/fEqOKfW6ksSOp0NHBlGUR2vGYBiF02hjOYhmEjj56TDueeJLGjrDKJzoFqjVfFqLQvr/jQ"
            + "Y00doS+J86MQWi6mR4HIjs7nuGTxgWgb+zORceXjei/7zMmUDF3QhoXwEeHytFlBf8hy1IEaVyQIb+r"
            + "0hWPIUs2gzQuiL9CUfM/FuTCY2AeRMXaGWlcEBpFXlNInV5WeE+cuRoVa2ekcUG8nh5DRsdJrizz1dK"
            + "4AGTzMvedZ0FcKuZOSOMCMOminJybjRMfSPtAYJpXxT030tgRsnnZu3LzMHUYnhZ5jDSGkv5NCcciMg"
            + "2GcmxskEzEKzVJix837/FTS/sFPs/xf2GzDV+Wra2tdi+/0BcM5dCY2d9KMFEQ+WpC7w4HXW/sabcrx"
            + "nUtsfA5J/PMb+voK+caBzZOnE8dWnqmxPSahb+tmcKLHTsh9Fo4AHFXkpNSobkInDxRNw+817AQU2L6"
            + "1MLf1kzhxY6dELq5Om1phG7xEvcpNAjAf2/ZdmCMQsPGq1dk9SnT4MZU3IgXO3ZC6FhSUVVpjrGTnzA"
            + "uITC32GiFnoyLyaYDFv5eJbATix0/VehpYk/Yd++/ee3wM2993TowaqFDVl9sw9+nBHZksZPbvaak9f"
            + "HMctfm6fejQ74/Jhqh/4uN9dRkM2VVCRy5otjJ7d5kP3CecvDxk4+NVWDH4zv60ns/23a30fTIUmEVF"
            + "N/uSGg798jRl8/WWt9cnJDN3o19wFhf27fOnT+X2E5adczTGHLOisEixdfTvuaYMtyFdu588hQLNNFo"
            + "r7G40NvGOYude1B16sckNng8bVZvWOjbQlNct/ZveQGmCZ1mdnOrsNN+3hQ799EjL56ZcMoZk9gey90"
            + "vnNk6cOje+/becuu6hb+yuueGbZ1aXdCpwbV0JoS2wol+cFPC5xMnm52G4Zg1Eh/hiOjYN4xE8MvEYR"
            + "l93PbvNy1WDz/95r5Dx98gs6eWqFcjJKjin8XPg9CMDnl8xUPZMTz5vho8aWm6d7ZdbcToUuykuUiL2"
            + "dbbzx40lGNjA6F9+e4VzxFnl127/gVExKthKygdVQAAAABJRU5ErkJggg==";

        private static readonly Image OriginalThumbnail;

        static Resources()
        {
            Debugger.WriteLine("Loading original generic maid thumbnail...");
            byte[] thumbnail = Convert.FromBase64String(ThumbnailBase64);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(thumbnail, 0, thumbnail.Length);
                OriginalThumbnail = Image.FromStream(ms);
            }
        }

        public static Image DefaultThumbnail { get; private set; }

        public static void InitThumbnail()
        {
            Debugger.Assert(() =>
            {
                Debugger.WriteLine("Loading generic maid thumbnail...");
                float size = Translation.GetTranslation("THUMB_TEXT_SIZE").Parse(10.0F);
                float posX = Translation.GetTranslation("THUMB_TEXT_POS_X").Parse(0.0F);
                float posY = Translation.GetTranslation("THUMB_TEXT_POS_Y").Parse(0.0F);

                Debugger.WriteLine(LogLevel.Info, $"Font size: {size}, posX: {posX}, posY: {posY}");

                DefaultThumbnail = new Bitmap(OriginalThumbnail);
                using (Graphics g = Graphics.FromImage(DefaultThumbnail))
                {
                    Font f = new Font("Arial", size);
                    g.DrawString(Translation.GetTranslation("THUMB_NONE"), f, new SolidBrush(Color.Black), posX, posY);
                }
            }, "Failed to load the generic maid thumbnail");
        }
    }
}