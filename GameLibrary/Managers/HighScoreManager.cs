using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using GameLibrary.Entities;
using System.Security.Cryptography.X509Certificates;

namespace GameLibrary.Managers
{
    public class HighScoreManager
    {
        private const string FileName = "highscores.txt";
        private const int MaxEntries = 10;
        private readonly List<Entities.HighScore> entries = new List<HighScore>();
        public IReadOnlyList<HighScore> Entries => entries.AsReadOnly();

        public async Task LoadAsync()
        {
            entries.Clear();
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.TryGetItemAsync(FileName) as StorageFile;
                if (file == null) return;

                var text = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrWhiteSpace(text)) return;

                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach(var line in lines)
                {
                    var parts = line.Split(new[] { '|' }, 2);
                    if (parts.Length != 2) continue;
                    int score;
                    if (!int.TryParse(parts[1], out score)) continue;
                    entries.Add(new HighScore { Name = parts[0], Score = score });
                }

                entries.Sort((a, b) => b.Score.CompareTo(a.Score));

            }
            catch
            {
                entries.Clear();
            }
        }

        // Save high scores to local storage
        public async Task SaveAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            var lines = entries.Select(e => $"{e.Name}|{e.Score}");
            await FileIO.WriteTextAsync(file, string.Join(Environment.NewLine, lines));
        }

        // Add a new high score entry
        public bool AddScore(string name, int score)
        {
            if (string.IsNullOrWhiteSpace(name)) name = "Player";
            var entry =  new HighScore { Name = name.Trim(), Score = score };
            entries.Add(entry);
            entries.Sort((a, b) => b.Score.CompareTo(a.Score));
            if (entries.Count > MaxEntries)
            {
                entries.RemoveRange(MaxEntries, entries.Count - MaxEntries);
            }

            bool isTop = entries.Count > 0 && entries[0] == entry;
            return isTop;
        }

        //debugging method to clear all high scores
        public void Clear() => entries.Clear();
    }
}
