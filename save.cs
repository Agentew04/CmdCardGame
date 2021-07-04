using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiteDB;
using System.Linq;

namespace BlackJackJs{
    public class SaveGameState{
        public List<User> usuarios {get;set;} = new();
        public SaveGameState(){
            
        }
    }
    public class User{
        /// <summary>
        /// Creates a new instance of a user
        /// </summary>
        /// <param name="name">The player nickname</param>
        /// <param name="tokens">The inital tokens of the player</param>
        /// <param name="patente">The inital rank for this player</param>
        /// <param name="partidasJogadas">The number of matches the player has played so far</param>
        /// <param name="blackjacks">The number of blackjacks the player achieved</param>
        /// <param name="partidasPerdidas">The number of the matches the player has lost</param>
        /// <param name="partidasEmpatadas">The number of the matches the player has tied</param>
        /// <param name="partidasGanhas">The numbero of matches the player has won</param>
        public User(string name = "",
                    int tokens = 100,
                    int level = 1,
                    int experience = 0,
                    Patente patente = Patente.Unranked,
                    int partidasJogadas = 0,
                    int blackjacks = 0,
                    int partidasPerdidas = 0,
                    int partidasEmpatadas = 0,
                    int partidasGanhas = 0) 
        {
            this.Name = name;
            this.Tokens = tokens;
            this.Level = level;
            this.Patente = patente;
            this.PartidasJogadas = partidasJogadas;
            this.Blackjacks = blackjacks;
            this.PartidasPerdidas = partidasPerdidas;
            this.PartidasEmpatadas = partidasEmpatadas;
            this.PartidasGanhas = partidasGanhas;  
            this.Friends = new List<string>();
            this.Mail = new List<Message>();
        }
        #region properties

        /// <summary>
        /// The nickname of the player
        /// </summary>
        /// <value></value>
        public string Name {get;set;} = "";

        /// <summary>
        /// The amount of tokens the player has
        /// </summary>
        /// <value></value>
        public int Tokens {get;set;} = 100;

        /// <summary>
        /// The amount of tokens the player has lost so far
        /// </summary>
        /// <value></value>
        public int TokensLost { get; set; } = 0;

        /// <summary>
        /// The amount of tokens the player has won so far
        /// </summary>
        /// <value></value>
        public int TokensWon { get; set; } = 0;
        public int Experience { get; set;} = 0;
        public int Level { get; set; } = 1;
        public Patente Patente {get;set;} = Patente.Unranked;
        public int PartidasJogadas{get;set;} = 0;
        public int Blackjacks{get;set;} = 0;
        public int PartidasPerdidas{get;set;} = 0;
        public int PartidasEmpatadas{get;set;}=0;
        public int PartidasGanhas{get;set;}=0;
        public List<string> Friends { get; set;} = new();
        public List<Message> Mail {get;set;} = new();
        public BsonValue _id {get;set;}

        #endregion

        #region social
        public bool ReceiveMail(Message msg){
            if(msg.ToUser == this.Name && msg.Content != null){
                this.Mail.Add(msg);
                return true;
            }else{
                return false;
            }
        }

        public bool SendMail(string toUser, string message){
            Message msg = new Message(toUser,message);
            var user = Saving.GetUser(toUser);
            return user.ReceiveMail(msg);
        }public bool SendMail(Message message){
            if(!string.IsNullOrWhiteSpace(message.ToUser)
                && message.Content != null
                && message.Content != ""){
                var user = Saving.GetUser(message.ToUser);
                return user.ReceiveMail(message);
            }
            return false;
        }
        public bool ReadMail(Guid msgId){
            var msg = this.Mail.AsQueryable()
                .Where(msg => msg.MessageId == msgId)
                .Select(msg => msg)
                .First();
            if(msg!= null){
                msg.WasRead = true;
                return true;
            }
            return false;
        }
        public bool ReadMail(Message message){
            var msg = this.Mail.AsQueryable()
                .Where(msg => msg.MessageId == message.MessageId)
                .Select(msg => msg)
                .First();
            if(msg!= null){
                msg.WasRead = true;
                return true;
            }
            return false;
        }
        public bool AddFriend(string username){
            if(string.IsNullOrWhiteSpace(username)){
                var user = Saving.GetUser(username);
                if(user!=null){
                this.Friends.Add(username);
                user.Friends.Add(this.Name);
                return true;
                }
            }
            return false;
        }
        public bool RemoveFriend(string username){
            if(string.IsNullOrWhiteSpace(username)){
                Friends.RemoveAll(x=>x==username);
                User friend = Saving.GetUser(username);
                if(friend!=null){
                    friend.Friends.Remove(Name);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region overrides

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        #endregion
    }



    //poco class entity = user
    public static class Saving{
        public static string apppath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Rodrigo's_Stuff\BlackJackCMD\users.db";
        public static void Update(User user){
            using(var db = new LiteDatabase(apppath)){
                var col = db.GetCollection<User>("users");
                col.EnsureIndex(x => x.Name);
                col.Update(user);
            }
        }
        public static bool CreateUser(string username){
            using(var db = new LiteDatabase(apppath)){
                var col = db.GetCollection<User>("users");
                var user1 = col.Query().Where(u=>u.Name==username).Select(u=>u).First();
                if(user1==null){
                    var user = new User(name: username);
                    col.Insert(user);
                    return true;
                }
                return false;
            }
        }
        public static User GetUser(string username){
            using(var db= new LiteDatabase(apppath)){
                var col = db.GetCollection<User>("users");

                return  col.Query()
                            .Where(x => x.Name == username)
                            .Select(x=>x)
                            .First();
                
            }
        }
        public static List<User> GetUsers(){
            using(var db = new LiteDatabase(apppath)){
                var col = db.GetCollection<User>("users");

                return col.Query()
                .Select(user=>user)
                .ToList();
            }
        }
    }
}