using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LeaderBoardProject.Models;

namespace LeaderBoardProject.Controllers
{
    public class HomeController : Controller
    {
        public IMongoCollection<Gamer> mongoCollection;
        public int currentUserId = 10;

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var returnList = GetGamerList(currentUserId);
            return View(returnList);
        }

        public List<Gamer> GetGamerList(int paramCurrentUserId)
        {
            paramCurrentUserId = currentUserId;
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("mygamers");
            mongoCollection = db.GetCollection<Gamer>("gamers");
            var list = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).Take(100).ToList();
            var currentUserInList = list.Where(x => x.UserId == currentUserId).Count();
            var currentGamer = mongoCollection.AsQueryable<Gamer>().Where(x => x.UserId == currentUserId).FirstOrDefault();
            currentGamer.currentUser = true;
            //data db de tutulsa
            //var filter = Builders<Gamer>.Filter.Where(x => x.UserId == currentUserId);
            //var update = Builders<Gamer>.Update.Set(x=>x.currentUser, true);
            //var options = new FindOneAndUpdateOptions<Gamer>();
            //mongoCollection.FindOneAndUpdate(filter, update, options);
            if (currentUserInList == 0)
            {
                var nextFirstGamer = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).ToList().SkipWhile(x => x.UserId != currentUserId).Skip(1).FirstOrDefault();
                var nextSecondGamer = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).ToList().SkipWhile(x => x.UserId != nextFirstGamer.UserId).Skip(1).FirstOrDefault();
                var prevFirsGamer = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).ToList().TakeWhile(x => x.UserId != currentUserId).Last();
                var prevSecondGamer = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).ToList().TakeWhile(x => x.UserId != prevFirsGamer.UserId).Last();
                var prevThirdGamer = mongoCollection.AsQueryable<Gamer>().OrderByDescending(x => x.Score).ToList().TakeWhile(x => x.UserId != prevSecondGamer.UserId).Last();
                list.Add(prevFirsGamer);
                list.Add(prevSecondGamer);
                list.Add(prevThirdGamer);
                list.Add(currentGamer);
                list.Add(nextFirstGamer);
                list.Add(nextSecondGamer);
            }
            return list;
        }
    }
}
