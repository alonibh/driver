using Driver.API.Dbo;

namespace Driver.API
{
    public class GetPersonRequest
    {
        public string Username { get; set; }
    }

    public class GetPersonResponse
    {
        public PersonDbo Person { get; set; }
    }
}