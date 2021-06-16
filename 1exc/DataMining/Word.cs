using System.ComponentModel.DataAnnotations;

namespace DataMining
{
    public class Word
    {
        public int Id { get; set; }
        public int Count { get; set; } 
        public string Text { get; set; }
    }
}