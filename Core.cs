using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BlackJackJs{
    public class Card{
        public Naipe Naipe {get;}
        public int Numero {get;}
        public Card(int number, Naipe naipe){
            this.Naipe = naipe;
            this.Numero = number;
        }
        public override string ToString(){
            return this.Numero + " de "+ Enum.GetName(typeof(Naipe),this.Naipe);
        }
        public override int GetHashCode(){
            return HashCode.Combine<int,Naipe>(Numero,Naipe);
        }

        
    }
    public enum Naipe { 
        Paus, 
        Espadas,
        Ouros,
        Copas
    }
    public enum Regras{
        BlackJackClassic,
        Truco,
        Poker,
        Pife
    }
    public class Pilha{
        /// <summary>
        /// A List of all the cards the deck has
        /// </summary>
        /// <value></value>
        public List<Card> Cartas {get;set;}
        /// <summary>
        /// The amount of cards the deck has
        /// </summary>
        /// <value></value>
        public int Count {
            get {
                return Cartas.Count;
            }
            private set {}
        }
        public Pilha(){
            this.Cartas = new List<Card>();
        }
        public Pilha(List<Card> cartas){
            this.Cartas = cartas;
        }
        public Pilha(Card[] cartas){
            this.Cartas = new(cartas);
        }
        public void addCard(Card carta){
            this.Cartas.Add(carta);
        }
        public void RenderCards(bool hideone=false){
            if(!hideone){
                List<int> tempnmlist = new();
                List<Naipe> tempnplist = new();
                foreach(var card in Cartas){
                    tempnmlist.Add(card.Numero);
                    tempnplist.Add(card.Naipe);
                }
                Utils.Printf(Utils.GenStr(tempnmlist.ToArray(),tempnplist.ToArray(),Cartas.Count));
            }else{
                List<int> tempnmlist = new();
                List<Naipe> tempnplist = new();
                foreach(var card in Cartas){
                    tempnmlist.Add(card.Numero);
                    tempnplist.Add(card.Naipe);
                }
                Utils.Printf(Utils.GenStr(tempnmlist.ToArray(),tempnplist.ToArray(),Cartas.Count,true));
            }
            
        }
        public int CountCards(Regras rules = Regras.BlackJackClassic){
            int count=0;
            if(rules == Regras.BlackJackClassic){
                var hasHighValue = Cartas.AsQueryable()
                .Any(c=>c.Numero>=11 && c.Numero<=13);
                foreach(Card card in Cartas){
                    if(card.Numero<=10 && card.Numero!=1){
                        count+=card.Numero;
                        continue;
                    }else if(card.Numero>=11 && card.Numero<=13){
                        count+=10;
                        continue;
                    }else if(card.Numero==1){
                        if(hasHighValue){
                            count+=11;
                        }else{
                            count+=1;
                        }
                        continue;
                    }
                }
            }
            return count;

        }
        public bool removeCard(Card carta){
            return this.Cartas.Remove(carta);
        }
        public void removeCard(ushort index){
            this.Cartas.RemoveAt(index);
        }
        public void clear(){
            this.Cartas = new List<Card>();
        }
    }
    public class Baralho : Pilha{
        private Random rng {get;}

        /// <summary>
        /// Creates a new instance of a card deck
        /// </summary>
        public Baralho(){
            this.rng = new Random();
            this.Cartas = new();
            ResetBaralho();
        }

        /// <summary>
        /// Set all cards and shuffles
        /// </summary>
        public void ResetBaralho(){
            for(int i =0; i<13;i++){
                for(int j=0; j<4; j++){
                    this.Cartas.Add(new Card(i+1,(Naipe)j));
                }
            }
            Shuffle();
        }
        /// <summary>
        /// Shuffle the current deck of cards
        /// </summary>
        public void Shuffle(){
            for (double i = this.Cartas.Count - 1; i > 0; i--) {
                double j = Math.Floor(rng.NextDouble() * (i + 1));
                Card temp = this.Cartas[(int)i];
                this.Cartas[(int)i] = this.Cartas[(int)j];
                this.Cartas[(int)j] = temp;
            }
        }
        /// <summary>
        /// Grab a card from the deck, removes it afterwards
        /// </summary>
        /// <returns>The card that has been removed</returns>
        public Card GetCard(){
            Card c = Cartas[0];
            this.Cartas.RemoveAt(0);
            return c;
        }

        public override bool Equals(object obj)
        {
            return obj is Baralho baralho &&
                   EqualityComparer<Random>.Default.Equals(rng, baralho.rng) &&
                   EqualityComparer<List<Card>>.Default.Equals(Cartas,baralho.Cartas);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(rng, Cartas);
        }
    }
    public enum GameActions{
        Comprar,
        Dobrar,
        Parar,
        Dividir,
        Apostar
    }
    public enum MenuActions{
        Jogar,
        VerPerfil,
        VerHighscores,
        ConseguirMaisTokens,
        Deslogar,
        Sair 
    }
    public enum ProfileActions{
        VerSeuPerfil,
        VerOutroPerfil,
        DeletarSuaConta,
        AdicionarAmigo,
        VerAmigos,
        EnviarMsg,
        LerMsg,
        ResetarProgresso,
        VoltarMenu
    }
    public enum HighScoreSorting{
        Level,
        Tokens,
        TokensWon,
        TokensLost,
        Patente,
        MatchesWon,
        MatchesPlayed,
        MatchesLost,
        BlackJacks
    }
    public static class Actions{
        private static IDictionary<GameActions,string> _GameActions = new Dictionary<GameActions,string>(){
            {GameActions.Comprar,"Comprar"},
            {GameActions.Dobrar,"Dobrar"},
            {GameActions.Parar,"Parar"},
            {GameActions.Dividir,"Dividir"},
            {GameActions.Apostar,"Apostar"}
        };
        public static ReadOnlyDictionary<GameActions, string> GameActionsDict { get; } = new ReadOnlyDictionary<GameActions, string>(_GameActions);

        private static IDictionary<MenuActions,string> _MenuActions = new Dictionary<MenuActions,string>(){
            {MenuActions.Jogar,"Jogar (Paga 2:3)"},
            {MenuActions.VerPerfil,"ver o seu perfil ou de outro jogador"},
            {MenuActions.VerHighscores,"Ver o placar de l??deres"},
            {MenuActions.ConseguirMaisTokens,"Consiga mais Tokens"},
            {MenuActions.Deslogar,"Deslogar"},
            {MenuActions.Sair,"Sair do jogo"}
        };
        public static ReadOnlyDictionary<MenuActions, string> MenuActionsDict { get; } = new ReadOnlyDictionary<MenuActions, string>(_MenuActions);

        private static IDictionary<ProfileActions,string> _ProfileActions = new Dictionary<ProfileActions,string>(){
            {ProfileActions.VerSeuPerfil,"Ver o seu perfil"},
            {ProfileActions.VerOutroPerfil,"Ver o perfil de outro jogador"},
            {ProfileActions.AdicionarAmigo,"Adicionar este usu??rio como amigo"},
            {ProfileActions.VerAmigos,"Ver os amigos deste jogador"},
            {ProfileActions.EnviarMsg,"Enviar uma mensagem privada a este jogador"},
            {ProfileActions.LerMsg,"Ler as suas mensagens privadas"},
            {ProfileActions.VoltarMenu,"Voltar ao menu principal"},
            {ProfileActions.ResetarProgresso,"Sair do jogo"},
            {ProfileActions.DeletarSuaConta,"Deletar a sua conta"}
        };
        public static ReadOnlyDictionary<ProfileActions, string> ProfileActionsDict { get; } = new ReadOnlyDictionary<ProfileActions, string>(_ProfileActions);

        private static IDictionary<HighScoreSorting,string> _HighScoreSorting = new Dictionary<HighScoreSorting,string>(){
            {HighScoreSorting.BlackJacks,"Sortear por n??mero de BlackJacks"},
            {HighScoreSorting.Level,"Sortear pelo n??vel de experi??ncia"},
            {HighScoreSorting.MatchesLost,"Sortear pelo n??mero de partidas perdidas"},
            {HighScoreSorting.MatchesPlayed,"Sortear pelo n??mero de partidas jogadas"},
            {HighScoreSorting.MatchesWon,"Sortear pelo n??mero de partidas ganhas"},
            {HighScoreSorting.Patente,"Sortear pela patente competitiva"},
            {HighScoreSorting.Tokens,"Sortear pelo n??mero de Tokens"},
            {HighScoreSorting.TokensLost,"Sortear pelo n??mero de Tokens perdidos"},
            {HighScoreSorting.TokensWon,"Sortear pelo n??mero de Tokens lucrados"}
        };
        public static ReadOnlyDictionary<HighScoreSorting, string> HighScoreSortingDict { get; } = new ReadOnlyDictionary<HighScoreSorting, string>(_HighScoreSorting);

    }
    public class Message{

        public Message(string toUser, string content){
            this.ToUser = toUser;
            this.Content = content;
            this.WasRead = false;
            this.MessageId = Guid.NewGuid();
        }
        public bool WasRead { get; set;}
        public string ToUser {get;}
        public string Content {get;}
        public Guid MessageId {get;}
    }
}   

