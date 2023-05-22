using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***");

            ////Console.ReadLine().Split(new[] { " ", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

            //var array = new int[10];

            //for (int i = 0; i < 10; i++)
            //{
            //    array[i] = Convert.ToInt32(Console.ReadLine());
            //}

            //Console.WriteLine("Отсортированный массив: {0}", string.Join(", ", Sorts.BubbleSort(array)));



            BinaryTree bst = new BinaryTree();

            bst.Insert(50);
            bst.Insert(30);
            bst.Insert(20);
            bst.Insert(40);
            bst.Insert(70);
            bst.Insert(60);
            bst.Insert(80);

            Console.WriteLine(bst.Search(60)); // Output: True
            Console.WriteLine(bst.Search(90)); // Output: False

            PrintTreeRecursive(bst.root);
        }


        public static void PrintTreeRecursive(Node current, string indent = "", bool isRight = false)
        {
            if (current == null)
                return;

            Console.Write(indent);

            if (isRight)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "│ ";
            }

            Console.WriteLine(current.value);

            PrintTreeRecursive(current.left, indent, false);
            PrintTreeRecursive(current.right, indent, true);
        }

    }
}
