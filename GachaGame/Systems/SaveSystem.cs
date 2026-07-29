using GachaGame.Models;
using System.IO;
using System.Text.Json;

namespace GachaGame .Systems
{
    public class SaveSystem
    {
        private const string SaveFile = "player.json";


        public void Save(PlayerData data)
        {
            string json = JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            File.WriteAllText(SaveFile, json);
        }


        public PlayerData Load()
        {
            if (!File.Exists(SaveFile))
            {
                return new PlayerData
                {
                    MoonTears = 1600
                };
            }


            string json = File.ReadAllText(SaveFile);

            return JsonSerializer.Deserialize<PlayerData>(json);
        }

        public void ResetSave()
        {
            PlayerData newPlayer = new PlayerData();

            Save(newPlayer);
        }
    }
}