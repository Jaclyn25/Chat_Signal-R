namespace Chat_Website.ViewModel.Account
{
    public class registerViewModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
