using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebApplication2.Models;

namespace WebApplication2.DAO
{
    public class Dao
    {
        //private readonly Context c;
        //public Dao(Context context)
        //{
        //    this.c = context;
        //}

        public Dao()
        {
        }

        Context c = new Context();
        public String sifrele(String s, int ks)
        {
            String sifreli = "";
            String orjinal_alfabe = "abcçdefgğhiıjklmnoöprsştuüvyzxwq0123456789ABCÇDEFGĞHİIJKLMNOÖPRSŞTUÜVYZXWQ -_.@";
            String alfabe = "cşÜXRyU462arw5ozçGJMCN9YxöZhtAQevsDIFdkİPnbü3qpu8ĞıLmlifWgŞ jBKHVT7ÇSO0ğ1ÖE-_.@";
            int i = 0;
            int j = 0;
            while (i < s.Length)
            {
                j = 0;
                while (j < alfabe.Length)
                {
                    if (s[i] == alfabe[j])
                    {
                        int a = j + ks;
                        if (a < 0) a = alfabe.Length + a;
                        if (a > alfabe.Length) a = a - alfabe.Length;
                        sifreli += alfabe[a];

                    }
                    j++;
                }
                i++;
            }
            return sifreli;
        }
        public String sifreCoz(String s, int ks)
        {
            String sifresiz = "";
            String orjinal_alfabe = "abcçdefgğhiıjklmnoöprsştuüvyzxwq0123456789ABCÇDEFGĞHİIJKLMNOÖPRSŞTUÜVYZXWQ -_.@";
            String alfabe = "cşÜXRyU462arw5ozçGJMCN9YxöZhtAQevsDIFdkİPnbü3qpu8ĞıLmlifWgŞ jBKHVT7ÇSO0ğ1ÖE-_.@";
            int i = 0;
            int j = 0;
            while (i < s.Length)
            {
                j = 0;
                while (j < alfabe.Length)
                {
                    if (s[i] == alfabe[j])
                    {
                        int a = j - ks;
                        if (a < 0) a = alfabe.Length + a;
                        if (a > alfabe.Length) a = a - alfabe.Length;
                        sifresiz += alfabe[a];

                    }
                    j++;
                }
                i++;
            }
            return sifresiz;
        }

        public int SaveCheck(string name, string surname,
            string username, string phonenumber,
            string password, string password2)
        {

            List<User> users = c.MyUsers.ToList();
            foreach (User u in users)
            {
                if (username == u.Username) return 2;
                if (phonenumber == u.Phonenumber) return 3;
            }
            if (password != password2) return 4;
            if (!Regex.IsMatch(name, "^[öüÖÜĞğşŞçÇıİ|a-z|A-Z]{2,20}(\\s[öüÖÜĞğşŞçÇıİ|a-z|A-Z]{2,20})?$")) return 5;
            if (!Regex.IsMatch(surname, "^[öüÖÜĞğşŞçÇıİa-zA-Z]{2,20}$")) return 6;
            if (!Regex.IsMatch(username, "^[öüÖÜĞğşŞçÇıİa-zA-Z0-9]{2,20}$")) return 7;
            if (!Regex.IsMatch(phonenumber, "^[0]?[0-9]{3}[0-9]{3}[0-9]{2}[0-9]{2}$")) return 8;
            if (!Regex.IsMatch(password, "^[öüÖÜĞğşŞçÇıİa-zA-Z0-9_\\.\\-]+$")) return 9;
            User user = new User();
            user.Name = name;
            user.Surname = surname;
            user.Username = username;
            user.Phonenumber = phonenumber;
            user.Password = password;
           
            c.MyUsers.Add(user);
            c.SaveChanges();
            return 1;
            return 100;
        
        }
        public int EnterCheck(string usernameorphonenumber, string password)
        {
            List<User> users = c.MyUsers.Include(e=>e.MemberedGroups).Include(e=>e.BannedUsers).ToList();
            bool unamesame=false;
            bool phonesame = false;
            bool unameorphonesame = false;
            bool unameorphoneandpasssame = false;
            bool passsame = false;
            int i = 0;
            while(i<users.Count)
            {
                if ((usernameorphonenumber == users[i].Username 
                    || usernameorphonenumber == users[i].Phonenumber) && password 
                    == users[i].Password)
                {
                    unameorphoneandpasssame = true;
                    unameorphonesame = true;
                    passsame = true;

                    
                }
                else if (usernameorphonenumber == users[i].Username
                    || usernameorphonenumber == users[i].Phonenumber)
                { unameorphonesame = true; }
                else if ( password== users[i].Password)
                { passsame = true; }
                i++;
            }
            
            if (unameorphonesame==false && passsame==false) return 2;
            if (unameorphonesame==false ) return 3;
            if (passsame == false) return 4;
            
           
            return 1;
            return 100;
        }
        
        public int UpdateCheck(string name, string surname,
            string username,string oldusername, string phonenumber)
        {

            List<User> users = c.MyUsers.Include(p =>p.MemberedGroups).ToList();
            User us = c.MyUsers.Include(e => e.MemberedGroups).Include(e=>e.BannedUsers).Include(e=>e.BanningUsers)
                .Include(e=>e.BanningGroups)
                .Where(p => p.Username == oldusername).FirstOrDefault();
            users.Remove(us);
            
            foreach (User u in users)
            {
                if (username == u.Username) return 2;
                if (phonenumber == u.Phonenumber) return 3;
            }
            //if ((name != null && name.Replace("\\s", "") != null))
             if (!Regex.IsMatch(name, "^[öüÖÜĞğşŞçÇıİ|a-z|A-Z]{2,20}(\\s[öüÖÜĞğşŞçÇıİ|a-z|A-Z]{2,20})?$")) return 5; 
            //if ((surname != null && surname.Replace("\\s", "") != null))
             if (!Regex.IsMatch(surname, "^[öüÖÜĞğşŞçÇıİa-zA-Z]{2,20}$")) return 6; 
            //if ((username != null && username.Replace("\\s", "") != null))
             if (!Regex.IsMatch(username, "^[öüÖÜĞğşŞçÇıİa-zA-Z0-9]{2,20}$")) return 7; 
            //if ((phonenumber != null && phonenumber.Replace("\\s", "") != null))
             if (!Regex.IsMatch(phonenumber, "^[0]?[0-9]{3}[0-9]{3}[0-9]{2}[0-9]{2}$")) return 8; 
            if((name != null && name.Replace("\\s", "") != null)) us.Name = name;
            if ((surname != null && surname.Replace("\\s", "") != null)) us.Surname = surname;
            if ((username != null && username.Replace("\\s", "") != null)) us.Username = username;
            if ((phonenumber != null && phonenumber.Replace("\\s", "") != null)) us.Phonenumber = phonenumber;
            c.MyUsers.Update(us);
            //User user = new User();
            //user.Name = name;
            //user.Surname = surname;
            //user.Username = username;
            //user.Phonenumber = phonenumber;
            //user.Password = us.Password;
            //user.MemberedGroups = us.MemberedGroups;
            //user.BannedUsers = us.BannedUsers;
            //user.BanningUsers = us.BanningUsers;
            //user.BanningGroups = us.BanningGroups;
            //if (us.IsAdmin == true) user.IsAdmin = true;
            //if (us.IsOwner == true) user.IsOwner = true;
            //c.MyUsers.Remove(us);
            //c.MyUsers.Add(user);
            //c.MyProducts2.Where(p => p.User == us)
            //        .ToList().ForEach(x => x.User = user);
            //c.FriendRequests.Where(p => p.Sender == us)
            //        .ToList().ForEach(x => x.Sender = user);
            //c.FriendRequests.Where(p => p.Receiver == us)
            //        .ToList().ForEach(x => x.Receiver = user);
            //c.Friendships.Where(p => p.Friend1 == us)
            //        .ToList().ForEach(x => x.Friend1 = user);
            //c.Friendships.Where(p => p.Friend2 == us)
            //        .ToList().ForEach(x => x.Friend2 = user);
            //c.Messages.Where(p => p.MessageReceiver == us)
            //        .ToList().ForEach(x => x.MessageReceiver = user);
            //c.Messages.Where(p => p.MessageSender == us)
            //        .ToList().ForEach(x => x.MessageSender = user);
            //c.Topics.Where(p => p.Owner == us)
            //        .ToList().ForEach(x => x.Owner = user);
            //c.Comments.Where(p => p.Owner == us)
            //        .ToList().ForEach(x => x.Owner = user);

            //List<AlagramGroup> groups = c.AlagramGroups.Include(p => p.Members).Include(e=>e.BannedUsers)
            //    .ToList();
            //foreach (AlagramGroup group in groups)
            //{
            //    if (group.BannedUsers.Contains(us))
            //    {
            //        group.BannedUsers.Remove(us);
            //        group.BannedUsers.Add(user);
            //    }
            //    if (group.Members.Contains(us))
            //    {
            //        group.Members.Remove(us);
            //        group.Members.Add(user);

            //    }
            //    if (group.Owner == us)
            //    { group.Owner = user; }
            //    c.AlagramGroups.Update(group);
            //}

            c.SaveChanges();

            return 1;
            return 100;

        }

        
        public int ChangePassword(string username,string oldpassword,string newpassword,string newpassword2)
        {
            if (newpassword != newpassword2) return 2;
            if (!Regex.IsMatch(newpassword, "^[öüÖÜĞğşŞçÇıİa-zA-Z0-9_\\.\\-]+$")) return 3;
            User us = c.MyUsers.Include(e => e.MemberedGroups).Include(e=>e.BannedUsers)
                .Where(p => p.Username == username).FirstOrDefault();
            if (us.Password != oldpassword) return 4;
            us.Password = newpassword;
            c.MyUsers.Update(us);
            c.SaveChanges();
            return 1;
        }

    }
}
