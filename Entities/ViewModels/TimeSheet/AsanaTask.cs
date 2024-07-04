namespace Entities.ViewModels.TimeSheet
{
    public class AsanaTask
    {
        public string Gid { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Html_Notes { get; set; }
        public string Permalink_Url { get; set; }
    }

    public class AsanaResponse
    {
        public List<AsanaTask> Data { get; set; }
    }
}
