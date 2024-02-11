namespace budgetManager.Dto.AuthDto
{
    public class LoginValidation
    {
        public string Message { get; set; } = default!;
        public int AttemptsLeft { get; set; } = default!;
    }
}
