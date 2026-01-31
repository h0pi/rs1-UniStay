// UserSecurityAnswer.cs
using UniStay.API.Data.Models;

public class UserSecurityAnswer
{
    public int UserSecurityAnswerID { get; set; }
    public int UserID { get; set; }
    public Users User { get; set; } = null!;
    public int SecurityQuestionID { get; set; }
    public SecurityQuestion SecurityQuestion { get; set; } = null!;

    // store hashed answer
    public string AnswerHash { get; set; } = null!;
}