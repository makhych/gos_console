using System;
using System.Runtime.InteropServices;

namespace UnsafeTest
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DoublyLinkedList list = new();
            list.AddFirst(new Person { Name = "Person 1", Age = 1 });
            Console.WriteLine(list); 
            list.AddFirst(new Person { Name = "Person 2", Age = 2 });
            Console.WriteLine(list); 
            list.AddFirst(new Person { Name = "Person 3", Age = 3 });
            Console.WriteLine(list); 
            list.AddLast(new Person { Name = "Person 4", Age = 4 });
            Console.WriteLine(list); 
            list.AddLast(new Person { Name = "Person 5", Age = 5 });
            Console.WriteLine(list); 
            list.Insert(3, new Person { Name = "Person 6", Age = 6 });
            Console.WriteLine(list);
            list.RemoveFirst();
            Console.WriteLine(list);
            list.RemoveLast();
            Console.WriteLine(list);
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
                Head = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));
                Marshal.StructureToPtr(newNode, Head, false);
                Tail = Head;
            }
            else
            {
                DoublyLinkedListNode oldHead = Marshal.PtrToStructure<DoublyLinkedListNode>(Head);
                IntPtr newNodePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));
                oldHead.Previous = newNodePtr;
                newNode.Next = Head;
                Marshal.StructureToPtr(oldHead, Head, false);
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
                Head = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));
                Marshal.StructureToPtr(newNode, Head, false);
                Tail = Head;
            }
            else
            {
                DoublyLinkedListNode oldTail = Marshal.PtrToStructure<DoublyLinkedListNode>(Tail);
                IntPtr newNodePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));
                oldTail.Next = newNodePtr;
                newNode.Previous = Tail;
                Marshal.StructureToPtr(oldTail, Tail, false);
                Marshal.StructureToPtr(newNode, newNodePtr, false);
                Tail = newNodePtr;
            }
            Count++;
        }

        public void RemoveFirst()
        {
            if (Count == 0)
            {
                return;
            }

            if (Head == Tail)
            {
                Head = IntPtr.Zero;
                Tail = IntPtr.Zero;
                return;
            }

            DoublyLinkedListNode oldHead = Marshal.PtrToStructure<DoublyLinkedListNode>(Head);
            DoublyLinkedListNode oldHeadNext = Marshal.PtrToStructure<DoublyLinkedListNode>(oldHead.Next);

            Head = oldHead.Next;
            oldHeadNext.Previous = IntPtr.Zero;

            Marshal.StructureToPtr(oldHeadNext, oldHead.Next, false);
        }

        public void RemoveLast()
        {
            if (Count == 0)
            {
                return;
            }

            if (Head == Tail)
            {
                Head = IntPtr.Zero;
                Tail = IntPtr.Zero;
                return;
            }

            DoublyLinkedListNode oldTail = Marshal.PtrToStructure<DoublyLinkedListNode>(Tail);
            DoublyLinkedListNode oldTailPrev = Marshal.PtrToStructure<DoublyLinkedListNode>(oldTail.Previous);

            Tail = oldTail.Previous;
            oldTailPrev.Next = IntPtr.Zero;

            Marshal.StructureToPtr(oldTailPrev, oldTail.Previous, false);
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
                for (int i = 0; i < index; i++)
                {
                    currentPtr = current.Next;
                    current = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Next);
                }

                // создаем указатель на новую ноду 
                IntPtr newNodePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));

                // следующая нода для новой ноды указывает на текущую ноду 
                newNode.Next = currentPtr;

                // предыдущая нода для новой ноды указывает на предыдущую ноду текущей ноды 
                newNode.Previous = current.Previous;

                // привязываем новую ноду к указателю
                Marshal.StructureToPtr(newNode, newNodePtr, false);

                // получаем предыдущую ноду текущей ноды 
                DoublyLinkedListNode currentPrevious = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Previous);

                // следующая нода для предыдущей ноды текущей ноды указывает на текущую ноду 
                currentPrevious.Next = newNodePtr;
                Marshal.StructureToPtr(currentPrevious, current.Previous, false);

                // предыдущая нода для текущей ноды указывает на новую ноду 
                current.Previous = newNodePtr;
                Marshal.StructureToPtr(current, currentPtr, false);
            }
            Count++;
        }

        public override readonly string ToString()
        {
            if (Count == 0)
            {
                return string.Empty;
            }

            string result = string.Empty;

            DoublyLinkedListNode current = Marshal.PtrToStructure<DoublyLinkedListNode>(Head);
            result += current.ToString() + Environment.NewLine;
            
            while (current.Next != IntPtr.Zero)
            {
                current = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Next);
                result += current.ToString() + Environment.NewLine;
            }

            return result;
        }
    }
    public struct DoublyLinkedListNode
    {
        public IntPtr Next;
        public IntPtr Previous;
        public Person Data;

        public override readonly string ToString()
        {
            string result = string.Empty;

            if (Previous != IntPtr.Zero)
            {
                DoublyLinkedListNode prevData = Marshal.PtrToStructure<DoublyLinkedListNode>(Previous);
                result += $"{prevData.Data.Name} <- ";
            }
            else
            {
                result += $"empty <- ";
            }

            result += $"{Data.Name}";

            if (Next != IntPtr.Zero)
            {
                DoublyLinkedListNode nextData = Marshal.PtrToStructure<DoublyLinkedListNode>(Next);
                result += $" -> {nextData.Data.Name}";
            }
            else
            {
                result += $" -> empty";
            }

            return result;
        }
    }
}