using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace EFnetVisitations.Entities
{
    internal class Debouncing
    {
        string text;
        public bool isRelevant = false;
        public Debouncing(string text) 
        {
            this.text = text;
            waitingTime();
        }
        public async void TextRelevant(string newtext)
        {
            var isTextRelevant = text == newtext;
            if (isTextRelevant) isRelevant = true;
            else isRelevant = false;
        }
        public async void waitingTime()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(500));
        }
    }
}
