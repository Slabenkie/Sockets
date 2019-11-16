using System;
using System.Collections.Generic;


namespace Client
{
    class PhraseCollector
    {
        private List<string> _phrases = new List<string> {
            "How is it going?",
            "What are you up to?",
            "What have you been up to?",
            "Long time no see!",
            "How's life?",
            "Good luck!",
            "Have a nice day!",
            "Take care!",
            "Until we meet again!",
            "Say hi",
            "See you soon!",
            "Do you understand me?"
        };


        public string GetPhrase()
        {
            if (_phrases == null)
            {
                return null;
            }
            else
            {
                Random rnd = new Random();
                var result = rnd.Next(0, _phrases.Count);
                var phrase = _phrases[result];
                _phrases.Remove(phrase);

                if (phrase == "Do you understand me?")
                    _phrases = null;

                return phrase;
            }
        }
    }
}
