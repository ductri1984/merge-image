using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData
{
    public class Case
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int RowStart { get; set; }
        public int RowEnd { get; set; }
        public List<CaseOrder> ListOrder { get; set; }
        public List<CaseMaster> ListMaster { get; set; }
    }

    public class CaseOrder
    {
        public string Code { get; set; }
        public string ID { get; set; }
        public string OPSContainerID { get; set; }
    }

    public class CaseMaster
    {
        public string Code { get; set; }
        public string ID { get; set; }
        public string VehicleID { get; set; }
        public string RomoocID { get; set; }
        public int RowStart { get; set; }
        public int ColStart { get; set; }
        public int ColEnd { get; set; }
        public int StateColStart { get; set; }
        public int StateColEnd { get; set; }
        public List<CaseMasterLocation> ListLocation { get; set; }
        public List<CaseMasterOrder> ListOrder { get; set; }
    }

    public class CaseMasterLocation
    {
        public string Code { get; set; }
        public string ID { get; set; }
        public string TypeCode { get; set; }
        public string TypeID { get; set; }
        public string SortOrder { get; set; }
        public string SortPrev { get; set; }
        public string SortReal { get; set; }
        public bool? IsBreakmooc { get; set; }
    }

    public class CaseMasterOrder
    {
        public string OrderCode { get; set; }
        public string OrderID { get; set; }
        public string OPSContainerID { get; set; }
        public string LocationCode { get; set; }
        public string LocationID { get; set; }
        public string SortOrder { get; set; }
    }


    public class PMCollection
    {
        public PMCollection_Info info { get; set; }
        public List<PMCollection_Item> item { get; set; }
    }

    public class PMCollection_Info
    {
        public string _postman_id { get; set; }
        public string name { get; set; }
        public string schema { get; set; }
    }

    public class PMCollection_Item
    {
        public string name { get; set; }
        [Newtonsoft.Json.JsonProperty("event")]
        public List<PMCollection_Event> _event { get; set; }
        public PMCollection_Request request { get; set; }
        public List<string> response { get; set; }
    }

    public class PMCollection_Event
    {
        public string listen { get; set; }
        public PMCollection_EventScript script { get; set; }
    }

    public class PMCollection_EventScript
    {
        public string id { get; set; }
        public string type { get; set; }
        public List<string> exec { get; set; }
    }

    public class PMCollection_Request
    {
        public string method { get; set; }
        public PMCollection_RequestAuth auth { get; set; }
        public List<PMCollection_RequestHeader> header { get; set; }
        public PMCollection_RequestBody body { get; set; }
        public PMCollection_RequestURL url { get; set; }
    }

    public class PMCollection_RequestAuth
    {
        //bearer, basic
        public string type { get; set; }
        //key token
        public List<PMCollection_RequestAuthData> bearer { get; set; }
        //key username, password
        public List<PMCollection_RequestAuthData> basic { get; set; }
    }

    public class PMCollection_RequestAuthData
    {
        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }

    public class PMCollection_RequestHeader
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class PMCollection_RequestBody
    {
        public string mode { get; set; }
        public string raw { get; set; }
        public List<PMCollection_RequestBodyData> urlencoded { get; set; }
    }

    public class PMCollection_RequestBodyData
    {
        public string key { get; set; }
        public string value { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

    public class PMCollection_RequestURL
    {
        public string raw { get; set; }
        public string protocol { get; set; }
        public string port { get; set; }
        public List<string> host { get; set; }
        public List<string> path { get; set; }
        public List<PMCollection_RequestQuery> query { get; set; }
    }

    public class PMCollection_RequestQuery
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
