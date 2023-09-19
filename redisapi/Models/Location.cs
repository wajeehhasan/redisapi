using AutoMapper;
using LOGIC.Model;

namespace redisapi.Models
{
    [AutoMap(typeof(LocationLOGIC))]
    public class Location
    {
        public string city { get; set; }
    }
}
