using DataStream.Exceptions;

namespace DataStream.Configuration
{
    public class DataStreamPaginationOptions
    {
        private int _defaultPage = 1;
        private int _defaultPageSize = 10;
        private int _defaultPageSizeLimit = 50;

        /// <summary>
        /// Default instance with predefined settings.
        /// </summary>
        public static DataStreamPaginationOptions Default => new();

        /// <summary>
        /// Default page number for pagination (must be greater than 0).
        /// </summary>
        public int DefaultPage
        {
            get => _defaultPage;
            set
            {
                if (value <= 0)
                    throw new ArgumentExceptionDS("Default page number must be greater than zero.");
                _defaultPage = value;
            }
        }

        /// <summary>
        /// Default number of rows per page (must be greater than 0 and less than or equal to DefaultPageSizeLimit).
        /// </summary>
        public int DefaultPageSize
        {
            get => _defaultPageSize;
            set
            {
                if (value <= 0)
                    throw new ArgumentExceptionDS("Default page size must be greater than zero.");

                if (value > _defaultPageSizeLimit)
                    throw new ArgumentExceptionDS($"Default page size ({value}) cannot exceed page size limit ({_defaultPageSizeLimit}).");

                _defaultPageSize = value;
            }
        }

        /// <summary>
        /// Maximum allowed rows per page (must be greater than 0).
        /// </summary>
        public int DefaultPageSizeLimit
        {
            get => _defaultPageSizeLimit;
            set
            {
                if (value <= 0)
                    throw new ArgumentExceptionDS("Page size limit must be greater than zero.");

                if (_defaultPageSize > value)
                    throw new ArgumentExceptionDS($"Current default page size ({_defaultPageSize}) cannot exceed new page size limit ({value}).");

                _defaultPageSizeLimit = value;
            }
        }
    }
}