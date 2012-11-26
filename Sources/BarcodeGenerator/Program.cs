using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BarcodeGenerator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 



        [STAThread]
        static void Main()
        {
            int count = 0;

            LinkedList<int> nums = new LinkedList<int>();

            for (int num = 0; num < 1024; ++num)
            {
                string bin = Convert.ToString(num, 2);
                for (int i = bin.Length; i <= 10; ++i)
                    bin = "0" + bin;

                if (bin.IndexOf("0000") == -1 && bin.IndexOf("1111") == -1 
                    && !bin.StartsWith("111")
                    && !bin.EndsWith("00") && !bin.EndsWith("11"))
                {
                    Console.WriteLine("{0} {1}", num, bin);
                    nums.AddLast(num);
                }
            }

            Console.WriteLine("Found: {0}", nums.Count);

            Console.Write("{ ");
            foreach (int num in nums)
                Console.Write("{0}, ", num);
            Console.WriteLine("}");

            Console.ReadKey();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GeneratorForm());



        }
    }
}
