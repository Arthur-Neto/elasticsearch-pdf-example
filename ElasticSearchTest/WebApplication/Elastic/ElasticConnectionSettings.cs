namespace WebApplication.Elastic
{
    public class ElasticConnectionSettings
    {
        public string ClusterUrl { get; set; }

        private string _defaultIndex;
        public string DefaultIndex
        {
            get
            {
                return _defaultIndex;
            }
            set
            {
                _defaultIndex = value.ToLower();
            }
        }
    }
}
