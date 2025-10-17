
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using WebApplication2.DAO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //private readonly Context c;
        //public HomeController(Context context)
        //{
        //    this.c = context;
        //}
        Context c = new Context();
        public IActionResult Index()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                    .Where(e => e.Username == username).FirstOrDefault();
            
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }

        [HttpPost("hesabimisil")]
        public IActionResult hesabimisil()
        {
            //User user = new User();
            string usernametobedeleted = (string)TempData["veri"];
            //user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User usertobedeleted = c.MyUsers.Include(p => p.MemberedGroups)
                .Where(p => p.Username == usernametobedeleted).FirstOrDefault();
            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(usertobedeleted);
            var topicstobedeleted = c.Topics.Where(p => p.Owner == usertobedeleted);
            var messagestobedeleted = c.Messages.Where(p => p.MessageSender == usertobedeleted
                                                            || p.MessageReceiver == usertobedeleted);
            var commentstobedeleted = c.Comments.Where(p => p.Owner == usertobedeleted);
            var fstobedeleted1 = c.Friendships.Where(p => p.Friend1 == usertobedeleted);
            var fstobedeleted2 = c.Friendships.Where(p => p.Friend2 == usertobedeleted);
            var frtobedeleted1 = c.FriendRequests.Where(p => p.Sender == usertobedeleted);
            var frtobedeleted2 = c.FriendRequests.Where(p => p.Receiver == usertobedeleted);
            if (fstobedeleted1 != null) c.Friendships.RemoveRange(fstobedeleted1);
            if (fstobedeleted2 != null) c.Friendships.RemoveRange(fstobedeleted2);
            if (frtobedeleted1 != null) c.FriendRequests.RemoveRange(frtobedeleted1);
            if (frtobedeleted2 != null) c.FriendRequests.RemoveRange(frtobedeleted2);

            List<AlagramComment> commentstobeDeleted = c.AlagramComments
                .Where(p => p.Owner == usertobedeleted).ToList();
            List<AlagramComment> allComments = c.AlagramComments.ToList();

            foreach (AlagramComment allCom in allComments)
            {
                foreach (AlagramComment comDel in commentstobeDeleted)
                {
                    if (allCom.QuotedComment == comDel)
                    {
                        allCom.QuotedComment = null;
                    }
                }
            }
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            int i = 0;
            while (i < groups.Count)
            {
                if (groups[i].BannedUsers.Contains(usertobedeleted))
                {
                    groups[i].BannedUsers.Remove(usertobedeleted);
                }
                if (usertobedeleted.MemberedGroups.Contains(groups[i]))
                {
                    usertobedeleted.MemberedGroups.Remove(groups[i]);
                }
                if (groups[i].Members.Contains(usertobedeleted))
                {
                    groups[i].Members.Remove(usertobedeleted);
                }
                if (groups[i].Owner == usertobedeleted)
                {
                    groups.Remove(groups[i]);
                }
                i++;
            }
         
            c.AlagramComments.UpdateRange(allComments);
            c.AlagramComments.RemoveRange(commentstobeDeleted);
            c.AlagramGroups.UpdateRange(groups);
            i = 0;
            while(i<users.Count)
            {
                if (usertobedeleted.BannedUsers.Contains(users[i]))
                {
                    usertobedeleted.BannedUsers.Remove(users[i]);
                }
                i++;
            }

            c.MyUsers.Update(usertobedeleted);
            
            c.MyUsers.Remove(usertobedeleted);
            c.Topics.RemoveRange(topicstobedeleted);
            c.Messages.RemoveRange(messagestobedeleted);
            c.Comments.RemoveRange(commentstobedeleted);
            c.SaveChanges();
       
            TempData["veri"] = null;
            return RedirectToAction("index");
        }

        [Route("engelikaldir")]
        public IActionResult engelikaldir(string usernametobeunbanned)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            User usertobeunbanned = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == usernametobeunbanned).FirstOrDefault();
           List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers).ToList();

            
            
            if (user.BannedUsers.Contains(usertobeunbanned))
            {
                user.BannedUsers.Remove(usertobeunbanned);

            }


            c.MyUsers.Update(user);
            c.SaveChanges();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroups = groups;
            return View("engellenenkullanicilarsay", vm);
        }

        [HttpGet("engellenenkullanicilarsay")]
        public IActionResult Engellenenkullanicilarsay()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(u => u.MemberedGroups).Include(u => u.BannedUsers).Where(p => p.Username == username).FirstOrDefault();
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);


            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View(vm);
        }

        [Route("banikaldir")]
        public IActionResult banikaldir(string groupId, string usernametobeunbanned)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            User usertobeunbanned = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == usernametobeunbanned).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();
            if (group.BannedUsers.Contains(usertobeunbanned))
            {
                group.BannedUsers.Remove(usertobeunbanned);

            }
            
            
            c.AlagramGroups.Update(group);
            c.SaveChanges();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            return View("banlananuyelersay", vm);
        }

        [Route("kullaniciyibanla")]
        public IActionResult kullaniciyibanla(string groupId,string usernametobebanned)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            User usertobebanned = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == usernametobebanned).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();
            if(group.Members.Contains(usertobebanned))
            {
                group.Members.Remove(usertobebanned);
                
            }
            if (!group.BannedUsers.Contains(usertobebanned))
            {
                group.BannedUsers.Add(usertobebanned);
            }
            List<AlagramComment> commentstobedeleted = c.AlagramComments.Where(e => e.Owner == usertobebanned).ToList();
            if(commentstobedeleted!=null)
            {
                c.AlagramComments.RemoveRange(commentstobedeleted);
            }
            c.AlagramGroups.Update(group);
            c.SaveChanges();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            return View("grupuyelerisay",vm);
        }

        [HttpGet("gruptancik")]
        public IActionResult gruptancik(string groupId)
        {
            
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();
            List<AlagramComment> commentstobeDeleted = c.AlagramComments
                .Where(p => p.Owner == user && p.Group == group).ToList();
            List<AlagramComment> allComments = c.AlagramComments.ToList();
            foreach(AlagramComment allCom in allComments)
            {
                foreach(AlagramComment comDel in commentstobeDeleted)
                {
                    if(allCom.QuotedComment==comDel)
                    {
                        allCom.QuotedComment = null;
                    }
                }
            }
            c.AlagramComments.UpdateRange(allComments);
            c.AlagramComments.RemoveRange(commentstobeDeleted);
            user.MemberedGroups.Remove(group);
            group.Members.Remove(user);
            c.MyUsers.Update(user);
            c.AlagramGroups.Update(group);
            c.SaveChanges();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();


            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramComments=allComments;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            return View("alagram",vm);
        }

        [HttpGet("gruplardaara")]
        public IActionResult gruplardaara(string gruparamaveri)
        {
            
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            List<AlagramGroup> groups = new List<AlagramGroup>();
            if(gruparamaveri==null)
            {
                //groups = c.AlagramGroups.ToList();
                groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            }
            if(gruparamaveri!=null)
            {
                groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                    .Where(p => p.Name.Contains(gruparamaveri)).ToList();
            }
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View("alagram",vm);
        }

        [Route("grupuyelerisay")]
        public IActionResult Grupuyelerisay(string groupId)
        {
            if(groupId==null) groupId = (string)TempData["groupId"];

            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            return View(vm);
        }
        //[Route("grupuyelerisay2")]
        //public IActionResult Grupuyelerisay2()
        //{
        //    string groupId = (string)TempData["groupId"];

        //    string username = (string)TempData["veri"];
        //    //User user = c.MyUsers.Find(username);
        //    User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
        //        .Where(p => p.Username == username).FirstOrDefault();
        //    //List<AlagramGroup> groups = c.AlagramGroups.ToList();
        //    List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers).ToList();

        //    //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
        //    AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers)
        //        .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

        //    ViewModel vm = new ViewModel();
        //    vm.User = user;
        //    vm.AlagramGroup = group;
        //    vm.AlagramGroups = groups;
        //    return View(vm);
        //}

        [Route("banlananuyelersay")]
        public IActionResult Banlananuyelersay(string groupId)
        {
            if (groupId == null) groupId = (string)TempData["groupId"];

            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e => e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e => e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            return View(vm);
        }

        [HttpPost("gruptaalintila")]
        public IActionResult gruptaalintila(string groupId, string commentIdtobeQuoted, string commentContent)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();

            AlagramComment quotedComment = c.AlagramComments.Find(Convert.ToInt32(commentIdtobeQuoted));
            AlagramComment comment = new AlagramComment();

            if (commentContent != null)
            {
                comment.Group = group;
                comment.Owner = user;
                comment.Content = commentContent;
                comment.QuotedComment = quotedComment;
                c.AlagramComments.Add(comment);
                c.SaveChanges();
            }

            List<AlagramComment> comments = c.AlagramComments.ToList();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View("grupsay", vm);
        }

        [HttpPost("gruptaalintilasay")]
        public IActionResult Gruptaalintilasay(string groupId, string commentIdtobeQuoted)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            AlagramComment comment = c.AlagramComments.Find(Convert.ToInt32(commentIdtobeQuoted));


            List<AlagramComment> comments = c.AlagramComments.ToList();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View(vm);
        }

        [HttpPost("gruptayorumuduzenlesay")]
        public IActionResult Gruptayorumuduzenlesay(string groupId, string commentIdtobeEdited)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            AlagramComment comment = c.AlagramComments.Find(Convert.ToInt32(commentIdtobeEdited));


            List<AlagramComment> comments = c.AlagramComments.ToList();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View(vm);
        }

        [HttpPost("gruptayorumuduzenle")]
        public IActionResult gruptayorumuduzenle(string groupId, string commentIdToBeEdited, string newcommentcontent)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AppGroup> groups = c.AppGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();


            AlagramComment comment = c.AlagramComments.Find(Convert.ToInt32(commentIdToBeEdited));
            List<AlagramComment> comments0 = c.AlagramComments.ToList();
            
            foreach (AlagramComment co in comments0)
            {
                if (co.QuotedComment == comment)
                {

                    co.QuotedComment.Content = newcommentcontent;
                    c.AlagramComments.Update(co);
                    c.SaveChanges();

                }
            }
            comment.Content = newcommentcontent;
            
            c.AlagramComments.Update(comment);
            c.SaveChanges();

            List<AlagramComment> comments = c.AlagramComments.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View("grupsay", vm);
        }

        [HttpPost("gruptayorumusil")]
        public IActionResult gruptayorumusil(string groupId, string commentIdtobeDeleted)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AppGroup> groups = c.AppGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();


            AlagramComment comment = c.AlagramComments.Find(Convert.ToInt32(commentIdtobeDeleted));
            List<AlagramComment> comments0 = c.AlagramComments.ToList();
            foreach (AlagramComment co in comments0)
            {
                if (co.QuotedComment == comment)
                {

                    co.QuotedComment = null;
                    c.AlagramComments.Update(co);
                    c.SaveChanges();

                }
            }


            c.AlagramComments.Remove(comment);
            c.SaveChanges();

            List<AlagramComment> comments = c.AlagramComments.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View("grupsay", vm);
        }

        [HttpPost("gruptayorumyap")]
        public IActionResult gruptayorumyap(string groupId, string commentContent)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();

            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            AlagramComment comment = new AlagramComment();

            if (commentContent != null)
            {
                comment.Group = group;
                comment.Owner = user;
                comment.Content = commentContent;
                c.AlagramComments.Add(comment);
                c.SaveChanges();
            }

            List<AlagramComment> comments = c.AlagramComments.ToList();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComment = comment;
            vm.AlagramComments = comments;

            return View("grupsay", vm);


        }

        [HttpGet("grupsay")]
        public IActionResult Grupsay(string groupId)
        {
            string username = (string)TempData["veri"];

            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            List<User> users= c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            if (groupId==null && TempData["groupId"] != null)
            {
                groupId = Convert.ToString((int)TempData["groupId"]);
            }
            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();

            List<AlagramComment> comments = c.AlagramComments.ToList();

            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.AlagramGroup = group;
            vm.AlagramGroups = groups;
            vm.AlagramComments = comments;
            return View(vm);
        }

        //[HttpGet("grupsay2")]
        //public IActionResult Grupsay2(string groupId)
        //{
        //    string username = (string)TempData["veri"];

        //    //User user = c.MyUsers.Find(username);
        //    User user = c.MyUsers.Include(p => p.MemberedGroups)
        //        .Where(p => p.Username == username).FirstOrDefault();

        //    List<User> users = c.MyUsers.Include(p => p.MemberedGroups).ToList();
        //    if (TempData["groupId"] != null)
        //    {
        //        groupId = Convert.ToString((int)TempData["groupId"]);
        //    }
        //    //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
        //    AlagramGroup group = c.AlagramGroups.Include(p => p.Members)
        //        .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

        //    //List<AlagramGroup> groups = c.AlagramGroups.ToList();
        //    List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();

        //    List<AlagramComment> comments = c.AlagramComments.ToList();

        //    ViewModel vm = new ViewModel();
        //    vm.User = user;
        //    vm.Users = users;
        //    vm.AlagramGroup = group;
        //    vm.AlagramGroups = groups;
        //    vm.AlagramComments = comments;
        //    return View("grupsay",vm);
        //}

        [Route("grubauyeol")]
        public IActionResult grubauyeol(string groupId)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();
            if (!group.Members.Contains(user) && !group.BannedUsers.Contains(user))
            {
                group.Members.Add(user);
                c.AlagramGroups.Update(group);
                c.SaveChanges();
            }
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View("alagram", vm);
        }

        [HttpPost("grubusil")]
        public IActionResult grubusil(string groupId)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //AlagramGroup group = c.AlagramGroups.Find(Convert.ToInt32(groupId));
            AlagramGroup group = c.AlagramGroups.Include(p => p.Members)
                .Where(p => p.Id == Convert.ToInt32(groupId)).FirstOrDefault();

            c.AlagramGroups.Remove(group);
            c.SaveChanges();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View("alagram", vm);
        }

        [HttpPost("grubukur")]
        public IActionResult grubukur(string newgroupname)
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;

            ViewBag.samegroupname = false;
            foreach (AlagramGroup group in groups)
            {
                if (group.Name == newgroupname)
                {
                    ViewBag.samegroupname = true;
                }
            }
            if (ViewBag.samegroupname == true)
            {
                return View("Yenigrupkursay", vm);
            }
            if(newgroupname!=null)
            {
                AlagramGroup g = new AlagramGroup();
                g.Name = newgroupname;
                g.Owner = user;
                g.Members.Add(user);
                c.AlagramGroups.Add(g);
                user.MemberedGroups.Add(g);
                c.MyUsers.Update(user);
                c.SaveChanges();
                vm.AlagramGroup = g;
            }
            //List<AlagramGroup> groups1 = c.AlagramGroups.ToList();
            List<AlagramGroup> groups1 = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            
            vm.AlagramGroups = groups1;

            return View("alagram", vm);
        }

        [Route("yenigrupkursay")]
        public IActionResult Yenigrupkursay()
        {
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View(vm);
        }

        [Route("gruplarimsay")]
        public IActionResult Gruplarimsay()
        {
            string username = (string)TempData["veri"];
            //User user = c.Users.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View(vm);
        }

        [Route("alagram")]
        public IActionResult Alagram()
        {
            
            string username = (string)TempData["veri"];
            //User user = c.MyUsers.Find(username);
            User user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();

            //List<AlagramGroup> groups = c.AlagramGroups.ToList();
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.AlagramGroups = groups;
            return View(vm);
        }

        [Route("mesajlarsay")]
        public IActionResult Mesajlarsay()
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            List<Message> messages = c.Messages.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            List<User> users;
            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users; 
            vm.Messages = messages;
            vm.Friendships = friendships;
            return View(vm);
        }

        [HttpPost("kullaniciyiengelle")]
        public IActionResult kullaniciyiengelle(string usernametobebanned)
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            User userToBeBanned = c.MyUsers.Where(e => e.Username == usernametobebanned).FirstOrDefault();
            user.BannedUsers.Add(userToBeBanned);
            c.MyUsers.Update(user);
            c.SaveChanges();
            List<User> users = c.MyUsers.OrderBy(x => x.Name).ToList();
            users.Remove(user);
            FriendRequest friendrequesttobedeleted1 = c.FriendRequests
                .Where(e => (e.Sender == user && e.Receiver == userToBeBanned)||((e.Sender == userToBeBanned && e.Receiver == user))).FirstOrDefault();
            //FriendRequest friendrequesttobedeleted2 = c.FriendRequests.Where(e => e.Sender == userToBeBanned && e.Receiver == user).FirstOrDefault();
            if (friendrequesttobedeleted1 != null)
            {
                c.FriendRequests.Remove(friendrequesttobedeleted1);
                //c.FriendRequests.Remove(friendrequesttobedeleted2);
                c.SaveChanges();
            }
            Friendship friendshiptobedeleted1 = c.Friendships
                .Where(e => (e.Friend1 == user && e.Friend2 == userToBeBanned)||(e.Friend1 == userToBeBanned && e.Friend2 == user))
                .FirstOrDefault();
            //Friendship friendshiptobedeleted2 = c.Friendships.Where(e => e.Friend1 == userToBeBanned && e.Friend2 == user).FirstOrDefault();
            if (friendshiptobedeleted1 != null)
            {
                c.Friendships.Remove(friendshiptobedeleted1);
                //c.Friendships.Remove(friendshiptobedeleted2);
                c.SaveChanges();
            }
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("kullanicilar",vm);

        }

            [HttpPost("kullaniciyisil")]
        public IActionResult kullaniciyisil(string usernametobedeleted)
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User usertobedeleted = c.MyUsers.Include(p => p.MemberedGroups)
                .Where(p => p.Username == usernametobedeleted).FirstOrDefault();
            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(usertobedeleted);
            var topicstobedeleted = c.Topics.Where(p => p.Owner == usertobedeleted);
            var messagestobedeleted = c.Messages.Where(p => p.MessageSender == usertobedeleted 
                                                            ||p.MessageReceiver==usertobedeleted);
            var commentstobedeleted=c.Comments.Where(p => p.Owner == usertobedeleted);
            var fstobedeleted1 = c.Friendships.Where(p => p.Friend1 == usertobedeleted);
            var fstobedeleted2 = c.Friendships.Where(p => p.Friend2 == usertobedeleted);
            var frtobedeleted1 = c.FriendRequests.Where(p => p.Sender == usertobedeleted);
            var frtobedeleted2 = c.FriendRequests.Where(p => p.Receiver == usertobedeleted);
            if (fstobedeleted1 != null) c.Friendships.RemoveRange(fstobedeleted1);
            if (fstobedeleted2 != null) c.Friendships.RemoveRange(fstobedeleted2);
            if (frtobedeleted1 != null) c.FriendRequests.RemoveRange(frtobedeleted1);
            if (frtobedeleted2 != null) c.FriendRequests.RemoveRange(frtobedeleted2);

            List<AlagramComment> commentstobeDeleted = c.AlagramComments
                .Where(p => p.Owner == usertobedeleted).ToList();
            List<AlagramComment> allComments = c.AlagramComments.ToList();

            foreach (AlagramComment allCom in allComments)
            {
                foreach (AlagramComment comDel in commentstobeDeleted)
                {
                    if (allCom.QuotedComment == comDel)
                    {
                        allCom.QuotedComment = null;
                    }
                }
            }
            List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers).ToList();
            int i = 0;
            while(i<groups.Count)
            {
                if (groups[i].BannedUsers.Contains(usertobedeleted))
                {
                    groups[i].BannedUsers.Remove(usertobedeleted);
                }
                if (usertobedeleted.MemberedGroups.Contains(groups[i]))
                {
                    usertobedeleted.MemberedGroups.Remove(groups[i]);
                }
                if (groups[i].Members.Contains(usertobedeleted))
                {
                    groups[i].Members.Remove(usertobedeleted);
                }
                if (groups[i].Owner == usertobedeleted)
                {
                    groups.Remove(groups[i]);
                }
                i++;
            }
            //foreach(AlagramGroup group in groups)
            //{
            //    if(usertobedeleted.MemberedGroups.Contains(group))
            //    {
            //        usertobedeleted.MemberedGroups.Remove(group);
            //    }
            //    if(group.Members.Contains(usertobedeleted))
            //    {
            //        group.Members.Remove(usertobedeleted);
            //    }
            //    if(group.Owner==usertobedeleted)
            //    {
            //        groups.Remove(group);
            //    }
            //}
            c.AlagramComments.UpdateRange(allComments);
            c.AlagramComments.RemoveRange(commentstobeDeleted);
            c.AlagramGroups.UpdateRange(groups);
            i = 0;
            while (i < users.Count)
            {
                if (usertobedeleted.BannedUsers.Contains(users[i]))
                {
                    usertobedeleted.BannedUsers.Remove(users[i]);
                }
                i++;
            }
            c.MyUsers.Update(usertobedeleted);
        
            c.MyUsers.Remove(usertobedeleted);
            c.Topics.RemoveRange(topicstobedeleted);
            c.Messages.RemoveRange(messagestobedeleted);
            c.Comments.RemoveRange(commentstobedeleted);
            c.SaveChanges();
            List<User> users2 = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users2.Remove(user);
            List<FriendRequest> friendrequests = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users2;
            vm.FriendRequests = friendrequests;
            vm.Friendships = friendships;
            return View("Kullanicilar",vm);
        }

        [HttpPost("alintila")]
        public IActionResult alintila(string TopicId, string content,string CommentId)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();
            Comment quot = c.Comments.Find(Convert.ToInt32(CommentId));
            Comment comment = new Comment();
            if (content != null)
            {
                comment.Content = content;
                comment.Topic = topic;
                comment.Owner = user;
                comment.QuotedComment = quot;
                c.Comments.Add(comment);
                c.SaveChanges();

            }


            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View("konusay", vm);
        }

        [HttpPost("alintilasay")]
        public IActionResult Alintilasay(string TopicId, string CommentId)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();

            Comment comment = c.Comments.Find(Convert.ToInt32(CommentId));
            

            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View(vm);
        }

        [HttpPost("yorumuduzenlesay")]
        public IActionResult Yorumuduzenlesay(string TopicId, string CommentId)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();

            Comment comment = c.Comments.Find(Convert.ToInt32(CommentId));


            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View(vm);
        }

        [HttpPost("yorumuduzenle")]
        public IActionResult yorumuduzenle(string TopicId, string CommentId, string newcontent)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();

            Comment comment = c.Comments.Find(Convert.ToInt32(CommentId));
            
            
            c.Comments.Where(p => p.QuotedComment == comment)
                    .ToList().ForEach(x => x.QuotedComment.Content=newcontent);
            comment.Content = newcontent;
            c.Comments.Update(comment);
            c.SaveChanges();

            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View("konusay", vm);
        }

        [HttpPost("yorumsil")]
        public IActionResult yorumsil(string TopicId, string CommentId)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();

            Comment comment = c.Comments.Find(Convert.ToInt32(CommentId));
            c.Comments.Where(p => p.QuotedComment == comment)
                    .ToList().ForEach(x => x.QuotedComment = null);
            c.Comments.Remove(comment);
            c.SaveChanges();

            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View("konusay", vm);
        }

        [HttpPost("yorumyap")]
        public IActionResult yorumyap(string TopicId,string content)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);

            Topic topic = c.Topics.Find(Convert.ToInt32(TopicId));
            List<Topic> topics = c.Topics.ToList();
            Comment comment = new Comment();
            if(content!=null) 
            { 
                comment.Content = content;
                comment.Topic = topic;
                comment.Owner = user;
                c.Comments.Add(comment);
                c.SaveChanges();

            }
            
           
            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comment = comment;
            vm.Comments = comments;
            return View("konusay",vm);
        }

        [HttpPost("konusay")]
        public IActionResult konusay(string Id)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e => e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);
            Topic topic = c.Topics.Find(Convert.ToInt32(Id));
            List<Topic> topics = c.Topics.ToList();
            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comments = comments;
            return View(vm);
        }

        [Route("konusay2")]
        public IActionResult konusay2()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers)
                .Include(e=>e.BanningUsers).Where(e => e.Username == username).FirstOrDefault();

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);
            int Id = (int)TempData["topicId"];
            Topic topic = c.Topics.Find(Id);
            List<Topic> topics = c.Topics.ToList();
            List<Comment> comments = c.Comments.Where(p => p.Topic == topic).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topic = topic;
            vm.Topics = topics;
            vm.Comments = comments;
            return View("Konusay",vm);
        }
        [Route("forum")]
        public IActionResult Forum()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);
            List<Topic> topics = c.Topics.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topics = topics;
            return View(vm);
        }

        [Route("konuacsay")]
        public IActionResult Konuacsay()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }

        [Route("konuac")]
        public IActionResult Konuac(string konubasligi)
        {
            ViewModel vm = new ViewModel();
            if (konubasligi==null)
            {
                User user = new User();
                string username = (string)TempData["veri"];

                user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

                List<Topic> topics = c.Topics.ToList();
                List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
                users.Remove(user);
                
                vm.User = user;
                vm.Users = users;
                
                vm.Topics = topics;
            }
            if(konubasligi!=null)
            {
                User user = new User();
                string username = (string)TempData["veri"];

                user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
                Topic topic = new Topic();
                topic.Owner = user;
                topic.Title = konubasligi;
                c.Topics.Add(topic);
                c.SaveChanges();
                List<Topic> topics = c.Topics.ToList();
                List<User> users = c.MyUsers.ToList();
                users.Remove(user);
                
                vm.User = user;
                vm.Users = users;
                vm.Topic = topic;
                vm.Topics = topics;
                
            }

            return View("forum", vm);
        }

        [Route("konulardaara")]
        public IActionResult konulardaara(string konuaramaveri)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            List<Topic> topics = new List<Topic>();

            if(konuaramaveri!=null)
            {
                topics = c.Topics.Where(p => p.Title.Contains(konuaramaveri)).ToList();
            }
            if(konuaramaveri==null)
            {
                topics = c.Topics.ToList();
            }

            List<User> users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).ToList();
            users.Remove(user);
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Topics = topics;
            return View("forum", vm);
        }

        [Route("konusil")]
        public IActionResult konusil(string Id)
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            Topic topic = c.Topics.Find(Convert.ToInt32(Id));
            var comments = c.Comments.Where(p => p.Topic == topic);
            c.Comments.RemoveRange(comments);
            c.Topics.Remove(topic);
            c.SaveChanges();
            List<Topic> topics = c.Topics.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Topic = topic;
            vm.Topics = topics;
            return View("forum", vm);
        }

        [Route("cikis")]
        public IActionResult Cikis()
        {
            TempData["veri"] = null;
            TempData["backright"] = "no";
            
            return RedirectToAction("index");
        }
        [Route("Kayitsayfasi")]
        public IActionResult Kayitsayfasi()
        {
            ViewModel vm = new ViewModel();
            return View(vm);
        }
        //[Route("/Girissayfasi")]
        [HttpGet("Girissayfasi")]
        public IActionResult Girissayfasi()
        {
            ViewModel vm = new ViewModel();
            return View(vm);
        }
        [HttpGet("aramasonuc")]
        public IActionResult Aramasonuc(string aramaveri)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Where(u => u.Username == username).FirstOrDefault();//.Find(username);
            List<Product> products;
            if (aramaveri != null)
            {
                products = c.MyProducts.Where(p => p.Name
               .Contains(aramaveri)).OrderBy(x=>x.Name).ToList();

            }
            else
            {
                 products = c.MyProducts.OrderBy(x=>x.Name).ToList();
            }
            
            
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Products = products;
            return View(vm);
        }

        [HttpGet("kullaniciaramasonuc")]
        public IActionResult Kullaniciaramasonuc(string kullaniciaramaveri)
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            List <User> users;
            if (kullaniciaramaveri != null)
            {
                users = c.MyUsers.Where(u => u.Name
               .Contains(kullaniciaramaveri) || u.Surname.Contains(kullaniciaramaveri))
                    .OrderBy(x => x.Name).ToList();

            }
            else
            {
                users = c.MyUsers.OrderBy(x => x.Name).ToList();
            }
            users.Remove(user);

            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("Kullanicilar",vm);
        }

        [HttpGet("kullanicilar")]
        public IActionResult Kullanicilar()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(u => u.MemberedGroups).Include(u => u.BannedUsers).Where(p => p.Username == username).FirstOrDefault();
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);

            
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View(vm);
        }

        [HttpGet("urunler")]
        public IActionResult Urunler()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            List<Product> products;
            products = c.MyProducts.OrderBy(x => x.Name).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Products = products;
            return View(vm);
        }

        [HttpPost("enteruser")]
        public IActionResult enteruser(string usernameorphonenumber, string password)
        {
            Dao dao = new Dao();
            User us = new User();
            int enterresult = dao.EnterCheck(usernameorphonenumber, password);
            if (enterresult == 1)
            {
                
                User user = c.MyUsers.Include(e => e.MemberedGroups).Include(e=>e.BannedUsers)
                    .Where(p => (p.Username == usernameorphonenumber 
                    || p.Phonenumber == usernameorphonenumber)
                    && p.Password == password).FirstOrDefault();
                ViewBag.enterresult = enterresult;
                ViewModel vm = new ViewModel();
                vm.User = user;
                return View("Index", vm);
            }
            else
                ViewBag.enterresult = enterresult;
            ViewModel vm1 = new ViewModel();
            
            return View("Girissayfasi",vm1);
        }

        [HttpPost("/saveuser")]
        public IActionResult saveuser(string name, string surname, 
                                        string username, string phonenumber, 
                                         string password, string password2)
        {
            Dao dao = new Dao();        
            int kayitsonucu =dao.SaveCheck(name, surname,
                                        username, phonenumber,
                                        password, password2) ;

            ViewModel vm = new ViewModel();
            
            if (kayitsonucu == 1)
            {
                ViewBag.kayitsonucu = 1;

                return View("Girissayfasi",vm);
            }
            else
            {
                ViewBag.kayitsonucu = kayitsonucu;
                return View("Kayitsayfasi",vm);
            }

        }

        [HttpGet("hakkimizda")]
        public IActionResult hakkimizda()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }
        [HttpGet("vizyonumuz")]
        public IActionResult vizyonumuz()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }
        [HttpGet("misyonumuz")]
        public IActionResult misyonumuz()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }
        [HttpGet("iletisim")]
        public IActionResult iletisim()
        {
            User user = new User();
            string username = (string)TempData["veri"];

            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);
        }

        [HttpGet("sikcasorulan")]
        public IActionResult Sikcasorulan()
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);

        }

        [HttpGet("bilgilerimsay")]
        public IActionResult Bilgilerimsay()
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);

        }

        [HttpGet("bilgileriguncellesay")]
        public IActionResult Bilgileriguncellesay()
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);

        }

        [HttpPost("updateuser")]
        public IActionResult updateuser(string name, string surname, string username, string phonenumber)
        {
            
            

            Dao dao = new Dao();
            string oldusername = (string)TempData["veri"];
            int guncellemesonucu = dao.UpdateCheck(name, surname, username, oldusername, phonenumber);

            User user = new User();
            ViewModel vm = new ViewModel();
            string username2 = oldusername;
            if (guncellemesonucu == 1)
            {
                ViewBag.guncellemesonucu = guncellemesonucu;
                if ((username != null && username.Replace("\\s", "") != null))
                {  username2 = username; }
              
                user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                    .Where(e => e.Username == username2).FirstOrDefault();
                
                
                vm.User = user;
                
                return View("Bilgilerimsay", vm);
            }
            else
            {
                ViewBag.guncellemesonucu = guncellemesonucu;
                
                
                user = c.MyUsers.Include(p => p.MemberedGroups).Include(e=>e.BannedUsers)
                    .Where(e => e.Username == username2).FirstOrDefault();
                
                vm.User = user;
                
                return View("Bilgileriguncellesay", vm);
            }
        }


        [HttpGet("sifredegistirsay")]
        public IActionResult Sifredegistirsay()
        {
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View(vm);

        }

        [HttpPost("changepassword")]
        public IActionResult changepassword(string oldpassword,string newpassword,string newpassword2)
        {
            Dao dao = new Dao();
            User user = new User();
            string username = (string)TempData["veri"];
            int passwordchangeresult=dao.ChangePassword(username, oldpassword, newpassword, newpassword2);
            user = c.MyUsers.Include(e => e.MemberedGroups).Where(e => e.Username == username).FirstOrDefault();
            ViewBag.passwordchangeresult = passwordchangeresult;
            ViewModel vm = new ViewModel();
            vm.User = user;
            return View("Sifredegistirsay", vm);
            
            

        }

        [HttpGet("sepeteekle")]
        public IActionResult sepeteekle(string pid)
        {
            
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Where(u => u.Username == username).FirstOrDefault();//.Find(username);
            Product product = new Product();
            product = c.MyProducts.Find(Convert.ToInt32(pid));
            List<Product2> pro = c.MyProducts2.Where(p => p.Name == product.Name
            && p.Price == product.Price
            && p.Origin == product.Origin
            && p.User == user).ToList();
            if (pro.Count == 0)
            {
                Product2 product2 = new Product2();
                product2.Name = product.Name;
                product2.Price = product.Price;
                product2.Origin = product.Origin;
                product2.Number = 1;
                product2.User = user;
                c.MyProducts2.Add(product2);
                c.SaveChanges();
            }
            if (pro.Count>0)
            {
                List<Product2> products2 = c.MyProducts2.Where(p => p.Name == product.Name
            && p.Price == product.Price
            && p.Origin == product.Origin
            && p.User == user).OrderBy(x => x.Name).ToList();
               foreach(Product2 p in products2)
                {
                    if(product.Name==p.Name && product.Price==p.Price && product.Origin==p.Origin)
                    { 
                        p.Number+=1;
                        c.SaveChanges();
                    }
                }
            }



            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Products2 = c.MyProducts2.Where(u => u.User == user).OrderBy(x=>x.Name).ToList();
            return View("Sepetim", vm);
        }

        [HttpGet("sepetim")]
        public IActionResult Sepetim()
        {
            Dao dao = new Dao();
            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Where(u => u.Username == username).FirstOrDefault();//.Find(username);
            List<Product2> products2 = c.MyProducts2.Where(u => u.User == user).OrderBy(x => x.Name).ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Products2 = products2;
            return View(vm);

        }

        [HttpGet("sepettencikar")]
        public IActionResult sepettencikar(string pid)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Where(u => u.Username == username).FirstOrDefault();//.Find(username);
            Product2 product2 = new Product2();
            product2 = c.MyProducts2.Find(Convert.ToInt32(pid));

            if (product2.Number == 1)
            {
                c.MyProducts2.Remove(product2);
                c.SaveChanges();
            }
            if(product2.Number>1)
            {
                product2.Number--;
                c.SaveChanges();
            }
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Products2 = c.MyProducts2.Where(u => u.User == user).OrderBy(x => x.Name).ToList();
            return View("Sepetim", vm);
        }

        [HttpGet("arkadasekle")]
        public IActionResult arkadasekle(string friendrequestreceiver)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups)
                .Include(e => e.BannedUsers)
                .Where(e => e.Username == username).FirstOrDefault();
            
            User receiver = new User();
            receiver = c.MyUsers.Where(u => u.Username == friendrequestreceiver).FirstOrDefault();//.Find(friendrequestreceiver);
            List<FriendRequest> friendrequests = c.FriendRequests.Where(p => p.Sender == user
            && p.Receiver==receiver).ToList();
            List<FriendRequest> friendrequests1 = c.FriendRequests.Where(p => p.Sender == receiver
            && p.Receiver == user).ToList();
            FriendRequest req = new FriendRequest();
            if (friendrequests.Count == 0 && friendrequests1.Count==0)
            {
                
                
                req.Sender = user;
                req.Receiver = receiver;
                c.FriendRequests.Add(req);
                c.SaveChanges();
            }

            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.FriendRequest = req;
            vm.FriendRequests = friendrequests2;
            vm.Friendships = friendships;
            return View("Kullanicilar", vm);
        }

        [HttpGet("arkadaslikisteginigericek")]
        public IActionResult arkadaslikisteginigericek(string friendrequestreceiver)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User receiver = new User();
            receiver = c.MyUsers.Where(u => u.Username == friendrequestreceiver).FirstOrDefault();//.Find(friendrequestreceiver);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);
            
            FriendRequest friendrequest = c.FriendRequests.Where(p => p.Sender == user
             && p.Receiver == receiver).FirstOrDefault();

            

            c.FriendRequests.Remove(friendrequest);
            c.SaveChanges();
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.FriendRequests = friendrequests2;
            vm.Friendships = friendships;
            return View("Kullanicilar", vm);
        }

        [HttpGet("arkadaslikisteginireddet2")]
        public IActionResult arkadaslikisteginireddet(string friendrequestsender)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User sender = new User();
            sender = c.MyUsers.Where(u => u.Username == friendrequestsender).FirstOrDefault();//.Find(friendrequestsender);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);

            FriendRequest friendrequest = c.FriendRequests.Where(p => p.Sender == sender
             && p.Receiver == user).FirstOrDefault();

            

            c.FriendRequests.Remove(friendrequest);
            c.SaveChanges();
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.FriendRequests = friendrequests2;
            vm.Friendships = friendships;
            return View("Kullanicilar", vm);
        }

        [HttpGet("arkadaslikisteginireddet1")]
        public IActionResult arkadaslikisteginireddet1(string friendrequestsender)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault(); 

            User sender = new User();
            sender = c.MyUsers.Where(u => u.Username == friendrequestsender).FirstOrDefault();//.Find(friendrequestsender);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);

            FriendRequest friendrequest = c.FriendRequests.Where(p => p.Sender == sender
             && p.Receiver == user).FirstOrDefault();


            c.FriendRequests.Remove(friendrequest);
            c.SaveChanges();
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.FriendRequests = friendrequests2;
            return View("Arkadaslikistekleri", vm);
        }

        [HttpGet("arkadaslikisteginikabulet")]
        public IActionResult arkadaslikisteginikabulet(string friendrequestsender)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault(); 

            User sender = new User();
            sender = c.MyUsers.Where(u => u.Username == friendrequestsender).FirstOrDefault();//.Find(friendrequestsender);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);


            FriendRequest friendrequest = c.FriendRequests.Where(p => p.Sender == sender
             && p.Receiver == user).First();
            List<Friendship> f1 = c.Friendships.Where(p => p.Friend1 == user && p.Friend2 == sender).ToList();
            List<Friendship> f2 = c.Friendships.Where(p => p.Friend1 == sender && p.Friend2 == user).ToList();
            Friendship friendship = new Friendship();
            if (f1.Count==0 && f2.Count==0)
            {
                

                c.FriendRequests.Remove(friendrequest);
                c.SaveChanges();

                friendship.Friend1 = user;
                friendship.Friend2 = sender;
                c.Friendships.Add(friendship);
                c.SaveChanges();
            }
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships=c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendship = friendship;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("Arkadaslar", vm);
        }

        [HttpGet("arkadaslikisteginikabulet2")]
        public IActionResult arkadaslikisteginikabulet2(string friendrequestsender)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User sender = new User();
            sender = c.MyUsers.Where(u => u.Username == friendrequestsender).FirstOrDefault();//.Find(friendrequestsender);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);


            FriendRequest friendrequest = c.FriendRequests.Where(p => p.Sender == sender
             && p.Receiver == user).First();
            List<Friendship> f1 = c.Friendships.Where(p => p.Friend1 == user && p.Friend2 == sender).ToList();
            List<Friendship> f2 = c.Friendships.Where(p => p.Friend1 == sender && p.Friend2 == user).ToList();
            Friendship friendship = new Friendship();
            if (f1.Count == 0 && f2.Count == 0)
            {


                c.FriendRequests.Remove(friendrequest);
                c.SaveChanges();

                friendship.Friend1 = user;
                friendship.Friend2 = sender;
                c.Friendships.Add(friendship);
                c.SaveChanges();
            }
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendship = friendship;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("Kullanicilar", vm);
        }

        [HttpGet("arkadasliktancikar")]
        public IActionResult arkadasliktancikar(string friendtodefriend)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User ftd = new User();
            ftd = c.MyUsers.Where(u => u.Username == friendtodefriend).FirstOrDefault();//.Find(friendtodefriend);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);
            
            

            Friendship f1 = c.Friendships.Where(p => p.Friend1 == user && p.Friend2 == ftd).FirstOrDefault();
            Friendship f2 = c.Friendships.Where(p => p.Friend1 == ftd && p.Friend2 == user).FirstOrDefault();
               
            
            
            if (f1 == null && f2!=null)
            {

                c.Friendships.Remove(f2);
                c.SaveChanges();
            }
            else if (f1 != null && f2 == null)
            {

                c.Friendships.Remove(f1);
                c.SaveChanges();
            }
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("Arkadaslar", vm);
        }

        [HttpGet("arkadasliktancikar2")]
        public IActionResult arkadasliktancikar2(string friendtodefriend)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User ftd = new User();
            ftd = c.MyUsers.Where(u => u.Username == friendtodefriend).FirstOrDefault();//.Find(friendtodefriend);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);



            Friendship f1 = c.Friendships.Where(p => p.Friend1 == user && p.Friend2 == ftd).FirstOrDefault();
            Friendship f2 = c.Friendships.Where(p => p.Friend1 == ftd && p.Friend2 == user).FirstOrDefault();



            if (f1 == null && f2 != null)
            {

                c.Friendships.Remove(f2);
                c.SaveChanges();
            }
            else if (f1 != null && f2 == null)
            {

                c.Friendships.Remove(f1);
                c.SaveChanges();
            }
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View("Kullanicilar", vm);
        }

        [HttpGet("arkadaslar")]
        public IActionResult Arkadaslar(string friendrequestsender)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User sender = new User();
            sender = c.MyUsers.Where(u => u.Username == friendrequestsender).FirstOrDefault();//.Find(friendrequestsender);
            List<User> users;

            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);


            
            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            List<Friendship> friendships = c.Friendships.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            
            vm.Friendships = friendships;
            vm.FriendRequests = friendrequests2;
            return View(vm);
        }

        [HttpGet("arkadaslikistekleri")]
        public IActionResult Arkadaslikistekleri()
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();


            List<User> users;
            users = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).OrderBy(x => x.Name).ToList();
            users.Remove(user);

            List<FriendRequest> myfriendrequests = c.FriendRequests.Where(p =>
            p.Receiver == user).ToList();

            List<FriendRequest> friendrequests2 = c.FriendRequests.ToList();
            ViewModel vm = new ViewModel();
            vm.User = user;
            vm.Users = users;
            vm.FriendRequests = myfriendrequests;
            return View(vm);
        }

        [HttpPost("mesaj")]
        public IActionResult mesaj(string friendtosendmessage)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User messagereceiver = new User();
            messagereceiver = c.MyUsers.Where(u => u.Username == friendtosendmessage).FirstOrDefault();//.Find(friendtosendmessage);

            List<Message> messages;
            messages = c.Messages.ToList();

            
            ViewModel vm = new ViewModel();

            Message m = new Message();
            m.MessageSender = user;
            m.MessageReceiver = messagereceiver;

            vm.User = user;
            vm.Message = m;
            vm.Messages = messages;
            
            return View("Mesajlasma",vm);
        }

        [HttpPost("mesajgonder")]
        public IActionResult mesajgonder(string friendtosendmessage, string sendedmessage)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User messagereceiver = new User();
            messagereceiver = c.MyUsers.Where(u => u.Username == friendtosendmessage).FirstOrDefault();//.Find(friendtosendmessage);

            

            
            ViewModel vm = new ViewModel();
           
                Message m = new Message();
                m.MessageSender = user;
                m.MessageReceiver = messagereceiver;
            if (sendedmessage != null)
            {
                m.MessageContent = sendedmessage;
                c.Messages.Add(m);
                c.SaveChanges();
            }
            List<Message> messages;
            messages = c.Messages.ToList();

            vm.User = user;
            vm.Message = m;
            vm.Messages = messages;
           
            return View("Mesajlasma", vm);
        }

        [Route("mesajgonder2")]
        public IActionResult mesajgonder2()
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();
            string friendtosendmessage = (string)TempData["friendtosendmessage"];
            User messagereceiver = new User();
            messagereceiver = c.MyUsers.Where(u => u.Username == friendtosendmessage).FirstOrDefault();//.Find(friendtosendmessage);
            Message m = new Message();
            m.MessageSender = user;
            m.MessageReceiver = messagereceiver;
            m.MessageContent = "";
            ViewModel vm = new ViewModel();

            List<Message> messages;
            messages = c.Messages.ToList();

            vm.User = user;
            vm.Message = m;
            vm.Messages = messages;

            return View("Mesajlasma", vm);
        }


        [HttpPost("mesajlasmagecmisinisil")]
        public IActionResult mesajlasmagecmisinisil(string friendtosendmessage)
        {

            User user = new User();
            string username = (string)TempData["veri"];
            user = c.MyUsers.Include(e => e.MemberedGroups).Include(e => e.BannedUsers).Where(e => e.Username == username).FirstOrDefault();

            User messagereceiver = new User();
            messagereceiver = c.MyUsers.Where(u => u.Username == friendtosendmessage).FirstOrDefault();//.Find(friendtosendmessage);




            ViewModel vm = new ViewModel();

            Message m = new Message();
            m.MessageSender = user;
            m.MessageReceiver = messagereceiver;
            //m.MessageContent = sendedmessage;
            var x=c.Messages.Where(p => p.MessageSender == user && p.MessageReceiver == messagereceiver);
            var y=c.Messages.Where(p => p.MessageSender == messagereceiver && p.MessageReceiver == user);
            c.Messages.RemoveRange(x);
            c.Messages.RemoveRange(y);
            c.SaveChanges();

            List<Message> messages;
            messages = c.Messages.ToList();

            vm.User = user;
            vm.Message = m;
            vm.Messages = messages;

            return View("Mesajlasma", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}