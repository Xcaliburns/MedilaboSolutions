using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientNotes.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonElement("PatientId")]
        public int PatientId { get; set; }

        [BsonElement("PatientNote")]
        public string PatientNote { get; set; }
    }
}
