// AccountDtos.cs
namespace UniStay.API.Data.Models.Dto.Account
{
    public class SetSecurityAnswersDto
    {
        public string Email { get; set; } = string.Empty;
        public List<UserAnswerDto> Answers { get; set; } = new();
    }

    public class UserAnswerDto
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; } = string.Empty;
    }

    public class SecurityQuestionsResponseDto
    {
        public int QuestionId { get; set; }
        public string Question { get; set; } = string.Empty;
    }

    public class VerifyAnswersDto
    {
        public string Email { get; set; } = string.Empty;
        public List<UserAnswerDto> Answers { get; set; } = new();
    }

    public class VerifyResultDto
    {
        public bool Success { get; set; }
        public string? ResetToken { get; set; } // optional - if you want to return it
    }

    public class PasswordResetDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}