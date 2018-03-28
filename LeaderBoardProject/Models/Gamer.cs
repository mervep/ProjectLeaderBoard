using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaderBoardProject.Models
{
    public class Gamer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("userid")]
        public int UserId { get; set; }

        [BsonElement("username")]
        public string UserName { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("puan")]
        public int Score { get; set; }

        [BsonElement("date")]
        public string ScoreDate { get; set; }
        
        [BsonElement("isActive")]
        public bool isActive { get; set; }

        [NotMapped]
        public bool currentUser { get; set; }
    }
}