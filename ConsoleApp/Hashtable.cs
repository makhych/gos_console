using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{

    /// <summary>
    /// Элемент данных хеш таблицы.
    /// </summary>
    public class Item
    {
        public string Key { get; private set; }
        public string Value { get; private set; }


        public Item(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Key;
        }
    }


    public class Hashtable
    {
        private readonly byte _maxSize = 255;

        //словарь, ключ которого представляет собой хеш ключа хранимых данных,
        // а значение это список элементов с одинаковым хешем ключа.
        private Dictionary<int, List<Item>> _items = null;

        // Коллекция хранимых данных в хеш-таблице в виде пар Хеш-Значения.
        public IReadOnlyCollection<KeyValuePair<int, List<Item>>> Items => _items?.ToList()?.AsReadOnly();

        public Hashtable()
        {
            _items = new Dictionary<int, List<Item>>(_maxSize);
        }


        public void Insert(string key, string value)
        {

            if (key.Length > _maxSize)
            {
                throw new ArgumentException($"Максимальная длинна ключа составляет {_maxSize} символов.", nameof(key));
            }

            // Создаем новый экземпляр данных.
            var item = new Item(key, value);

            var hash = GetHash(item.Key);

            // Получаем коллекцию элементов с таким же хешем ключа.
            // Если коллекция не пустая, значит заначения с таким хешем уже существуют,
            // следовательно добавляем элемент в существующую коллекцию.
            // Иначе коллекция пустая, значит значений с таким хешем ключа ранее не было,
            // следовательно создаем новую пустую коллекцию и добавляем данные.
            List<Item> hashTableItem = null;
            if (_items.ContainsKey(hash))
            {
                // Получаем элемент хеш таблицы.
                hashTableItem = _items[hash];

                // Проверяем наличие внутри коллекции значения с полученным ключом.
                // Если такой элемент найден, то сообщаем об ошибке.
                var oldElementWithKey = hashTableItem.SingleOrDefault(i => i.Key == item.Key);
                if (oldElementWithKey != null)
                {
                    throw new ArgumentException($"Хеш-таблица уже содержит элемент с ключом {key}. Ключ должен быть уникален.", nameof(key));
                }

                _items[hash].Add(item);
            }
            else
            {
                hashTableItem = new List<Item> { item };

                // Добавляем данные в таблицу.
                _items.Add(hash, hashTableItem);
            }
        }

        public void Delete(string key)
        {
            // Получаем хеш ключа.
            var hash = GetHash(key);

            if (!_items.ContainsKey(hash))
            {
                return;
            }

            // Получаем коллекцию элементов по хешу ключа.
            var hashTableItem = _items[hash];

            // Получаем элемент коллекции по ключу.
            var item = hashTableItem.SingleOrDefault(i => i.Key == key);

            // Если элемент коллекции найден, 
            // то удаляем его из коллекции.
            if (item != null)
            {
                hashTableItem.Remove(item);
            }
        }

        public string Search(string key)
        {
            var hash = GetHash(key);

            if (!_items.ContainsKey(hash))
            {
                return null;
            }

            var hashTableItem = _items[hash];

            if (hashTableItem != null)
            {
                var item = hashTableItem.SingleOrDefault(i => i.Key == key);

                if (item != null)
                {
                    return item.Value;
                }
            }

            return null;
        }


        private int GetHash(string value)
        {
            // Получаем длину строки.
            var hash = value.Length;
            return hash;
        }

    }
}
