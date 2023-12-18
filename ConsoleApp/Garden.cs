﻿using ConsoleApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Garden
    {
        public int Size { get; }
        private ICollection<string> Items { get; }

        public Garden(int size)
        {
            if(size < 0 || size > 10)
                throw new ArgumentOutOfRangeException(nameof(size));
            Size = size;
            Items = new List<string>();
        }

        public bool Plant(string name)
        {
            ValidatePlantName(name);

            if (Items.Count() >= Size)
                return false;

            if (Items.Contains(name))
            {
                name = name + (Items.Count(x => x.StartsWith(name)) + 1);
            }

            Items.Add(name);

            return true;
        }

        private static void ValidatePlantName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(Resources.PlantNeedsName, nameof(name));
        }

        public ICollection<string> GetPlants()
        {
            return Items.ToList();
        }
    }
}
