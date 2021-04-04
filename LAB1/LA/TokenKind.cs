namespace LAB1
{
    public enum TokenKind // Тип токена.
    {
        FirstWord,     // Число.
        SecondWord, // Идентификатор.
        EndOfText,  // Конец текста.
        Unknown,     // Неизвестный.

        SqrLeftParen,
        SqrRightParen,
        Comma
    };
}
