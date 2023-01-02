using System;
using System.Drawing;
using System.IO;
using System.Xml;
using ImageProcessor;
using ImageProcessor.Imaging;


namespace SparrowAtalsSizer
{
    internal class Program
    {
        public static string[] xml_vars =
        {
            "x", "y", "width", "height",
            "frameX", "frameY", "frameWidth", "frameHeight"
        };
        
        public static void Resize(string folder, float factor)
        {
            XmlDocument doc = new XmlDocument();

            string split = Directory.GetCurrentDirectory() + "\\" + folder.Split('.')[0];
            
            doc.Load(split + ".xml");

            if (doc.DocumentElement == null)
            {
                Console.WriteLine("yo fucked it");
                return;
            }

            ImageFactory factory = new ImageFactory();
            factory.Load(split + ".png");


            ResizeLayer l = new ResizeLayer(new Size((int)(factory.Image.Width * factor), (int)(factory.Image.Height * factor)), ResizeMode.Min, AnchorPosition.TopLeft, false);

            factory.Resize(l);

            factory.Save(split + ".png");
            Console.WriteLine("Saved " + split + ".png");        
    
            foreach (XmlNode n in doc.DocumentElement.ChildNodes)
            {
                
                if (n.Name == "SubTexture")
                    foreach (string s in xml_vars)
                        foreach(XmlAttribute a in n.Attributes)
                            if (a.Name == s)
                                a.Value = (int.Parse(a.Value) * factor) + "";
            }
            doc.Save(split + ".xml");
            Console.WriteLine("Saved " + split + ".xml");
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Folder/image (Will not recurse through every folder): ");

            string folder = Console.ReadLine();

            if (folder == null)
            {
                Console.WriteLine("yo fucked it");
                return;
            }
            Console.WriteLine("Factor: ");

            float factor = 1;

            float.TryParse(Console.ReadLine(), out factor);
            
            Console.WriteLine("btw this will overwrite the original files, so make sure you know that. enter anything to say ok");

            Console.ReadLine(); // buffer
            
            Console.WriteLine("\n\nRESIZING IMAGES BY A FACTOR OF " + factor + "x\n\n");
            
            if (folder.Contains(".")) // file
                Resize(folder, factor);
            else
            {
                foreach (string f in Directory.GetFiles(folder))
                {
                    if (f.EndsWith(".xml"))
                        Resize(f, factor);
                }
            }
            
            Console.WriteLine("End of resize");

            Console.ReadLine();
        }
    }
}