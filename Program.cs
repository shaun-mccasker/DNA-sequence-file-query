using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp5
{    
    class Part1
    {
        /// <summary>
        /// Outputs the StartPos and EndPos for any given sequence within the text document
        /// </summary>
        /// <param name="startPos">start position to begin from </param>
        /// <param name="endPos">end position to end at </param>
        /// <param name="arg_position"></param>
        public static void Level_1_parameter_setup(out int startPos, out int endPos, string[] arg_position)
        {
            int linesToRead;

            startPos = Convert.ToInt32(arg_position[2]) - 1; // set StartPos to arg2, -1 as it starts from 0
            linesToRead = Convert.ToInt32(arg_position[3]) * 2; // set linesToRead to arg3, *2 as we read double the lines inputted
            endPos = linesToRead + startPos; // set endPos to be the start pos + amount of lines to read.

        }

        /// <summary>
        /// Conducts a file search using a starting pos/line and amount of lines to read and outputs to console the lines relevent to the search.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="arg_position"></param>
        public static void Level_1(int startPos, int endPos, List<int> pos, List<int> size, string[] arg_position)
        {
            using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
            {

                for (int n = startPos; n < endPos; n++) //count from start position to end position
                {
                    byte[] bytes = new byte[size[n]];
                    fs.Seek(pos[n], SeekOrigin.Begin);
                    fs.Read(bytes, 0, size[n]);
                    Console.WriteLine(Encoding.Default.GetString(bytes));
                }
            }
        }

        /// <summary>
        /// Conducts a file search using a sequence of chracters and outputs to console the lines relevent to the search.
        /// </summary>
        /// <param name="counter"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="arg_position"></param>
        public static void Level_2(int counter, List<int> pos, List<int> size, string[] arg_position)
        {
            using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
            {

                bool found = false;
                for (int n = 0; n < counter; n++) //count from start position to end position
                {
                    byte[] bytes = new byte[size[n]];
                    fs.Seek(pos[n], SeekOrigin.Begin);
                    fs.Read(bytes, 0, size[n]);
                    string line = Encoding.Default.GetString(bytes); //convert bytes to string
                    if (line.Contains(arg_position[2])) // check if string contains arg[2]
                    {
                        byte[] next_bytes = new byte[size[n + 1]]; // print the string which contains it and following string sequence
                        fs.Seek(pos[n + 1], SeekOrigin.Begin);
                        fs.Read(next_bytes, 0, size[n + 1]);
                        Console.WriteLine(line);
                        Console.WriteLine(Encoding.Default.GetString(next_bytes));
                        found = true;
                    }

                }
                if (!found)// if sequence not found
                {
                    Console.WriteLine("Error, sequence {0} not found.", arg_position[2]);

                }

            }
        }
        /// <summary>
        /// reads all lines within document and stores the lines within a list variable
        /// </summary>
        /// <param name="query_lines">List to store lines from text document</param>
        /// <param name="arg_position"></param>
        public static void Level_3_query_read(out List<string> query_lines, string[] arg_position)
        {
            string query_file = arg_position[2];
            query_lines = File.ReadAllLines(query_file).ToList();
        }
        /// <summary>
        /// Conducts a file search using a sequence of string within a list and outputs to a text doucment the lines relevent to the search.
        /// </summary>
        /// <param name="query_lines"></param>
        /// <param name="arg_position"></param>
        public static void Level_3(int counter, List<string> query_lines, List<int> pos, List<int> size, string[] arg_position)
        {
            string results_file = arg_position[3]; // get file name to export to   

            string query_file = arg_position[2];
            query_lines = File.ReadAllLines(query_file).ToList();
            File.WriteAllText(results_file, String.Empty);
            StreamWriter outputFile = new StreamWriter(results_file, append: true);
            using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
            {

                bool[] found = new bool[query_lines.Count];
                for (int n = 0; n < counter; n++) //count from start position to end position
                {
                    byte[] bytes = new byte[size[n]];
                    fs.Seek(pos[n], SeekOrigin.Begin);
                    fs.Read(bytes, 0, size[n]);
                    string line = Encoding.Default.GetString(bytes); //convert bytes to string

                    for (int i = 0; i < query_lines.Count; i++)
                    {
                        if (line.Contains(query_lines[i])) // check if string contains arg[2]
                        {
                            byte[] next_bytes = new byte[size[n + 1]]; //add strings which contain sequence to list
                            fs.Seek(pos[n + 1], SeekOrigin.Begin);
                            fs.Read(next_bytes, 0, size[n + 1]);
                            //write to file
                            outputFile.WriteLine("{0}", line);
                            outputFile.WriteLine("{0}", Encoding.Default.GetString(next_bytes));
                            found[i] = true;
                        }
                    }
                }

                for (int i = 0; i < query_lines.Count; i++)
                {
                    if (!found[i])// if sequence not found
                    {
                        Console.WriteLine("Error, sequence {0} not found.", query_lines[i]);

                    }
                    if (found[i])
                    {
                        Console.WriteLine("Sequence {0} found, check the {1} document for more details.", query_lines[i], arg_position[3]);
                    }
                }

            }
            outputFile.Close();
        }
    }
    class Part2
    {
        /// <summary>
        /// searches index file via DNA string. input and output via text files
        /// </summary>
        /// <param name="counter"> counts the amount of lines</param>
        /// <param name="position"> position of each line within the text document</param>
        /// <param name="pos">stores positions into a list</param>
        /// <param name="size">store line size into a list</param>
        /// <param name="arg_position">main argument</param>
        public static void Level_4_search(int counter, List<int> pos, List<int> size, string[] arg_position)
        {
            Console.WriteLine("-----\nSearching {1} using {0}...", arg_position[2], arg_position[3]);
            if (Helper.Text_is_valid(arg_position, 2))
            {
                if (Helper.Text_is_valid(arg_position, 3))
                {
                    if (Helper.Results_file_valid(arg_position, ".txt"))
                    {
                        List<string> index_seqid = File.ReadAllLines(arg_position[2]).ToList(); //store all index points to search from 
                        List<string> query_file = File.ReadAllLines(arg_position[3]).ToList(); //store  all seq_id to search by 
                        List<int> offset = new List<int>();
                        string seqid, idseq;
                        int byte_size, byte_size_seq;
                        string results_file = arg_position[4]; // file to output to   
                        File.WriteAllText(results_file, String.Empty);
                        StreamWriter outputFile = new StreamWriter(results_file, append: true);

                        foreach (string seq_id in index_seqid) //check every seq_id to match with each query within the query file
                        {
                            foreach (string query in query_file)
                            {
                                if (seq_id.Contains(query)) // if seq_id contains query
                                {
                                    int query_offset = Convert.ToInt32(seq_id.Substring(seq_id.LastIndexOf(' '))); //convert seq_id into int 
                                    offset.Add(query_offset);
                                }
                            }
                        }
                        using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read)) //search file fasta file using offset and store information in ouput file.
                        {
                            using (StreamReader reader = new StreamReader(fs))
                            {
                                for (int i = 0; i < offset.Count; i++)
                                {
                                    //get byte size
                                    int offset_count = pos.FindIndex(a => a == (offset[i]));
                                    byte_size = size[offset_count];

                                    //get sequence id
                                    byte[] bytes_seqid = new byte[byte_size];
                                    fs.Seek(offset[i], SeekOrigin.Begin);
                                    fs.Read(bytes_seqid, 0, byte_size);
                                    seqid = Encoding.Default.GetString(bytes_seqid);

                                    //get byte seize and next line offset
                                    int seq_offset = byte_size += offset[i];
                                    byte_size_seq = size[offset_count + 1];
                                    //get sequence
                                    byte[] new_byte = new byte[byte_size_seq];
                                    fs.Seek(seq_offset, SeekOrigin.Begin);
                                    fs.Read(new_byte, 0, byte_size_seq);
                                    idseq = Encoding.Default.GetString(new_byte);
                                    outputFile.WriteLine(seqid);
                                    outputFile.WriteLine(idseq);
                                }
                            }
                        }
                        outputFile.Close();
                        Console.WriteLine("Search Complete; results can be found in {0}\n-----", results_file);
                    }
                    else
                    {
                        Console.WriteLine("ERROR, {0} is an invalid text file. Please make the results file is text file. EG: 'results.txt'", arg_position[4]);
                    }

                }
                else
                {
                    Console.WriteLine("ERROR, {0} is an invalid text file. Please make sure the text file exists within the appropriate folder", arg_position[3]);
                }

            }
            else
            {
                Console.WriteLine("ERROR, {0} is an invalid text file. Please make sure the text file exists within the appropriate folder", arg_position[2], arg_position[3]);
            }

        }

        /// <summary>
        /// searches text file via DNA query string. input and output via console commands
        /// </summary>
        /// <param name="counter"> counts the amount of lines</param>
        /// <param name="position"> position of each line within the text document</param>
        /// <param name="pos">stores positions into a list</param>
        /// <param name="size">store line size into a list</param>
        /// <param name="arg_position">main argument</param>
        public static void Level_5(int counter, List<int> pos, List<int> size, string[] arg_position)
        {
            string user_input = arg_position[2];
            user_input = user_input.ToUpper();
            Console.WriteLine("-----\nThe following sequences ids contain '{0}'", user_input);//text buffer
            using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
            {
                bool found = false;
                int print_check = 0;
                for (int n = 0; n < counter; n++) //count from start position to end position
                {
                    byte[] bytes = new byte[size[n]];
                    fs.Seek(pos[n], SeekOrigin.Begin);
                    fs.Read(bytes, 0, size[n]);
                    string line = Encoding.Default.GetString(bytes); //convert bytes to string
                    if (line.Contains(user_input)) // check if string contains arg[2]
                    {
                        byte[] next_bytes = new byte[size[n]]; // print the string which contains it and following string sequence
                        fs.Seek(pos[n - 1], SeekOrigin.Begin);
                        fs.Read(next_bytes, 0, size[n]);
                        string new_byte_string = Encoding.Default.GetString(next_bytes);
                        //fix this to substring
                        string[] elements = new_byte_string.Split(' ');
                        foreach (string elem in elements) //if any elements within the string contain '>' then print them 
                        {
                            if (elem.StartsWith(">"))
                            {
                                Console.WriteLine(elem);
                                print_check++;
                            }
                        }
                        found = true;
                    }
                }
                if (!found | print_check == 0)// if sequence not found
                {
                    Console.WriteLine("Error, no matching Sequence IDs");
                }
            }
            Console.WriteLine("Search End\n-----");//text buffer
        }

        /// <summary>
        /// searches text file via sequence-ids containing specified word. input and output via console commands
        /// </summary>
        /// <param name="counter"> counts the amount of lines</param>
        /// <param name="position"> position of each line within the text document</param>
        /// <param name="pos">stores positions into a list</param>
        /// <param name="size">store line size into a list</param>
        /// <param name="arg_position">main argument</param>
        public static void Level_6(int counter, List<int> pos, List<int> size, string[] arg_position)
        {

            bool found = false;
            string id_to_search = arg_position[2];//store arg2

            //char[] id_chars = (id_to_search.ToLower()).ToCharArray();//conver arg2 to lower case and turn into char array            
            //id_chars[0] = char.ToUpper(id_chars[0]);//make sure first letter is capital
            //id_to_search = new string(id_chars);//convert charyarray back to string

            Console.WriteLine("-----\nThe following sequence ids contain '{0}':", id_to_search);//text buffer   
            
            using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
            {
                for (int n = 0; n < counter; n++) //count from start position to end position
                {
                    byte[] bytes = new byte[size[n]];
                    fs.Seek(pos[n], SeekOrigin.Begin);
                    fs.Read(bytes, 0, size[n]);
                    string line = Encoding.Default.GetString(bytes); //convert bytes to string
                    if (line.Contains(id_to_search)) // check if string contains arg[2]
                    {
                        foreach (string elem in line.Split(' '))
                        {
                            if (elem.StartsWith(">")) // print sequences without '>'
                            {
                                Console.WriteLine(elem.TrimStart('>'));
                            }
                        }
                        found = true;
                    }
                }
                if (!found)// if sequence not found
                {
                    Console.WriteLine("Error, no matching Sequence IDs");
                }
            }
            Console.WriteLine("Please Note that your search is case sensitive\nSearch End\n-----");//text buffer
        }
        // not done regex
        public static void Level_7(int counter, List<int> pos, List<int> size, string[] arg_position)
        {
            string user_input = arg_position[2];
            if (user_input.Contains('*'))
            {
                user_input = user_input.ToUpper(); // make all chracters upper case            
                Console.WriteLine("-----\nThe following DNA Sequences contain '{0}'", user_input.Replace("*", @"|ANY CHAR|"));//text buffer
                user_input = user_input.Replace("*", @"(.*?)"); //repalce '*' with regex expression             
                using (FileStream fs = new FileStream(arg_position[1], FileMode.Open, FileAccess.Read))
                {
                    for (int n = 0; n < counter; n++) //count from start position to end position
                    {
                        byte[] bytes = new byte[size[n]];
                        fs.Seek(pos[n], SeekOrigin.Begin);
                        fs.Read(bytes, 0, size[n]);
                        Match match = Regex.Match(Encoding.Default.GetString(bytes), user_input); //match line with regex expression
                        if (match.Success) //return lines containing query
                        {
                            byte[] new_bytes = new byte[size[n - 1]];
                            fs.Seek(pos[n - 1], SeekOrigin.Begin);
                            fs.Read(new_bytes, 0, size[n - 1]);
                            Console.WriteLine("{0}{1}", Encoding.Default.GetString(new_bytes), Encoding.Default.GetString(bytes));
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Please input a regex expression, where '*' stand for any character. EG: ACTG*GTAC*CA ");
            }

            Console.WriteLine("Search End\n-----");//text buffer
        }
    }
    class Program
    {
        /// <summary>
        /// checks console input and runs specified functions
        /// </summary>
        /// <param name="counter"> counts the amount of lines</param>
        /// <param name="position"> position of each line within the text document</param>
        /// <param name="pos">stores positions into a list</param>
        /// <param name="size">store line size into a list</param>
        /// <param name="arg_position">main argument</param>
        public static void Read_files(int counter, List<int> pos, List<int> size, string[] arg_position)
        {
            if (Helper.Text_is_valid(arg_position, 1))
            {
                if (Helper.Level_is_valid(arg_position))
                {
                    switch (arg_position[0])
                    {
                        case "-level1":
                            if (Helper.Input_amount_valid(arg_position, 4))//where 3 is the expected amunt of args for level 1
                            {
                                Part1.Level_1_parameter_setup(out int startPos, out int endPos, arg_position);
                                Part1.Level_1(startPos, endPos, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level1 expects {0} arguments but received {1}", 4, arg_position.Length);
                            }
                            break;

                        case "-level2":
                            if (Helper.Input_amount_valid(arg_position, 3))//where 3 is the expected amunt of args for level 1
                            {
                                Part1.Level_2(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level2 expects {0} arguments but received {1}", 3, arg_position.Length);
                            }
                            break;

                        case "-level3":
                            if (Helper.Input_amount_valid(arg_position, 4))//where 3 is the expected amunt of args for level 1
                            {
                                Part1.Level_3_query_read(out List<string> query_lines, arg_position);
                                Part1.Level_3(counter, query_lines, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level3 expects {0} arguments but received {1}", 4, arg_position.Length);
                            }
                            break;
                        case "-index":
                            if (Helper.Input_amount_valid(arg_position, 3))//where 3 is the expected amunt of args for level 1
                            {
                                Helper.Level_4_index(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("index expects {0} arguments but received {1}", 3, arg_position.Length);
                            }
                            break;
                        case "-level4":
                            if (Helper.Input_amount_valid(arg_position, 5))//where 5 is the expected amunt of args for level 4
                            {
                                Part2.Level_4_search(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level4 expects {0} arguments but received {1}", 5, arg_position.Length);
                            }
                            break;
                        case "-level5":
                            if (Helper.Input_amount_valid(arg_position, 3))//where 3 is the expected amunt of args for level 1
                            {
                                Part2.Level_5(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level5 expects {0} arguments but received {1}", 3, arg_position.Length);
                            }
                            break;
                        case "-level6":
                            if (Helper.Input_amount_valid(arg_position, 3))//where 3 is the expected amunt of args for level 1
                            {
                                Part2.Level_6(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level6 expects {0} arguments but received {1}", 3, arg_position.Length);
                            }
                            break;
                        case "-level7":
                            if (Helper.Input_amount_valid(arg_position, 3))//where 3 is the expected amunt of args for level 1
                            {
                                Part2.Level_7(counter, pos, size, arg_position);
                            }
                            else
                            {
                                Console.WriteLine("level7 expects {0} arguments but received {1}", 3, arg_position.Length);
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("ERROR, {0} is an invalid level flag. Please make sure the flag begins with a '-'. eg:'-level1'", arg_position[0]);
                }
            }
            else
            {
                Console.WriteLine("ERROR, {0} is an invalid text file. Please make sure the text file exists within the appropriate folder", arg_position[1]);
            }


        }

        static void Main(string[] args)
        {
            //intisialise variables
            int counter = 0;
            int position = 0;
            List<int> pos = new List<int>();
            List<int> size = new List<int>();

            //try to read file, any incorect argument types/amounts will result in a catch
            Helper.Setup_file_counters(ref counter, ref position, ref pos, ref size, args);
            Read_files(counter, pos, size, args);
            Console.WriteLine("press any button to exit");
            Console.ReadLine();
        }
    }
}
