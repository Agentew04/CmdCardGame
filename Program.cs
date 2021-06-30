using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;

namespace BlackJackJs
{
    class Program
    {
        

        static void Main(string[] args)
        {

            Auth.Load();

            Menu.StartEngine();
                        
            Auth.Save();
        }
        

        static void GetMenuInput(){
            List<String> play = new(){ "1","jogar","Jogar","JOGAR","play","Play","PLAY"};
            List<String> profile = new(){ "2","perfil","Perfil","PERFIL","profile","Profile","PROFILE",
            "Ver perfil", "ver perfil", "VER PERFIL"};
            List<String> exit = new(){ "3","sair","Sair","SAIR","exit","Exit","EXIT", "leave",
            "Leave","LEAVE","dc","DC","Dc"};
            Console.WriteLine("\n\nO que você quer fazer?\n\n-Jogar\n-Ver perfil\n-Sair");
            string response = Console.ReadLine();
            if(play.Contains(response)){
                Game.PlayGame();
            }else if(profile.Contains(response)){
                //Menu.ShowProfile();
            }else if(exit.Contains(response)){
                return;
            }else{
                GetMenuInput();
            }
        }
    }
}
