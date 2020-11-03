using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SchetsEditor
{
    public class BestandLader
    {
        /// <summary>
        /// Gebruik een binary formatter om een bestand met de data van de afbeelding te deserializeren.
        /// </summary>
        /// <param name="file">Het bestand</param>
        /// <returns>Een reeks van shape objecten die getekent kunnen worden</returns>
        public static BestandObject LaadSchets(string file)
        {
            var bf = new BinaryFormatter();
            BestandObject data;
            using (var stream = new FileStream(file, FileMode.Open))
            {
                try
                {
                    data = (BestandObject) bf.Deserialize(stream);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return data;
        }

        /// <summary>
        /// Gebruik een binary formatter om een object met de data van de afbeelding te serializeren.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="data"></param>
        public static void SchetsOpslaan(string file, Size grootte, List<Shape> data)
        {
            var bf = new BinaryFormatter();
            using (var stream = new FileStream(file, FileMode.Create))
            {
                bf.Serialize(stream, new BestandObject(grootte, data));
            }
        }

        [Serializable]
        public class BestandObject
        {
            public Size Grootte { get; }
            public List<Shape> ShapesList() => shapes.ToList();
            private Shape[] shapes;

            public BestandObject(Size _grootte, List<Shape> _shapes)
            {
                Grootte = _grootte;
                shapes = _shapes.ToArray();
            }
        }
    }
}