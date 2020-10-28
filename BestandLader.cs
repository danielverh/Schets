using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SchetsEditor
{
    public class BestandLader
    {
        public static List<Shape> LaadSchets(string file)
        {
            var bf = new BinaryFormatter();
            List<Shape> data;
            using (var stream = new FileStream(file, FileMode.Open))
            {
                data = (List<Shape>)bf.Deserialize(stream);
            }

            return data;
        }

        public static void SchetsOpslaan(string file, List<Shape> shape)
        {
            var bf = new BinaryFormatter();
            // using (resource)
            // {
            //     
            // }
        }
    }
}
