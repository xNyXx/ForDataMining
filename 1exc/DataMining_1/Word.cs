using System.ComponentModel.DataAnnotations;

namespace DataMining_1
{
    public class Word
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string Text { get; set; }
    }
}