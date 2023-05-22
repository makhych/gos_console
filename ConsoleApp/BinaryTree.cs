using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Node
    {
        public int value;
        public Node left;
        public Node right;

        public Node(int data)
        {
            value = data;
            left = null;
            right = null;
        }
    }

    class BinaryTree
    {
        public Node root;

        public BinaryTree()
        {
            root = null;
        }

        public void Insert(int value)
        {
            root = InsertRecursive(root, value);
        }

        private Node InsertRecursive(Node current, int value)
        {
            if (current == null)
            {
                return new Node(value);
            }

            if (value < current.value)
            {
                current.left = InsertRecursive(current.left, value);
            }
            else if (value > current.value)
            {
                current.right = InsertRecursive(current.right, value);
            }

            return current;
        }

        public bool Search(int value)
        {
            return SearchRecursive(root, value);
        }

        private bool SearchRecursive(Node current, int value)
        {
            if (current == null)
            {
                return false;
            }

            if (value == current.value)
            {
                return true;
            }

            if (value < current.value)
            {
                return SearchRecursive(current.left, value);
            }

            return SearchRecursive(current.right, value);
        }

        public void Delete(int value)
        {
            root = DeleteRecursive(root, value);
        }

        private Node DeleteRecursive(Node current, int value)
        {
            if (current == null)
            {
                return null;
            }

            if (value < current.value)
            {
                current.left = DeleteRecursive(current.left, value);
            }
            else if (value > current.value)
            {
                current.right = DeleteRecursive(current.right, value);
            }
            else
            {
                // Узел для удаления найден

                // Узел без потомков или с одним потомком
                if (current.left == null)
                {
                    return current.right;
                }
                else if (current.right == null)
                {
                    return current.left;
                }

                // Узел с двумя потомками
                current.value = FindMinValue(current.right);

                // Удалить наименьший элемент из правого поддерева
                current.right = DeleteRecursive(current.right, current.value);
            }

            return current;
        }

        private int FindMinValue(Node node)
        {
            int minValue = node.value;
            while (node.left != null)
            {
                minValue = node.left.value;
                node = node.left;
            }
            return minValue;
        }

    }
}
