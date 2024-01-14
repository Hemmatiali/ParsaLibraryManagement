namespace ParsaLibraryManagement.Application.Utilities;

public static class EmailCleaner
{
    public static string CleanEmail(this string email)
    {
        // Check if the email is not null before cleaning
        return email != null ?
            // Convert to lowercase and trim
            email.ToLower().Trim() : null;
    }
}