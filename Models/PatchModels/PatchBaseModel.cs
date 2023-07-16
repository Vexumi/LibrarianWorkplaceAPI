namespace LibrarianWorkplaceAPI.Models.PatchModels
{
    public abstract class PatchBaseModel
    {
        private HashSet<string> PropertiesInHttpRequest { get; set; }
            = new HashSet<string>();

        /// <summary>
        /// Returns true if property was present in http request; false otherwise 
        /// </summary>
        public bool IsFieldPresent(string propertyName)
        {
            return PropertiesInHttpRequest.Contains(propertyName.ToLowerInvariant());
        }

        public void SetHasProperty(string propertyName)
        {
            PropertiesInHttpRequest.Add(propertyName.ToLowerInvariant());
        }
    }
}
