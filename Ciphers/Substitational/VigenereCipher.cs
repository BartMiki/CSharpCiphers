using Ciphers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ciphers.Substitational
{
    public class VigenereCipher : ICipher
    {
        public string Keyword { get; }

        public static List<string> VigenereSquare { get; }
        public static Dictionary<char, int> LetterIndex { get; }
        public static Dictionary<int, char> IndexLetter { get; }

        static VigenereCipher()
        {
            int asciiBegin = 32;
            int asciiEnd = 126 - asciiBegin + 1;

            IndexLetter = new Dictionary<int, char>(asciiEnd);
            LetterIndex = new Dictionary<char, int>(asciiEnd);
            VigenereSquare = new List<string>(asciiEnd);

            var letterIndexes = Enumerable.Range(asciiBegin, asciiEnd).Select(x => (letter: (char)x, index: x - asciiBegin));

            foreach(var (letter, index) in letterIndexes)
            {
                IndexLetter[index] = letter;
                LetterIndex[letter] = index;
            }

            int i = 0;
            var alphabet = letterIndexes.Select(x => x.letter);
            var lettersCount = alphabet.Count();
            while (i < lettersCount)
            {
                VigenereSquare.Add(string.Join("",alphabet));
                var head = alphabet.Take(1);
                var tail = alphabet.Skip(1);
                alphabet = tail.Concat(head);

                i++;
            }
        }

        public VigenereCipher(string keyword)
        {
            if(string.IsNullOrWhiteSpace(keyword))
            {
                throw new Exception("Klucz nie może być pusty");
            }

            Keyword = keyword;
        }


        public string Decrypt(string encrypted)
        {
            var tokens = encrypted.Split(new[] { Environment.NewLine, "\t"},StringSplitOptions.RemoveEmptyEntries);
            string key = RepeatKey(encrypted.Length);
            var builder = new StringBuilder();
            for (int i = 0; i < encrypted.Length; i++)
            {
                int row = LetterIndex[key[i]];
                int column = VigenereSquare[row].IndexOf(encrypted[i]);

                builder.Append(IndexLetter[column]);
            }
            return builder.ToString();
        }

        public string Encrypt(string message)
        {
            string key = RepeatKey(message.Length);
            var builder = new StringBuilder();
            for(int i = 0; i<message.Length; i++)
            {
                int row = LetterIndex[message[i]];
                int column = LetterIndex[key[i]];

                builder.Append(VigenereSquare[row][column]);
            }
            return builder.ToString();
        }

        private string RepeatKey(int textLength)
        {
            string key = Keyword;
            while(key.Length < textLength)
            {
                key += Keyword;
            }
            return key;
        }
    }
}
