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
        public static Baralho Baralho {get;set;}
        public static Pilha Player {get;set;}
        public static Pilha Dealer {get;set;}

        static void Main(string[] args)
        {
            Utils.Print(Utils.CardString.GenStr(new int[]{1,2},new Naipe[]{Naipe.Espadas,Naipe.Copas},2));
            return;

            Auth.Load();
            Baralho = new Baralho();
            Player = new Pilha();
            Dealer = new Pilha();

            Menu.StartEngine();
                        
            Auth.Save();
        }
        public static bool PlayGame(){
            //set bets
            Console.WriteLine("Digite a sua aposta!");
            string apostastr = Console.ReadLine();
            int bet = 0;
            int.TryParse(apostastr, out bet);
            while(bet<=0 || bet>Auth.CurrentUser.Tokens){
                Console.WriteLine($"A aposta não pode ser 0 ou maior que os seus tokens ({Auth.CurrentUser.Tokens})!");
                apostastr = Console.ReadLine();
                int.TryParse(apostastr, out bet);
            }
            Auth.CurrentUser.PartidasJogadas++;
            Console.WriteLine("Distribuindo as cartas!\n\n");
            Dealer.addCard(Baralho.GetCard());
            Player.addCard(Baralho.GetCard());
            Dealer.addCard(Baralho.GetCard());
            Player.addCard(Baralho.GetCard());

            Console.WriteLine($"Cartas da casa:\n\n>>???\n>>{Dealer.Cartas[1]}\n");
            Console.WriteLine($"Suas cartas:\n\n>>{Player.Cartas[0]}\n>>{Player.Cartas[1]}");

            //check if first blackjack
            if(Player.CountCards() == 21){
                Console.WriteLine("Você tem um BlackJack! Você venceu!");
                Console.WriteLine($"+{bet/2} Tokens");
                Auth.CurrentUser.Tokens+=bet/2;
                Auth.CurrentUser.Blackjacks++;
                Auth.CurrentUser.PartidasGanhas++;
                return true;
            }
            bool stopped=false;
            bool doubled=false;
            bool lost = false;
            bool won = false;
            string input ="";
            int count = 0;
            Card buycard;
            while(!(stopped || doubled || lost || won)){
                Console.WriteLine("O que você quer fazer?\n1-Comprar\n2-Dobrar\n3-Parar");
                input = Console.ReadLine();
                switch(input){
                    case "1":
                        buycard = Baralho.GetCard();
                        Console.WriteLine($"Veio um(a) {buycard.ToString()}");
                        Player.addCard(buycard);

                        //output cards
                        Utils.Print("Suas cartas: ");
                        Player.OutputCards();

                        if(Player.CountCards()>21){ 
                            Console.WriteLine("Você passou de 21!");
                            lost=true;
                        }
                        else if(Player.CountCards() == 21){
                            Console.WriteLine("Você tem um BlackJack! Você venceu!");
                            Console.WriteLine($"+{bet/2} Tokens");
                            Auth.CurrentUser.Tokens+=bet/2;
                            Auth.CurrentUser.Blackjacks++;
                            Auth.CurrentUser.PartidasGanhas++;
                            return true;
                            }
                        break;
                    case "2":
                        if(!(Auth.CurrentUser.Tokens >= bet*2)){
                            Console.WriteLine("Você não tem tokens o suficiente para dobrar :(");
                            continue;
                        }
                        doubled=true;
                        bet*=2;
                        Console.WriteLine($"Aposta atual: {bet}");
                        buycard = Baralho.GetCard();
                        Console.WriteLine($"Veio um(a) {buycard.ToString()}");
                        Player.addCard(buycard);
                        count = Player.CountCards();
                        if(count>21){ Console.WriteLine("Você passou de 21!");lost=true;}
                        else if(count ==21){
                            Console.WriteLine("BlackJack!!!");
                            won=true;
                            Auth.CurrentUser.Tokens+=(int)Math.Round(bet*1.5);
                            Auth.CurrentUser.Blackjacks++;
                            return true;
                            }
                        break;
                    case "3":
                    stopped=true;
                    break;
                }
            }
            //vez da casa
            //virar primeira carta
            Console.WriteLine($"A casa tem {Dealer.CountCards()}");
            while(Dealer.CountCards() < 17){
                Card dbuycard;
                dbuycard = Baralho.GetCard();
                Dealer.addCard(dbuycard);
                Console.WriteLine($"A casa comprou um {dbuycard.ToString()}, agora ela tem {Dealer.CountCards()}");
            }
            //check qtt
            if(Dealer.CountCards() > 21){
                Console.WriteLine("A casa passou de 21, você ganhou!");
                Console.WriteLine($"+{(int)Math.Round(bet*1.5)} Tokens");
                Auth.CurrentUser.Tokens += (int)Math.Round(bet*1.5);
                Auth.CurrentUser.PartidasGanhas++;
            }else if(Dealer.CountCards() <= 21 && Dealer.CountCards() > Player.CountCards()){
                Console.WriteLine("A casa tem mais que você, perdeu!");
                Console.WriteLine($"-{bet} Tokens");
                Auth.CurrentUser.PartidasPerdidas++;
                Auth.CurrentUser.Tokens -= bet;
            }else if(Dealer.CountCards() < Player.CountCards() && Player.CountCards() <=21){
                Console.WriteLine("Você tem mais que a casa, ganhou!");
                //ganhar tokens
                Auth.CurrentUser.Tokens += (int)Math.Round(bet*1.5);
                Console.WriteLine($"+{(int)Math.Round(bet*1.5)} Tokens");
                Auth.CurrentUser.PartidasGanhas++;
            }else if(Dealer.CountCards() == Player.CountCards()){
                Console.WriteLine("Você tem a mesma quantidade que a casa, empate!");
                //restituir
                Auth.CurrentUser.PartidasEmpatadas++;
                Auth.CurrentUser.Tokens += bet;
                
            }
            return true;
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
                PlayGame();
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
