using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Нажмите Enter, чтобы заполнить список");

            Console.ReadLine();

            DoublyLinkedList list = new();

            list.Add(new Plain
            {
                ID = Guid.NewGuid(),
                Type = "пассажирский",
                Model = "Самолёт150",
                Capacity = 150f,
                FlightLength = 1000f
            });
            Console.WriteLine(list);

            list.Add(new Plain
            {
                ID = Guid.NewGuid(),
                Type = "транспортный",
                Model = "Самолёт100",
                Capacity = 100f,
                FlightLength = 1000f
            });
            Console.WriteLine(list);

            list.Add(new Plain
            {
                ID = Guid.NewGuid(),
                Type = "транспортный",
                Model = "Самолёт80_1",
                Capacity = 80f,
                FlightLength = 1000f
            });
            Console.WriteLine(list);

            list.Add(new Plain
            {
                ID = Guid.NewGuid(),
                Type = "транспортный",
                Model = "Самолёт120",
                Capacity = 120f,
                FlightLength = 1000f
            });
            Console.WriteLine(list);

            list.Add(new Plain
            {
                ID = Guid.NewGuid(),
                Type = "транспортный",
                Model = "Самолёт80_2",
                Capacity = 80f,
                FlightLength = 1000f
            });
            Console.WriteLine(list);

            //list.Add(new Plain
            //{
            //    ID = Guid.NewGuid(),
            //    Type = "транспортный",
            //    Model = "Самолёт160",
            //    Capacity = 160f,
            //    FlightLength = 1000f
            //});
            //Console.WriteLine(list);

            Console.WriteLine("Нажмите Enter, чтобы вывести список");
            Console.ReadLine();

            Console.WriteLine(list);

            Console.ReadLine();
        }

        public struct Plain
        {
            public Guid ID;
            public string Type;
            public string Model;
            public float Capacity;
            public float FlightLength;
        }

        public struct DoublyLinkedList
        {
            public IntPtr Head;
            public IntPtr Tail;

            public int Count;

            public void Add(Plain p)
            {
                DoublyLinkedListNode newNode = new()
                {
                    Data = p
                };
                IntPtr newNodePtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(newNode));

                if (Count == 0)
                {
                    Head = newNodePtr;
                    Marshal.StructureToPtr(newNode, Head, false);
                    Tail = Head;
                }
                else
                {
                    DoublyLinkedListNode current = Marshal.PtrToStructure<DoublyLinkedListNode>(Head);
                    IntPtr currentPtr = Head;

                    while (current.Next != IntPtr.Zero)
                    {
                        if (current.Data.Capacity > p.Capacity)
                        {
                            currentPtr = current.Next;
                            current = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Next);
                        }
                        else
                        {
                            currentPtr = current.Previous;
                            current = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Previous);
                            break;
                        }
                    }

                    Console.WriteLine($"{current.Data.Model} граничный элем");

                    newNode.Previous = currentPtr;
                    newNode.Next = current.Next;

                    if (current.Next != IntPtr.Zero)
                    {
                        DoublyLinkedListNode currentNext = Marshal.PtrToStructure<DoublyLinkedListNode>(current.Next);
                        currentNext.Previous = newNodePtr;
                        Marshal.StructureToPtr(currentNext, current.Next, false);
                    }

                    current.Next = newNodePtr;

                    Marshal.StructureToPtr(newNode, newNodePtr, false);
                    Marshal.StructureToPtr(current, currentPtr, false);

                }

                Count++;


                Console.WriteLine($"{p.Model} добавлен");

                if (newNode.Previous != IntPtr.Zero)
                {
                    DoublyLinkedListNode prev = Marshal.PtrToStructure<DoublyLinkedListNode>(newNode.Previous);
                    Console.WriteLine($"{prev.Data.Model} пред");
                }
                if (newNode.Next != IntPtr.Zero)
                {
                    DoublyLinkedListNode next = Marshal.PtrToStructure<DoublyLinkedListNode>(newNode.Next);
                    Console.WriteLine($"{next.Data.Model} след");
                }
            }


            public override readonly string ToString()
            {
                if (Count == 0)
                {
                    return "Список пуст";
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

            public Plain Data;

            public override readonly string ToString()
            {
                string result = string.Empty;

                if (Previous != IntPtr.Zero)
                {
                    DoublyLinkedListNode prevData = Marshal.PtrToStructure<DoublyLinkedListNode>(Previous);
                    result += $"{prevData.Data.Model} <- ";
                }
                else
                {
                    result += $"no prev <- ";
                }

                result += $"{Data.Model}";

                if (Next != IntPtr.Zero)
                {
                    DoublyLinkedListNode nextData = Marshal.PtrToStructure<DoublyLinkedListNode>(Next);
                    result += $" -> {nextData.Data.Model}";
                }
                else
                {
                    result += $" -> no next";
                }

                return result;
            }
        }


    }
}
