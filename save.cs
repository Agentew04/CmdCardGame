using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        }

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

    }
}