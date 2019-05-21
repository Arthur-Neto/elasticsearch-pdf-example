namespace WebApplication.Elastic
{
    public class ElasticConnectionSettings
    {
        public string ClusterUrl { get; set; }

        public string DefaultIndex
        {
            get
            {
                return this.defaultIndex;
            }
            set
            {
                this.defaultIndex = value.ToLower();
            }
        }

        private string defaultIndex;
    }
}
