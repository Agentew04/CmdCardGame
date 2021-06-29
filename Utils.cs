using System;
using System.Collections.Generic;
using BlackJackJs;

namespace BlackJackJs{
    public static class Menu{
        public static void StartEngine(){
            Utils.Print("####################################");
            Utils.Print("#      Bem-vindo ao BlackJac#      #");
            Utils.Print("#                                  #");
            Utils.Print("#            V0.2 -Beta            #");
            Utils.Print("####################################");
            Utils.Print();
            Utils.Print("Digite o seu nome de usuário: ");
            Utils.Print("(Se não existe, um será criado para você!)");
            Utils.Printf("->"); 
            string name = Utils.GetInput();
            Auth.Login(name);
            Utils.Print("Logado como: " + Auth.CurrentUser.Name);
            ShowMenu();
        }
        public static void ShowMenu(){
            RouteMenuAction(Menu.ShowMenuActions());
        }
        public static int ShowMenuActions(){
            Utils.Print();
            Utils.Print("O que você deseja fazer?");
            int i = 1;
            foreach(var act in Actions.MenuActionsDict){
                Utils.Print($"{i}- {act.Value}");
                i++;
            }
            int maxchoice = i-1;
            int choice = Utils.GetIntInput(maxchoice);
            return choice;
        }
        public static int ShowHighScoreOptions(){
            Utils.Print();
            Utils.Print("O que você deseja fazer?");
            int i = 1;
            foreach(var act in Actions.HighScoreSortingDict){
                Utils.Print($"{i}- {act.Value}");
                i++;
            }
            int maxchoice = i-1;
            int choice = Utils.GetIntInput(maxchoice);
            return choice;
        }
        public static void RouteMenuAction(int numberchoice){
            MenuActions choice = (MenuActions)(numberchoice-1);
            switch (choice)
            {
                case MenuActions.Jogar:
                    Program.PlayGame();
                    break;
                case MenuActions.VerPerfil:
                    RouteProfileAction(ShowProfileActions());
                    break;
                case MenuActions.VerHighscores:
                    ShowHighScores(ShowHighScoreOptions());
                    break;
                case MenuActions.ConseguirMaisTokens:
                    break;
                case MenuActions.Deslogar:
                    break;
                case MenuActions.Sair:
                    return;
                default:
                    break;
            }
            Utils.Standby();
            ShowMenu();
        }
        public static int ShowProfileActions(){
            Utils.Print();
            Utils.Print("O que você deseja fazer?");
            int i = 1;
            foreach(var act in Actions.ProfileActionsDict){
                Utils.Print($"{i}- {act.Value}");
                i++;
            }
            int maxchoice = i-1;
            int choice = Utils.GetIntInput(maxchoice);
            return choice;
        }
        public static void RouteProfileAction(int numberchoice){
            ProfileActions choice = (ProfileActions)(numberchoice-1);
            switch (choice)
            {
                case ProfileActions.VerSeuPerfil:
                    ShowProfile(Auth.CurrentUser.Name);
                    break;
                case ProfileActions.VerOutroPerfil:
                    Utils.Printf("Qual usuário você quer procurar?\n->");
                    ShowProfile(Utils.GetInput());
                    break;
                case ProfileActions.AdicionarAmigo:
                    Utils.Print("Não implementado ainda!");
                    break;
                case ProfileActions.DeletarSuaConta:
                    break;
                case ProfileActions.VerAmigos:
                    break;
                case ProfileActions.EnviarMsg:
                    break;
                case ProfileActions.LerMsg:
                    break;
                case ProfileActions.ResetarProgresso:
                    break;
                case ProfileActions.VoltarMenu:
                    
                    break;
                default:
                    Utils.Print("Erro interno\nUtils.cs\nRouteProfileAction()\nswitch(choice)");
                    break;
            }
            //nao precisa chamar o menu aqui pq volta pro RouteMenuAction
        }

        public static void ShowProfile(string username){
            float getkd(User us){
                if(us.PartidasJogadas==0){
                    return 0;
                }else{
                    return ((float)us.PartidasGanhas)/((float)us.PartidasJogadas);
                }
            }
            if(username == Auth.CurrentUser.Name){
                //himself

                var user = Auth.CurrentUser;
                Utils.Print($"Mostrando o perfil de {user.Name}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"Nível: {user.Level}");
                Utils.Print($"Experiência: {user.Experience}");
                
                Utils.Print($"Patente: {Enum.GetName(typeof(Patente),user.Patente)}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"Tokens: {user.Tokens}");
                Utils.Print($"Tokens ganhos: {user.TokensWon}");
                Utils.Print($"Tokens perdidos: {user.TokensLost}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"BlackJacks: {user.Blackjacks}");
                Utils.Print($"Partidas jogadas: {user.PartidasJogadas}");
                Utils.Print($"Partidas ganhas: {user.PartidasGanhas}");
                Utils.Print($"Partidas empatadas: {user.PartidasEmpatadas}");
                Utils.Print($"Partidas perdidas: {user.PartidasPerdidas}");
                Utils.Print($"Relação Vitória/Jogadas: {getkd(user)}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
            }else{
                //other
                
                var user = Auth.GetUser(username);
                Utils.Print($"Mostrando o perfil de {user.Name}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"Nível: {user.Level}");
                Utils.Print($"Experiência: {user.Experience}");
                Utils.Print($"Patente: {Enum.GetName(typeof(Patente),user.Patente)}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"Tokens: {user.Tokens}");
                Utils.Print($"Tokens ganhos: {user.TokensWon}");
                Utils.Print($"Tokens perdidos: {user.TokensLost}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
                Utils.Print($"BlackJacks: {user.Blackjacks}");
                Utils.Print($"Partidas jogadas: {user.PartidasJogadas}");
                Utils.Print($"Partidas ganhas: {user.PartidasGanhas}");
                Utils.Print($"Partidas empatadas: {user.PartidasEmpatadas}");
                Utils.Print($"Partidas perdidas: {user.PartidasPerdidas}");
                Utils.Print($"Relação Vitória/Jogadas: {getkd(user)}");
                Utils.Print($"-=-=-=-=-=-=-=-=-=-");
            }
        }
    
        public static void ShowHighScores(int i){
            HighScoreSorting choice = (HighScoreSorting)(i-1);
            var users = Auth.GetUsers();
            List<User> top10 = new();
        }
    }

    public static class Utils {
        public static void Print(string s = "\n"){
            Console.WriteLine(s);
        }
        public static void Printf(string s = ""){
            Console.Write(s);
        }
        public static void Standby(){
            Utils.Printf("Pressione qualquer tecla para continuar");
            Console.ReadKey();
        }
        /// <summary>
        /// Gets and sanitizes a string input
        /// </summary>
        /// <returns></returns>
        public static string GetInput(){
            string i = "";
            bool isAcquired = false;
            while(!isAcquired){
                i = Console.ReadLine();

                if(i == null){
                    continue;
                }
                i = i.Trim();
                if(string.IsNullOrEmpty(i)){
                    continue;
                }
                isAcquired = true;
            }
            
            return i;
        }
        /// <summary>
        /// Gets and sanitizes a non negative integer input from user
        /// </summary>
        /// <returns>The value</returns>
        public static int GetIntInput(){
            bool isAcquired = false;
            string input = "";
            int result = 0;
            while(!isAcquired){
                input = Console.ReadLine();

                if(string.IsNullOrEmpty(input)){
                    continue;
                }
                input = input.Trim();
                try
                {
                    result = int.Parse(input);
                }
                catch (Exception)
                {
                    continue;
                }
                if(result == 0){
                    continue;
                }
                result = Math.Abs(result);

                isAcquired=!isAcquired;
            }
            return result;
        }
        /// <summary>
        /// Gets and sanitizes a non negative integer input from user
        /// </summary>
        /// <param name="maximum">The maximum number the user can enter</param>
        /// <returns>The value inputted by the user</returns>
        public static int GetIntInput(int maximum){
            bool isAcquired = false;
            string input = "";
            int result = 0;
            while(!isAcquired){
                input = Console.ReadLine();

                if(string.IsNullOrEmpty(input)){
                    continue;
                }
                input = input.Trim();
                try
                {
                    result = int.Parse(input);
                }
                catch (Exception)
                {
                    continue;
                }
                result = Math.Abs(result);
                if(result == 0 || result>maximum){
                    continue;
                }

                isAcquired=!isAcquired;
            }
            return result;
        }
    }

}