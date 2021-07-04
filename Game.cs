using System;

namespace BlackJackJs{
    public static class Game{
        public static Baralho Baralho {get;set;}
        public static Pilha Player {get;set;}
        public static Pilha Dealer {get;set;}

        public static void ResetGame(){
            Baralho = new();
            Player=new();
            Dealer=new();
        }

        public static bool EvalBlackjack(Pilha deck){
            int c = deck.CountCards();
            if(c == 21){
                return true;
            }
            return false;
        }
        public static bool EvalBust(Pilha deck){
            int c = deck.CountCards();
            if(c > 21){
                return true;
            }
            return false;
        }
        public static int CalculateProfit(int bet){
            return (int)Math.Floor(2d*((double)bet/3d));
        }

        public static void PlayGame(){
            User CurrentUser = Saving.GetUser(Auth.Username);

            ResetGame();
            CurrentUser.PartidasJogadas++;

            //set bets
            Utils.Print("Digite a sua aposta!");
            int bet = Utils.GetIntInput(CurrentUser.Tokens);
            Utils.Print("Distribuindo as cartas!\n\n");
            Dealer.addCard(Baralho.GetCard());
            Player.addCard(Baralho.GetCard());
            Dealer.addCard(Baralho.GetCard());
            Player.addCard(Baralho.GetCard());
            
            //show cards
            Utils.Print("Cartas da casa: ");
            Dealer.RenderCards(true);
            Utils.Print();
            Utils.Print("Suas cartas: ");
            Player.RenderCards();

            //check if blackjack
            if(EvalBlackjack(Player)){
                Utils.Print("Você tem um BlackJack! Você venceu!");
                var profit = CalculateProfit(bet);
                Utils.Print($"+{profit} Tokens");
                CurrentUser.Tokens+=profit;
                CurrentUser.TokensWon+=profit;
                CurrentUser.Blackjacks++;
                CurrentUser.PartidasGanhas++;
                return;
            }

            bool isloopfinished = false;
            while(!isloopfinished){
                GameActions input = Menu.ShowGameActions();
                switch(input){
                    case GameActions.Comprar:
                        Card buycard = Baralho.GetCard();
                        Player.addCard(buycard);
                        Utils.Print("Suas cartas: ");
                        Player.RenderCards();

                        if(Player.CountCards()>21){ 
                            Utils.Print("Você passou de 21!");
                            isloopfinished = true;
                        }
                        else if(Player.CountCards() == 21){
                            Utils.Print("Você tem um BlackJack! Você venceu!");
                            var profit = CalculateProfit(bet);
                            Utils.Print($"+{profit} Tokens");
                            CurrentUser.Tokens+=profit;
                            CurrentUser.TokensWon+=profit;
                            CurrentUser.Blackjacks++;
                            CurrentUser.PartidasGanhas++;
                            return;
                        }
                        break;
                    case GameActions.Dobrar:
                        if(!(CurrentUser.Tokens >= bet*2)){
                            Utils.Print("Você não tem tokens o suficiente para dobrar");
                            continue;
                        }
                        bet*=2;
                        Utils.Print($"Aposta atual: {bet}");
                        buycard = Baralho.GetCard();
                        Player.addCard(buycard);
                        Player.RenderCards();

                        //check if blackjack
                        if(EvalBlackjack(Player)){
                            Utils.Print("Você tem um BlackJack! Você venceu!");
                            var profit = CalculateProfit(bet);
                            Utils.Print($"+{profit} Tokens");
                            CurrentUser.Tokens+=profit;
                            CurrentUser.TokensWon+=profit;
                            CurrentUser.Blackjacks++;
                            CurrentUser.PartidasGanhas++;
                            return;
                        }
                        break;
                    case GameActions.Parar:
                        isloopfinished=true;
                        break;
                }
            }
            //vez da casa
            //virar primeira carta
            Utils.Print("Cartas da casa: ");
            Dealer.RenderCards();
            Utils.Print($"A casa tem {Dealer.CountCards()}");
            Utils.Print("A casa vai começar a comprar agora.");
            Utils.Standby("aperque qualquer botão para iniciar a compra...");
            while(Dealer.CountCards() < 17){
                Card dbuycard = Baralho.GetCard();
                Dealer.addCard(dbuycard);
                Utils.Print("\nAs cartas da casa: ");
                Dealer.RenderCards();
                Utils.Print($"A casa tem {Dealer.CountCards()}");
            }

            //check results
            if(EvalBust(Dealer)){
                Utils.Print("A casa tem mais que 21, você ganhou!");
                var profit = CalculateProfit(bet);
                Utils.Print($"+{profit} Tokens");

                CurrentUser.Tokens += profit;
                CurrentUser.TokensWon += profit;
                CurrentUser.PartidasGanhas++;

            }else if(EvalBlackjack(Dealer)){
                Utils.Print("A casa tem 21, ela ganhou!");
                Utils.Print($"-{bet} Tokens");

                CurrentUser.Tokens -= bet;
                CurrentUser.TokensLost += bet;
                CurrentUser.PartidasPerdidas++;

            }else if(Dealer.CountCards() > Player.CountCards()){
                Utils.Print("A casa tem mais que você, perdeu!");
                Utils.Print($"-{bet} Tokens");

                CurrentUser.Tokens -= bet;
                CurrentUser.TokensLost += bet;
                CurrentUser.PartidasPerdidas++;

            }else if(Dealer.CountCards() < Player.CountCards() && Player.CountCards() <=21){
                Utils.Print("Você tem mais que a casa, ganhou!");
                var profit = CalculateProfit(bet);
                Utils.Print($"+{profit} Tokens");

                CurrentUser.Tokens += profit;
                CurrentUser.TokensWon += profit;
                CurrentUser.PartidasGanhas++;

            }else if(Dealer.CountCards() == Player.CountCards()){
                Utils.Print("Você tem a mesma quantidade que a casa, empate!");

                CurrentUser.PartidasEmpatadas++;
            }
            Saving.Update(CurrentUser);
        }
    }
}