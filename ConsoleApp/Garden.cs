using ConsoleApp.Properties;
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
        private ILogger Logger { get; }

        public Garden(int size, ILogger logger) : this(size)
        {
            Logger = logger;
        }

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
            {
                Logger?.Log($"Brak miejsca w ogrodzie na {name}");
                return false;
            }

            if (Items.Contains(name))
            {
                var newName = name + (Items.Count(x => x.StartsWith(name)) + 1);

                Logger?.Log($"Zmiana nazwy z {name} na {newName}");
                name = newName;
            }

            Items.Add(name);
            Logger?.Log($"Roślina {name} została dodana do ogrodu");

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

        public bool Remove(string name)
        {
            if (!Items.Contains(name))
                return false;
            Items.Remove(name);
            return true;
        }

        public void Clear()
        {
            Items.Clear();
        }

        public int Count()
        {
            return Items.Count();
        }

        public string GetLastLogFromLastHour()
        {
            var datetime = DateTime.Now;
            var log = Logger.GetLogsAsync(datetime.AddHours(-1), datetime).Result;
            return log.Split("\n").Last();
        }
    }
}
