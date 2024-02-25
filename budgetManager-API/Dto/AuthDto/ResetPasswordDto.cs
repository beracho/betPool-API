namespace budgetManager.Dto.AuthDto
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = default!;
        public string RecoveryKey { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
