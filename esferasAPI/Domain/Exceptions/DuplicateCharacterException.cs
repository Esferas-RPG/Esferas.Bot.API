using System;

namespace apiEsferas.Domain.Exceptions
{
    public class DuplicateCharacterException : Exception
    {
        public DuplicateCharacterException(string characterName)
            :base($"A spreadsheet with the name '{characterName}' already exists")
        {
        }
    }
}
