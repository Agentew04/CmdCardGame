using System;

namespace BlackJackJs{
    public static class Auth{
        public static string apppath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Rodrigo's_Stuff\BlackJackCMD\save.bsave";
        public static string Username { get; set;} = "";
        //public static User CurrentUser{get;set;} = new User();
        // public static void CreateUser(string username){
        //     CurrentUser = new User(name: username);
        // }
        public static bool Login(string username){
            if(username!=null){
                Username=username;
                var user = Saving.GetUser(username);
                if(user==null){
                    Utils.Print("Usuário não existente, criando " + username);
                    Saving.CreateUser(username);
                }
            }
            return false;
        }
        // public static void Load(){
        //     if(!CheckFiles()){File.WriteAllText(apppath,JsonConvert.SerializeObject(new SaveGameState()));}
        //     save = JsonConvert.DeserializeObject<SaveGameState>(File.ReadAllText(apppath));
        // }
        // public static void Save(){
        //     CheckFiles();

        //     //redirect to save
        //     //Saving.Update(Auth.CurrentUser);
        //     var users = GetUsers();
        //     int i;
        //     for(i=0;i<users.Count;i++){
        //         if(users[i].Name==CurrentUser.Name){
        //             break;
        //         }
        //     }
        //     if(save==null){
        //         save = new SaveGameState();
        //         CurrentUser.Name = CurrentUser.Name;
        //         save.usuarios.Add(CurrentUser);
        //         File.WriteAllText(apppath,JsonConvert.SerializeObject(save));
        //         return;
        //     }
        //     CurrentUser.Name = CurrentUser.Name;
        //     try{save.usuarios[i] = CurrentUser;}
        //     catch(Exception){
        //         CurrentUser.Name = CurrentUser.Name;
        //         save.usuarios.Add(CurrentUser);
        //     }
        //     File.WriteAllText(apppath,JsonConvert.SerializeObject(save));
        // }
        // public static bool CheckFiles(){
        //     if(!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Rodrigo's_Stuff\BlackJackCMD\")){
        //         Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\Rodrigo's_Stuff\BlackJackCMD\");
        //         File.Create(apppath).Close();
        //         return false;
        //     }

        //     if (File.Exists(apppath)){
        //         return true;
        //     }else{
        //         File.Create(apppath).Close();
        //         return false;
        //     }
        // }
        // public static List<User> GetUsers(){
        //     List<User> list= new();
        //     if(save==null){
        //         return list;
        //     }
        //     if(save.usuarios==null){return list;}
        //     foreach(var user in save.usuarios){
        //         if(user ==null){
        //             continue;
        //         }
        //         list.Add(user);
        //     }
        //     return list;
        // }
        // public static User GetUser(string username){
        //     if(save==null){
        //         return null;
        //     }
        //     if(save.usuarios==null){
        //         return null;
        //     }
        //     foreach(var user in save.usuarios){
        //         if(user ==null){
        //             continue;
        //         }
        //         if(user.Name == username){
        //             return user;
        //         }
        //     }
        //     return null;
        // }
    }
}