using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace LYA1_Lexico3
{
    public class Lexico : Token, IDisposable
    {
        const int F = -1;
        const int E = -2;
        private StreamReader archivo;
        private StreamWriter log;

        int[,] TRAND =  
        {

            {0,1,2,27,1,19,20,8,10,11,12,13,17,18,22,22,24,25,27,28,32,33,F,F},
            {F,1,1,F,1,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,2,3,5,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {E,E,4,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,F,F},
            {F,F,4,F,5,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {E,E,7,E,E,6,6,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {E,E,7,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,F,F},
            {F,F,7,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,9,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,14,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,14,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,15,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,18,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,18,F,F,F,F,F,18,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,21,F,21,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,21,21,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,23,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,26,25,25,25,25,E,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,23,F,F,F,F,F,F,F,F,F,F,F,29,F,F,F,F},
            {29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,F,0},
            {30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,30,30,30,30,30,30,E,F},
            {30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,30,30,30,0,30,30,E,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},

        };
        public Lexico()
        {
            archivo = new StreamReader("prueba.cpp");
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        private int columna(char c)
        {
            if (char.IsWhiteSpace(c))
                return 0;
            else if (char.ToLower(c) == 'e')
                return 4;
            else if (char.IsLetter(c))
                return 1;
            else if (char.IsAsciiDigit(c))
                return 2;
            else if (c=='.')
                return 3;
            else if (c=='+')
                return 5;
            else if (c=='-')
                return 6;
            else if (c=='=')
                return 7;
            else if (c==';')
                return 8;
            else if (c=='&')
                return 9;
            else if (c=='|')
                return 10;
            else if(c=='!')
                return 11;
            else if (c=='<')
                return 12;
            else if (c=='>')
                return 13;
            else if (c=='%')
                return 14;
            else if (c=='*')
                return 15;
            else if (c=='?')
                return 16;
            else if (c=='"')
                return 17;
            else if (c=='/')
                return 19;
            else if (c=='{')
                return 20;
            else if (c=='}')
                return 21;
            else
                return 18;
        }
        private void clasificar(int estado)
        {
            switch (estado)
                {
                    
                    case 1:
                        setClasificacion(Tipos.Identificador);
                        break;
                    case 2:
                        setClasificacion(Tipos.Numero);
                        break;
                    case 8:
                        setClasificacion(Tipos.OpAsignacion);
                        break;
                    case 9:
                        setClasificacion(Tipos.OpRelacional);
                        break;
                    case 10:
                        setClasificacion(Tipos.FinSentencia);
                        break;
                    case 11:
                        setClasificacion(Tipos.Caracter);
                        break;
                    case 12:
                        setClasificacion(Tipos.Caracter);
                        break;
                    case 13:
                        setClasificacion(Tipos.OpLogico);
                        break;
                    case 14:
                        setClasificacion(Tipos.OpLogico);
                        break;
                    case 15:
                        setClasificacion(Tipos.OpRelacional);
                        break;
                    case 16:
                        setClasificacion(Tipos.OpRelacional);
                        break;
                    case 17:
                        setClasificacion(Tipos.OpRelacional);
                        break;
                    case 18:
                        setClasificacion(Tipos.OpRelacional);
                        break;
                    case 19:
                        setClasificacion(Tipos.OpTermino);
                        break;
                    case 20:
                        setClasificacion(Tipos.OpTermino);
                        break;
                    case 21:
                        setClasificacion(Tipos.IncrementoTermino);
                        break;
                    case 22:
                        setClasificacion(Tipos.OpFactor);
                        break;
                    case 23:
                        setClasificacion(Tipos.IncrementoFactor);
                        break;
                    case 24:
                        setClasificacion(Tipos.OpTernario);
                        break;
                    case 25:
                        setClasificacion(Tipos.Cadena);
                        break;
                    case 26:
                        setClasificacion(Tipos.Cadena);
                        break;
                    case 27:
                        setClasificacion(Tipos.Caracter);
                        break;
                    case 28:
                        setClasificacion(Tipos.OpFactor);
                        break;
                    case 29:
                        setClasificacion(Tipos.Comentario);
                        break;
                    case 30:
                        setClasificacion(Tipos.Comentario);
                        break;
                    case 32:
                        setClasificacion(Tipos.LlaveInicial);
                        break;
                    case 33:
                        setClasificacion(Tipos.LlaveFinal);
                        break;
                }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();

                estado = TRAND[estado,columna(c)];
                clasificar(estado);
                
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;    
                    }
                    archivo.Read();
                }
            }
            if (estado == E)
            {
                throw new Error("Lexico: Se espera un digito",log);
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + " = " + getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}