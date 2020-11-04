namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public class AuthOptions
    {
        public bool AuthHeaderCheckEnabled { get; }

        public AuthOptions() : this(false)
        {

        }

        public AuthOptions(bool devMode)
        {
            AuthHeaderCheckEnabled = !devMode;
        }

    }
}
