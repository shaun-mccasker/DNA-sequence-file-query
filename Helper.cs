using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp5
{
    class Helper
    {
        /// <summary>
        /// Gets the position, size and index of each line within a text file which is defined via a command line argument, argument 1.
        /// </summary>
        /// <param name="counter"> counts the amount of lines</param>
        /// <param name="position"> position of each line within the text document</param>
        /// <param name="pos">stores positions into a list</param>
        /// <param name="size">store line size into a list</param>
        /// <param name="arg_position">main argument</param>
        public static void Setup_file_counters(ref int counter, ref int position, ref List<int> pos, ref List<int> size, string[] arg_position)
        {
            string line;
            if (Helper.Text_is_valid(arg_position, 1))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(arg_position[1]);
                while ((line = file.ReadLine()) != null)
                {
                    pos.Insert(counter, position);
                    size.Insert(counter, line.Length + 1);
                    counter++;
                    position = position + line.Length + 1;
                }
            }
        }
        public static bool Text_is_valid(string[] arg_position, int arg_pos)
        {
            string text_file = arg_position[arg_pos];
            if (File.Exists(text_file)) // check if a given text file is valid
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Level_is_valid(string[] arg_position)
        {
            string[] levels = { "-level1", "-level2", "-level3", "-level4", "-level5", "-level6", "-level7", "-index" };
            //only for levels 4-7 as they are being marked
            if (arg_position[0] == levels[0] || arg_position[0] == levels[1] || arg_position[0] == levels[2] || arg_position[0] == levels[3] || arg_position[0] == levels[4] || arg_position[0] == levels[5] || arg_position[0] == levels[6] || arg_position[0] == levels[7])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Results_file_valid(string[] arg_position, string extention)
        {
            if (arg_position[4].Contains(extention))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
        public static bool Input_amount_valid(string[] args, int expected_amount)
        {
            if (args.Length == expected_amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void Level_4_index(int counter, List<int> pos, List<int> size, string[] arg_position)
        {

            string line;
            string results_file = arg_position[2];
            if (results_file.Contains(".index"))
            {
                File.WriteAllText(results_file, String.Empty);
                StreamWriter outputFile = new StreamWriter(results_file, append: true);
                using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
                {
                    for (int n = 0; n < counter; n++) //count from start position to end position
                    {
                        byte[] bytes = new byte[size[n]];
                        fs.Seek(pos[n], SeekOrigin.Begin);
                        fs.Read(bytes, 0, size[n]);
                        line = Encoding.Default.GetString(bytes);
                        if (line.StartsWith(">NR_"))
                        {
                            outputFile.WriteLine("{0} {1}", line.Substring(0, 12), fs.Seek(pos[n], SeekOrigin.Begin));
                        }
                    }
                    Console.WriteLine("Index File Created");
                }
                outputFile.Close();
            }
            else
            {
                Console.WriteLine("Argument 2 invalid. Please make sure it is of '.index' file type.");

            }
        }
    }
}
