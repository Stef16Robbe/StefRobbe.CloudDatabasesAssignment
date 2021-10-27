using System;

namespace Domain
{
    public class UserInfo
    {
        public Guid id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public float YearlyIncome { get; set; }
        public string BlobId { get; set; }
    }
}