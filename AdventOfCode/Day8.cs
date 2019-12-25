using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode
{
    class Day8
    {
        static void Main(string[] args)
        {
            var imgB = new ImageHandler();
            string input = File.ReadAllText("day8picture.txt");
            string[] pictures = input.Split('\n');
            List<List<List<string>>> image = ImageHandler.Parse(pictures[0], 25, 6);

            Console.WriteLine(imgB.PrintLayerRepresentation(image));
            var layer = imgB.FindLayerWithFewest0s(image);
            Console.WriteLine("layer to check: " + layer);
            Console.WriteLine("checksum: " + imgB.ChecksumOfLayer(image[layer]));
            Console.WriteLine(imgB.Print(imgB.Render(image)));
        }


    }

    class ImageHandler
    {
        public List<List<string>> Render(List<List<List<string>>> image)
        {
            var img = new List<List<string>>();

            // setup image with first layer
            img = image[0];


            // render layers beneath
            for(int row = 0; row < img.Count; row++) 
            {
                for(int index = 0; index < img[row].Count; index++)
                {
                    if (img[row][index].Equals("2")) // if transparent
                    {
                        // we have layer 0 as base
                        int layer = 1;
                        // find not transparent
                        while (image[layer][row][index].Equals("2") && layer < image.Count)
                        {
                            layer++;
                        }
                        img[row][index] = image[layer][row][index];
                    }
                }
            }

            return img;
        }


        public static List<List<List<string>>> Parse(string encoding, int width, int height)
        {
            var layers = new List<List<List<string>>>();
            var pixel = 0;

            while (pixel < encoding.Length)
            {
                var layer = new List<List<string>>();
                for (int y = 0; y < height; y++)
                {
                    var row = new List<string>();
                    for (int x = 0; x < width; x++)
                    {
                        row.Add(encoding[pixel].ToString());
                        pixel++;
                    }
                    layer.Add(row);

                }
                layers.Add(layer);
            }

            return layers;
        } 

        public int ChecksumOfLayer(List<List<string>> layer)
        {
            var sum = 0;
            var ones = 0;
            var twos = 0;

            foreach(List<string> row in layer)
            {
                foreach(string number in row)
                {
                    if (number.Equals("1"))
                    {
                        ones++;
                    }
                    else if (number.Equals("2"))
                    {
                        twos++;
                    }
                }
            }

            sum = ones * twos;

            return sum;
        }

        public int FindLayerWithFewest0s(List<List<List<string>>> image)
        {
            var layerNumber = "error";
            int fewest0s = 999999999;

            foreach(List<List<string>> layer in image)
            {
                int zeros = 0;
                foreach(List<string> row in layer)
                {
                    foreach(string number in row)
                    {
                        if (number.Equals("0"))
                        {
                            zeros++;
                        }
                    }
                }
                if(zeros < fewest0s)
                {
                    layerNumber = image.IndexOf(layer).ToString();
                    fewest0s = zeros;
                }
            }

            return int.Parse(layerNumber);
        }



        public string Print(List<List<string>> image)
        {
            var img = "";

            foreach(List<string> row in image)
            {
                foreach(string pixel in row)
                {
                    if (pixel.Equals("0"))
                    {
                        img = img + " ";
                    }
                    else if (pixel.Equals("1"))
                    {
                        img = img + "■";
                    }
                    else
                    {
                        img = img + pixel;
                    }
                }
                img = img + "\n";
            }

            return img;
        }

        public string PrintLayerRepresentation(List<List<List<string>>> image)
        {
            var rep = "";

            foreach(List<List<string>> layer in image)
            {
                rep = rep + "Layer " + image.IndexOf(layer) + ": ";
                foreach(List<string> row in layer)
                {
                    foreach(string s in row)
                    {
                        rep = rep + s;
                    }
                    rep = rep + "\n         ";
                    if(image.IndexOf(layer) > 9)
                    {
                        rep = rep + " ";
                    }
                }
                rep = rep + "\n";
            }

            return rep;
        }
    }

    enum colours
    {
        transparent = 2,
        black = 1,
        white = 0
    }
}
