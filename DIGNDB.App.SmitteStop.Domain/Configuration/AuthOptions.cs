namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public class AuthOptions
    {
        public bool MobileAuthHeaderCheckEnabled { get; }

        public AuthOptions() : this(false)
        {

        }

        public AuthOptions(bool devMode)
        {
            MobileAuthHeaderCheckEnabled = !devMode;
        }

    }
}
