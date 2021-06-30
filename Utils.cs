using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                    Game.PlayGame();
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
                    Menu.StartEngine();
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
            Collection<User> col = new Collection<User>(users);
            //var ordered = col.
        }
        public static GameActions ShowGameActions(){
            Utils.Print();
            Utils.Print("O que você deseja fazer?");
            int i = 1;
            foreach(var act in Actions.GameActionsDict){
                Utils.Print($"{i}- {act.Value}");
                i++;
            }
            int choice = Utils.GetIntInput(i-1);

            return (GameActions)(choice-1);
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
        public static void Standby(string message){
            Utils.Printf(message);
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
                result = (int)Math.Abs((long)result);

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
                result = (int)Math.Abs((long)result);
                if(result == 0 || result>maximum){
                    continue;
                }

                isAcquired=!isAcquired;
            }
            return result;
        }
    
        private static string getnaipe(Naipe np,bool hideone=false){
            if(!hideone){
                switch(np){
                case Naipe.Espadas:
                    return "# Espadas #";
                case Naipe.Copas:
                    return "#  Copas  #";
                case Naipe.Ouros:
                    return "#  Ouros  #";
                case Naipe.Paus:
                    return "#  Paus   #";
                default:
                    return "###########";
                }
            }else{
                return "#   ???   #";
            }
            
        }
        private static string[] getcard(int nm,Naipe np,bool hideone=false){
            if(!hideone){
                if(nm<10){
                    string[] x = {"###########",
                    $"#{nm}        #",
                    "#         #",
                    getnaipe(np),
                    "#         #",
                    $"#        {nm}#",
                    "###########"};
                    return x;
                }else{
                    string[] x = {
                        "###########",
                        $"#{nm}       #",
                        "#         #",
                        getnaipe(np),
                        "#         #",
                        $"#       {nm}#",
                        "###########"
                    };
                    return x;
                }
            }else{
                string[] x = {
                        "###########",
                        "#?        #",
                        "#         #",
                        getnaipe(np,hideone),
                        "#         #",
                        "#        ?#",
                        "###########"
                    };
                return x;
            }
            
        }
        public static string GenStr(int[] nm,Naipe[] np, int count=1){
            if(nm.Length != count && np.Length !=count){
                return "??";
            }
            List<string> x = new List<string>(){
                "","","","","","",""
            };
            for(var i=0;i<count;i++){
                var y = getcard(nm[i],np[i]);
                x[0] += "  "+y[0];
                x[1] += "  "+y[1];
                x[2] += "  "+y[2];
                x[3] += "  "+y[3];
                x[4] += "  "+y[4];
                x[5] += "  "+y[5];
                x[6] += "  "+y[6];
            }
            string w ="";
            foreach(var z in x){
                w += z + "\n";
            }
            return w;

        }
        
        public static string GenStr(int[] nm,Naipe[] np, int count=1,bool hideone=false){
            if(!hideone){
                return GenStr(nm,np,count);
            }else{
                if(nm.Length != count && np.Length !=count){
                    return "??";
                }
                List<string> x = new List<string>(){
                    "","","","","","",""
                };
                for(var i=0;i<count;i++){
                    if(i==0){
                        var y = getcard(nm[i],np[i],hideone);
                        x[0] += "  "+y[0];
                        x[1] += "  "+y[1];
                        x[2] += "  "+y[2];
                        x[3] += "  "+y[3];
                        x[4] += "  "+y[4];
                        x[5] += "  "+y[5];
                        x[6] += "  "+y[6];
                    }else{
                        var y = getcard(nm[i],np[i]);
                        x[0] += "  "+y[0];
                        x[1] += "  "+y[1];
                        x[2] += "  "+y[2];
                        x[3] += "  "+y[3];
                        x[4] += "  "+y[4];
                        x[5] += "  "+y[5];
                        x[6] += "  "+y[6];
                    }
                }
                string w ="";
                foreach(var z in x){
                    w += z + "\n";
                }
                return w;
            }
        }
    }

}