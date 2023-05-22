using System;
using System.Runtime.InteropServices;

namespace UnsafeTest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DoublyLinkedList list = new();
            list.AddFirst(new Person { Name = "1", Age = 1 });
            //Console.WriteLine(list); 
            list.AddFirst(new Person { Name = "2", Age = 2 });
            //Console.WriteLine(list); 
            list.AddFirst(new Person { Name = "3", Age = 3 });
            //Console.WriteLine(list); 
            list.AddLast(new Person { Name = "4", Age = 4 });
            //Console.WriteLine(list); 
            list.AddLast(new Person { Name = "5", Age = 5 });
            //Console.WriteLine(list); 
            list.Insert(3, new Person { Name = "6", Age = 6 });
            //Console.WriteLine(list); 
        }
    }

    public struct Person
    {
        public string Name;
        public int Age;
    }

    public struct DoublyLinkedList
    {
        public IntPtr Head;
        public IntPtr Tail;

        public int Count;

        public void AddFirst(Person p)
        {
            DoublyLinkedListNode newNode = new()
            {
                Data = p
            };

            if (Count == 0)
            {
                Head = Marshal.AllocHGlobal(Marshal.SizeOf(newNode));
                Marshal.StructureToPtr(newNode, Head, false);
                Tail = Head;
            }
            else
            {
                DoublyLinkedListNode oldHead = default;
                Marshal.PtrToStructure(Head, oldHead);
                IntPtr newNodePtr = Marshal.AllocHGlobal(Marshal.SizeOf(newNode));
                oldHead.Previous = newNodePtr;
                newNode.Next = Head;
                Marshal.StructureToPtr(newNode, newNodePtr, false);
                Head = newNodePtr;
            }
            Count++;
        }

        public void AddLast(Person p)
        {
            DoublyLinkedListNode newNode = new()
            {
                Data = p
            };

            if (Count == 0)
            {
                Head = Marshal.AllocHGlobal(Marshal.SizeOf(newNode));
                Marshal.StructureToPtr(newNode, Head, false);
                Tail = Head;
            }
            else
            {
                DoublyLinkedListNode oldTail = Marshal.PtrToStructure<DoublyLinkedListNode>(Tail);
                IntPtr newNodePtr = Marshal.AllocHGlobal(Marshal.SizeOf(newNode));
                oldTail.Next = newNodePtr;
                newNode.Previous = Tail;
                Marshal.StructureToPtr(newNode, newNodePtr, false);
                Tail = newNodePtr;
            }
            Count++;
        }

        public void Insert(int index, Person p)
        {
            DoublyLinkedListNode newNode = new()
            {
                Data = p
            };

            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == 0)
            {
                AddFirst(p);
            }
            else if (index == Count)
            {
                AddLast(p);
            }
            else
            {
                // текущая нода 
                DoublyLinkedListNode current = Marshal.PtrToStructure<DoublyLinkedListNode>(Head);
                // указатель на текущую ноду 
                IntPtr currentPtr = Head;

                // перебираем все ноды пока не дойдем до нужной ноды 
                for (int i = 0; i < index - 1; i++)
                {
                    currentPtr = current.Next;
                    current = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Next);
                }

                // создаем указатель на новую ноду 
                IntPtr newNodePtr = Marshal.AllocHGlobal(Marshal.SizeOf(newNode));
                // привязываем новую ноду к указателю

                Marshal.StructureToPtr(newNode, newNodePtr, false);
                // следующая нода для новой ноды указывает на текущую ноду 
                newNode.Next = currentPtr;
                // предыдущая нода для новой ноды указывает на предыдущую ноду текущей ноды 
                newNode.Previous = current.Previous;
                // получаем предыдущую ноду текущей ноды 
                DoublyLinkedListNode currentPrevious = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Previous);
                // следующая нода для предыдущей ноды текущей ноды указывает на текущую ноду 
                currentPrevious.Next = newNodePtr;
                // предыдущая нода для текущей ноды указывает на новую ноду 
                current.Previous = newNodePtr;
            }
            Count++;
        }
    }
    public struct DoublyLinkedListNode
    {
        public IntPtr Next { get; set; }
        public IntPtr Previous { get; set; }
        public Person Data { get; set; }

    }
}