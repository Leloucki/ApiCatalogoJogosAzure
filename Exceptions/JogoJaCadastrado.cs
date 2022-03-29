using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiCatalogoJogos.Exceptions
{
    public class JogoJaCadastrado : Exception
    {
        public static readonly string message = "Este jogo já está cadastrado";
        public JogoJaCadastrado() : base(message) { }
        public JogoJaCadastrado(string message) : base(message) { }
        public JogoJaCadastrado(string message, Exception inner) : base(message, inner) { }
    }
}
