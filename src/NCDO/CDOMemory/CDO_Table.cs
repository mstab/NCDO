﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;

namespace NCDO.CDOMemory
{
    public class CDO_Table<T> : JsonArray, IList<T> where T : CDO_Record, new()
    {
        protected List<T> _list;

        public CDO_Table(params T[] items)
        {
            _list = new List<T>();
            AddRange(items);
        }

        public CDO_Table(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _list = new List<T>(items);
        }

        public CDO_Table(IEnumerable<JsonObject> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            _list = new List<T>(items.Select(i =>
            {
                var record = new T();
                record.AddRange(i);
                return record;
            }));
        }

        public override JsonType JsonType => JsonType.Array;

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _list.Add(item);
        }

        public new void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            var itemId = item.GetId();
            return _list.Any(i => i.GetId() == itemId);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public override int Count => _list.Count;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        public int IndexOf(T item)
        {
            var itemId = item.GetId();
            return _list.FindIndex(i => i.GetId() == itemId);
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }

        public bool IsReadOnly => false;

        public new T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public bool Remove(T item) => _list.Remove(item);

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            _list.AddRange(items);
        }

        public void AddRange(params T[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            _list.AddRange(items);
        }

        public override void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            stream.WriteByte((byte)'[');

            for (var i = 0; i < _list.Count; i++)
            {
                JsonValue v = _list[i];
                if (v != null)
                {
                    v.Save(stream);
                }
                else
                {
                    stream.WriteByte((byte)'n');
                    stream.WriteByte((byte)'u');
                    stream.WriteByte((byte)'l');
                    stream.WriteByte((byte)'l');
                }

                if (i < Count - 1)
                {
                    stream.WriteByte((byte)',');
                    stream.WriteByte((byte)' ');
                }
            }

            stream.WriteByte((byte)']');
        }
    }
}